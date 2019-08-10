using System.Collections.Generic;
using backend_webapi.Dtos;
using webapi.Dtos;
using webapi.Entities;
using webapi.ViewModels;

namespace webapi.Services
{
    public interface IOrderService
    {
        void CreateNewOrder(OrderVM orderVM);
        object GetAllOrderDetailsByCustomer(int customerId);
        IEnumerable<OrderDto> GetAllOrdersByCustomer(int customerId);
        OrderDto GetOrderById(int customerId, int orderId);
        object GetOrdersNearByDeliverers(double latitude, double longitude);
        List<ProductDto> GetProductsByOrder(Order order);
        List<OrderDetails> GetWaitingOrderDetailsBySeller(int sellerId);
    }
}