using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contract
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreateOrUpdateAsync(string basketId, CancellationToken ct = default);
        Task PaymentSucceededAsync(string paymentIntentId, CancellationToken ct = default);
        Task PaymentFailedAsync(string paymentIntentId, CancellationToken ct = default);
    }
}
