using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contract.IRepositories;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class PaymentService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway,
        IOptions<PaymentGatewaySetting> options) : IPaymentService
    {
        public async Task<Result<BasketDto>> CreateOrUpdateAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetAsync(basketId, ct);
            if (basket == null)
                return Result<BasketDto>.Fail(new Error("Basket not found", $"Basket with the given ID {basketId} does not exist."));
            if (!basket.Items.Any())
                return Error.Validation("Basket is empty");

            if(!basket.DeliveryMethod.HasValue)
                return Error.Validation("delivery method Id is required");

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethod.Value, ct);
            if(deliveryMethod == null)
                return Error.NotFound("Delivery method not found", $"Delivery method with the given ID {basket.DeliveryMethod.Value} does not exist.");

            basket.ShippingPrice = deliveryMethod.Cost;


            var productIds = basket.Items.Select(i => i.Id).ToHashSet();
            var products = (await unitOfWork.Repository<Product, int>()
                .GetAllAsync(new ProductWithSpecifications(productIds), ct)).ToDictionary(x=> x.Id);
            foreach (var item in basket.Items)
            {
                if(!products.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product not found", $"Product with the given ID {item.Id} does not exist.");
                item.Price = product.Price;
            }

            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);
            var amount = (long)((subTotal + basket.ShippingPrice) * 100m);

            if(string.IsNullOrEmpty(basket.PaymentIntent))
            {
                var result = await paymentGateway.CreatePaymentIntentAsync(amount, options.Value.DefaultCurrency, ct);
                basket.PaymentIntent = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else
            {
                var result = await paymentGateway.UpdatePaymentIntentAsync(amount, basket.PaymentIntent, ct);
                basket.ClientSecret = result.ClientSecret;
            }

            await basketRepository.CreateOrUpdateAsync(basket, ct:ct);

            return new BasketDto
            {
                Id = basket.Id,
                Items = basket.Items.Select(i => new BasketItemDto
                {
                    Id = i.Id,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    PictureUrl = i.PictureUrl
                }).ToList(),
                DeliveryMethodId = basket.DeliveryMethod,
                ShippingPrice = basket.ShippingPrice,
                PaymentIntentId = basket.PaymentIntent,
                ClientSecret = basket.ClientSecret
            };

        }

        public async Task PaymentFailedAsync(string paymentIntentId, CancellationToken ct = default)
        {
            var order = await unitOfWork.Repository<Order, Guid>().GetByIdAsync(new PaymentIntentSpecifications(paymentIntentId), ct);
            if (order == null)
                return;
            order.Status = OrderStatus.PaymentFailed;
            await unitOfWork.SaveChangesAsync(ct);
        }

        public async Task PaymentSucceededAsync(string paymentIntentId, CancellationToken ct = default)
        {
            var order = await unitOfWork.Repository<Order, Guid>().GetByIdAsync(new PaymentIntentSpecifications(paymentIntentId), ct);
            if (order == null)
                return;
            order.Status = OrderStatus.PaymentReceived;

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}
