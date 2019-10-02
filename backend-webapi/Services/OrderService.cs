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
        private IPaymentService _paymentService;
        private ICommonRepository<Deliverer> _delivererRepository;

        public OrderService(ICommonRepository<Order> orderRepository, ICommonRepository<OrderItem> orderItemRepository, ICommonRepository<Seller> sellerRepository, ICommonRepository<Payment> paymentRepository, ICommonRepository<Deliverer> delivererRepository,
                            ICommonRepository<Product> productRepository, ICommonRepository<OrderItemProduct> orderItemProductRepository, IProductService productService, ICommonRepository<Customer> customerRepository, IPaymentService paymentService)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderItemProductRepository = orderItemProductRepository;
            _productService = productService;
            _sellerRepository = sellerRepository;
            _paymentRepository = paymentRepository;
            _customerRepository = customerRepository;
            _paymentService = paymentService;
            _delivererRepository = delivererRepository;
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
                order.CustomerLongitude,
                order.DelivererId
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
            var orders = _orderRepository.Get(o => o.CustomerId == customerId).OrderByDescending(o=>o.CreatedAt);
            foreach (var order in orders)
            {
                var payment = _paymentRepository.Get(x => x.OrderId == order.Id).FirstOrDefault();
                var shop = _sellerRepository.Get(x => x.Id == order.SellerId).FirstOrDefault();
                var productDeatails = GetProductsByOrder(order);
                var orderItems = new OrderDetails()
                {
                    Id = order.Id,
                    CreatedAt = payment.PaymentDate,
                    Products = productDeatails,
                    OrderStatus = order.Status,
                    TotalPrice = payment.Price,
                    ShopName = shop.ShopName       
                };

                orderDetails.Add(orderItems);
            }
            return orderDetails;
        }

        //GetAllOrderDetailsByDeliverer
        public List<OrderDeliveryDetails> GetAllOrderDetailsByDeliverer(int delivererId)
        {
            var orderDeliveryDetailsList = new List<OrderDeliveryDetails>();
            var orders = _orderRepository.Get(o =>o.Status== "confirm order recieved" && o.DelivererId == delivererId).OrderByDescending(o => o.CreatedAt);
            
            foreach (var order in orders)
            {
                var seller = _sellerRepository.Get(x => x.Id == order.SellerId).FirstOrDefault();
                var customer = _customerRepository.Get(x=>x.Id==order.CustomerId).FirstOrDefault();
                seller.AccountNo = null;
                seller.ConnectionId = null;
                var orderDeliveryDetails = new OrderDeliveryDetails
                {
                    customer = Mapper.Map<CustomerDto>(customer) ,
                    seller = Mapper.Map<SellerDto>(seller)
                };
                orderDeliveryDetailsList.Add(orderDeliveryDetails);
            }
            return orderDeliveryDetailsList;
        }
        

        //GetWaitingOrderDetailsBySeller
        public List<OrderDetails> GetWaitingOrderDetailsBySeller(int sellerId)
        {
            var orderDetails = new List<OrderDetails>(); 
            var orders = _orderRepository.Get(o => o.SellerId == sellerId && o.Status == "to be confirmed").OrderByDescending(o => o.CreatedAt);
            
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

        //Rate
        public void Rate(int id, double sellerRate, double delivererRate)
        {
            var order = _orderRepository.Get(x => x.Id == id).FirstOrDefault();
            var seller = _sellerRepository.Get(x => x.Id == order.SellerId).FirstOrDefault();
            var deliverer = _delivererRepository.Get(x => x.Id == order.DelivererId).FirstOrDefault();

            deliverer.Rating = Math.Round((deliverer.Rating + delivererRate)/2, 1);
            _delivererRepository.Update(deliverer);
            _delivererRepository.Save();

            seller.Rating = (seller.Rating + sellerRate) / 2;
            _sellerRepository.Update(seller);
            _sellerRepository.Save();
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
        public OrderDto CreateNewOrder(OrderVM orderVM)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    OrderDto orderDto = new OrderDto()
                    {
                        CreatedAt = DateTime.Now,
                        CustomerId = orderVM.CustomerId,
                        CustomerLatitude=orderVM.CustomerLatitude,
                        CustomerLongitude=orderVM.CustomerLongitude,
                        SellerId=orderVM.SellerId,
                        Status=orderVM.Status,
                    };

                    Order orderToAdd = Mapper.Map<Order>(orderDto);
                    _orderRepository.Add(orderToAdd);
                    bool orderResult = _orderRepository.Save();
                    bool x = orderResult;
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

                    //payment table create
                    double price = _paymentService.CalculateOrderPrice(orderToAdd.CustomerId, orderToAdd.Id);
                    _paymentService.CreateNewPayment(orderToAdd.Id, price);

                    scope.Complete();
                    return Mapper.Map<OrderDto>(orderToAdd);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(new Exception(ex.Message));
                return null;
            }
        }

        //delete order 
        public bool DeleteOrder(int orderId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Order order = _orderRepository.Get(x => x.Id == orderId).FirstOrDefault();
                    List<OrderItem> orderItems = _orderItemRepository.Get(x=>x.OrderId==order.Id).ToList();

                    foreach (var orderItem in orderItems)
                    {
                        OrderItemProduct orderItemProduct = _orderItemProductRepository.Get(x => x.OrderItemId == orderItem.Id).FirstOrDefault();

                        //increment Quantity in Product table
                        _productService.IncrementProductQuentity(orderItemProduct.ProductId, orderItem.Quantity);
                        _orderItemProductRepository.Remove(orderItemProduct);
                        _orderItemProductRepository.Save();
                        _orderItemRepository.Remove(orderItem);
                        _orderItemRepository.Save();

                    }
                    _orderRepository.Remove(order);
                    _orderRepository.Save();
                    //Payment payment = _paymentRepository.Get(x=>x.OrderId==orderId).FirstOrDefault();
                    //_paymentRepository.Remove(payment);
                    //_paymentRepository.Save();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(new Exception(ex.Message));
                return false;
            }
        }
    }
}