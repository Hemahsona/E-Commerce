using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contract.IRepositories;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<Result<OrderTOReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetAsync(orderDto.BasketId, ct);
            if(basket == null)
                return Error.NotFound("Basket not found", $"Basket with ID {orderDto.BasketId} not found.");
            if(!basket.Items.Any())
                return Error.Validation("Basket is empty", $"can not create order with basket id {orderDto.BasketId}.");

            var orderItem = new List<OrderItem>(basket.Items.Count);
            var productIds = basket.Items.Select(item => item.Id).ToHashSet();
            var productItem = (await unitOfWork.Repository<Product, int>()
                  .GetAllAsync(new ProductWithSpecifications(productIds), ct)).ToDictionary(p => p.Id);
            foreach (var item in basket.Items)
            {
                if(!productItem.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product not found", $"Product with ID {item.Id} not found.");
                orderItem.Add(new OrderItem
                {
                    Quantity = item.Quantity,
                    Price = product.Price,
                    Product = new ProductItemOrdered
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    }
                });
            }

            var orderAddress = new OrderAddress
            {
                FirstName = orderDto.ShipToAddress.FirstName,
                LastName = orderDto.ShipToAddress.LastName,
                Street = orderDto.ShipToAddress.Street,
                City = orderDto.ShipToAddress.City,
                Country = orderDto.ShipToAddress.Country
            };

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>()
                .GetByIdAsync(orderDto.DeliveryMethodId, ct);
            if(deliveryMethod == null)
                return Error.NotFound("Delivery method not found", $"Delivery method with ID {orderDto.DeliveryMethodId} not found.");

            var subtotal = orderItem.Sum(item => item.Price * item.Quantity);

            var order = new Order(email, orderAddress, orderItem, deliveryMethod, subtotal);
            await unitOfWork.Repository<Order, Guid>().AddAsync(order);
            var result = await unitOfWork.SaveChangesAsync(ct);
            if(result == 0)
                return Error.Failure("Order creation failed", "Failed to create order.");
            else
            {
                await basketRepository.DeleteAsync(basket.Id, ct);
                return new OrderTOReturnDto
                {
                    Id = order.Id,
                    BuyerEmail = order.BuyerEmail,
                    ShipToAddress = new AddressDto
                    {
                        FirstName = order.ShipToAddress.FirstName,
                        LastName = order.ShipToAddress.LastName,
                        Street = order.ShipToAddress.Street,
                        City = order.ShipToAddress.City,
                        Country = order.ShipToAddress.Country
                    },
                    DeliveryMethodCost = deliveryMethod.Cost,
                    DeliveryMethod = deliveryMethod.Description,
                    SubTotal = order.SubTotal,
                    Total = order.Total,
                    dateTime = order.OrderDate,
                    Status = order.Status.ToString(),
                    Items = order.Items.Select(item => new OrderItemDto
                    {
                        Price = item.Price,
                        ProductId = item.Product.ProductId,
                        ProductUrl= item.Product.PictureUrl,
                        ProductName = item.Product.ProductName,
                        Quantity = item.Quantity,
                    }).ToList(),

                };
            }
                


        }

        public async Task<Result<IReadOnlyList<OrderTOReturnDto>>> GetAllForUserAsync(string email, CancellationToken ct = default)
        {
            var orders = await unitOfWork.Repository<Order, Guid>().GetAllAsync(new OrderWithSpecifications(email));
            if (!orders.Any())
                return Error.NotFound("order not found", $"order with email {email} not found.");
            var result = orders.Select(order => new OrderTOReturnDto
            {
                BuyerEmail = order.BuyerEmail,
                dateTime = order.OrderDate,
                DeliveryMethod = order.DeliveryMethod.ToString(),
                Id = order.Id,
                Status = order.Status.ToString(),
                SubTotal = order.SubTotal,
                Total= order.Total,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    Price = item.Price,
                    ProductId = item.Product.ProductId,
                    ProductUrl = item.Product.PictureUrl,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                }).ToList()

            }).ToList();
            return Result<IReadOnlyList<OrderTOReturnDto>>.Ok(result);
        }

        public async Task<Result<OrderTOReturnDto>> GetByIdAndEmailForUserAsync(Guid id, string email, CancellationToken ct = default)
        {
            var order = await unitOfWork.Repository<Order, Guid>().GetByIdAsync(new OrderWithSpecifications(id, email));
            if (order == null)
                return Error.NotFound("order not found", $"order with Id {id} not found.");
            var result = new OrderTOReturnDto
            {
                BuyerEmail = order.BuyerEmail,
                dateTime = order.OrderDate,
                DeliveryMethod = order.DeliveryMethod.ToString(),
                Id = order.Id,
                Status = order.Status.ToString(),
                SubTotal = order.SubTotal,
                Total = order.Total,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    Price = item.Price,
                    ProductId = item.Product.ProductId,
                    ProductUrl = item.Product.PictureUrl,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                }).ToList()
            };
            return Result<OrderTOReturnDto>.Ok(result);
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodAsync(CancellationToken ct = default)
        {
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync(ct);
            if (!deliveryMethod.Any())
                return Error.NotFound("Delivery method not found");
            var result = deliveryMethod.Select(dm => new DeliveryMethodDto
            {
                Id = dm.Id,
                Cost = dm.Cost,
                DeliveryTime = dm.DeliveryTime,
                Description = dm.Description,
                ShortName = dm.ShortName,
            }).ToList();

            return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(result);
        }
    }
}
