using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contract
{
    public interface IOrderService
    {
        Task<Result<OrderTOReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderTOReturnDto>>> GetAllForUserAsync(string email, CancellationToken ct = default);
        Task<Result<OrderTOReturnDto>> GetByIdAndEmailForUserAsync(Guid id, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodAsync(CancellationToken ct = default);
    }
}