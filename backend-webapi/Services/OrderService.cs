using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using webapi.Dtos;
using webapi.Entities;
using webapi.Repositories;
using webapi.ViewModels;
using webapi.Services;

namespace webapi.Services
{
    public class OrderService : IOrderService
    {
        private ICommonRepository<Order> _orderRepository;
        private ICommonRepository<OrderItem> _orderItemRepository;
        private ICommonRepository<Product> _productRepository;
        private ICommonRepository<OrderItemProduct> _orderItemProductRepository;
        private IProductService _productService;

        public OrderService(ICommonRepository<Order> orderRepository, ICommonRepository<OrderItem> orderItemRepository,
                            ICommonRepository<Product> productRepository, ICommonRepository<OrderItemProduct> orderItemProductRepository, IProductService productService)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderItemProductRepository = orderItemProductRepository;
            _productService = productService;
        }

        public OrderDto GetOrderById(int customerId, int orderId)
        {
            var order = _orderRepository.Get(x => x.CustomerId == customerId && x.Id == orderId).SingleOrDefault();
            return Mapper.Map<OrderDto>(order);
        }
        public IEnumerable<OrderDto> GetAllOrdersByCustomer(int customerId)
        {
            var allOrders = _orderRepository.Get(x => x.CustomerId == customerId).ToList();
            var allOrdersDetails = allOrders.Select(x => Mapper.Map<OrderDto>(x));
            return allOrdersDetails;
        }

        public Object GetAllOrderDetailsByCustomer(int customerId)
        {
            var query = (
                        from o in _orderRepository.GetAll()
                        where o.CustomerId == customerId

                        from oi in _orderItemRepository.GetAll()
                        where oi.OrderId == o.Id

                        from oip in _orderItemProductRepository.GetAll()
                        where oip.OrderItemId == oi.Id

                        from p in _productRepository.GetAll()
                        where oip.ProductId == p.Id

                        orderby oi.OrderId 
                        select new { o.CreatedAt, p.Description, oi.Quantity }
                        );
            return query;
        }


        public void CreateNewOrder(OrderVM orderVM)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    OrderDto orderDto = new OrderDto()
                    {
                        CreatedAt = DateTime.Now,
                        CustomerId = orderVM.CustomerId
                    };

                    Order orderToAdd = Mapper.Map<Order>(orderDto);
                    _orderRepository.Add(orderToAdd);
                    bool orderResult = _orderRepository.Save();

                    foreach (var orderItem in orderVM.Items)
                    {

                        OrderItemDto orderItemDto = new OrderItemDto()
                        {
                            OrderId = Mapper.Map<Order>(orderToAdd).Id, //OrderId
                            Quantity = orderItem.Quantity
                        };

                        //Map
                        OrderItem orderItemToAdd = Mapper.Map<OrderItem>(orderItemDto);
                        _orderItemRepository.Add(orderItemToAdd);
                        _orderItemRepository.Save();

                        OrderItemProductDto orderItemProductDto = new OrderItemProductDto()
                        {
                            OrderItemId = Mapper.Map<OrderItem>(orderItemToAdd).Id,
                            ProductId = orderItem.ProductId
                        };

                        //Map
                        OrderItemProduct orderItemProductToAdd = Mapper.Map<OrderItemProduct>(orderItemProductDto);
                        _orderItemProductRepository.Add(orderItemProductToAdd);
                        bool orderItemProductResult = _orderItemProductRepository.Save();

                        //reduce Quantity in Product table
                        _productService.ReduceProductQuentity(orderItem.ProductId, orderItem.Quantity);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(new Exception(ex.Message));
            }
        }
    }
}