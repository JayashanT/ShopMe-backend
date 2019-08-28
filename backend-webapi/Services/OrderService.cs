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
using GeoCoordinatePortable;
using backend_webapi.Dtos;

namespace webapi.Services
{
    public class OrderService : IOrderService
    {
        private ICommonRepository<Order> _orderRepository;
        private ICommonRepository<OrderItem> _orderItemRepository;
        private ICommonRepository<Product> _productRepository;
        private ICommonRepository<OrderItemProduct> _orderItemProductRepository;
        private ICommonRepository<Seller> _sellerRepository;
        private IProductService _productService;
        private ICommonRepository<Payment> _paymentRepository;
        private ICommonRepository<Customer> _customerRepository;

        public OrderService(ICommonRepository<Order> orderRepository, ICommonRepository<OrderItem> orderItemRepository, ICommonRepository<Seller> sellerRepository, ICommonRepository<Payment> paymentRepository,
                            ICommonRepository<Product> productRepository, ICommonRepository<OrderItemProduct> orderItemProductRepository, IProductService productService, ICommonRepository<Customer> customerRepository)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderItemProductRepository = orderItemProductRepository;
            _productService = productService;
            _sellerRepository = sellerRepository;
            _paymentRepository = paymentRepository;
            _customerRepository = customerRepository;
        }

        //GetOrderById
        public object GetOrderDetailsById(int orderId)
        {
            var order = _orderRepository.Get(x => x.Id == orderId).SingleOrDefault();
            var products = GetProductsByOrder(order);
            var sellerDetails = _sellerRepository.Get(order.SellerId);
            var customerDetails = _customerRepository.Get(order.CustomerId);
            return new
            {
                products,
                sellerDetails.ShopName,
                sellerDetails.ShopLocationLatitude,
                sellerDetails.ShopLocationLongitude,
                customerDetails.FirstName,
                customerDetails.LastName,
                order.CustomerLatitude,
                order.CustomerLongitude

            };
        }

        //GetAllOrdersByCustomer
        public IEnumerable<OrderDto> GetAllOrdersByCustomer(int customerId)
        {
            var allOrders = _orderRepository.Get(x => x.CustomerId == customerId).ToList();
            var allOrdersDetails = allOrders.Select(x => Mapper.Map<OrderDto>(x));
            return allOrdersDetails;
        }

        //GetAllOrderDetailsByCustomer
        public List<OrderDetails> GetAllOrderDetailsByCustomer(int customerId)
        {
            var orderDetails = new List<OrderDetails>();
            var orders = _orderRepository.Get(o => o.CustomerId == customerId);

            foreach (var order in orders)
            {
                var payment = _paymentRepository.Get(x => x.OrderId == order.Id).FirstOrDefault();
                var productDeatails = GetProductsByOrder(order);
                var orderItems = new OrderDetails()
                {
                    Id = order.Id,
                    CreatedAt = payment.PaymentDate,
                    Products = productDeatails,
                    OrderStatus = order.Status,
                    TotalPrice = payment.Price,
                };

                orderDetails.Add(orderItems);
            }
            return orderDetails;
        }

        //GetWaitingOrderDetailsBySeller
        public List<OrderDetails> GetWaitingOrderDetailsBySeller(int sellerId)
        {
            var orderDetails = new List<OrderDetails>(); 
            var orders = _orderRepository.Get(o => o.SellerId == sellerId && o.Status == "to be confirmed");
            
            foreach (var order in orders)
            {
                var productDeatails = GetProductsByOrder(order);
                var orderItems = new OrderDetails()
                {
                    Id = order.Id,
                    OrderStatus=order.Status,
                    Products = productDeatails
                };

                orderDetails.Add(orderItems);
            }
            return orderDetails;
        }

        //GetProductsByOrder
        public List<ProductDto> GetProductsByOrder(Order order)
        {
            List<ProductDto> productsList = new List<ProductDto>();
            
            var orderItems = _orderItemRepository.Get(x => x.OrderId == order.Id);
            foreach (var orderItem in orderItems)
            {
                var quantity = orderItem.Quantity;
                var itemproducts = _orderItemProductRepository.Get(x => x.OrderItemId == orderItem.Id);
                foreach (var item in itemproducts)
                {
                    var product = _productRepository.Get(x => x.Id == item.ProductId).FirstOrDefault();
                    product.Quantity = quantity;
                    productsList.Add(Mapper.Map<ProductDto>(product));
                }
                
            }

            return productsList;
        }

        //GetAllOrderDetailsBySeller
        public Object GetOrdersNearByDeliverers(double latitude , double longitude)
        {
            var source = new GeoCoordinate() { Latitude = latitude, Longitude = longitude };

            var query = (
                        from s in _sellerRepository.GetAll()
                        from o in _orderRepository.Get(o => o.Status=="to be delivered" && o.SellerId==s.Id)

                        where new GeoCoordinate() { Latitude = s.ShopLocationLatitude, Longitude=s.ShopLocationLongitude }.GetDistanceTo(source) < 20000

                        orderby new GeoCoordinate() { Latitude = s.ShopLocationLatitude, Longitude = s.ShopLocationLongitude }.GetDistanceTo(source) ascending

                        select new { s.Id }
                        );
            return query;
        }


        //UpdateOrderStatus
        public void UpdateOrderStatus(int id, string status)
        {
            var order = _orderRepository.Get(x => x.Id == id).First();
            order.Status = status;
            _orderRepository.Update(order);
            _orderRepository.Update(order);
            _orderRepository.Save();
        }

        //CreateNewOrder
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