using System;
using System.Collections.Generic;
using webapi.Dtos;
using webapi.ViewModels;

namespace webapi.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetAllOrdersByCustomer(int customerId);
        OrderDto GetOrderById(int customerId, int orderId);
        Object GetAllOrderDetailsByCustomer(int customerId);
        void CreateNewOrder(OrderVM orderVM);
    }
}