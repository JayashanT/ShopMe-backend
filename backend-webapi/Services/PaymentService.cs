using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Dtos;
using webapi.Entities;
using webapi.Repositories;

namespace webapi.Services
{
    public class PaymentService : IPaymentService
    {
        private ICommonRepository<Payment> _paymentRepository;
        private ICommonRepository<Order> _orderRepository;
        private ICommonRepository<OrderItem> _orderItemRepository;
        private ICommonRepository<Product> _productRepository;
        private ICommonRepository<OrderItemProduct> _orderItemProductRepository;

        public PaymentService(ICommonRepository<Order> orderRepository, ICommonRepository<OrderItem> orderItemRepository,
                              ICommonRepository<Product> productRepository, ICommonRepository<OrderItemProduct> orderItemProductRepository,
                              ICommonRepository<Payment> paymentRepository)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderItemProductRepository = orderItemProductRepository;
            _paymentRepository = paymentRepository;
        }

        public double CalculateOrderPrice(int customerId, int orderId)
        {
            var query = (
                         from o in _orderRepository.GetAll()
                         where o.CustomerId==customerId
                         where o.Id==orderId

                         from oi in _orderItemRepository.GetAll()
                         where oi.OrderId==orderId

                         from oip in _orderItemProductRepository.GetAll()
                         where oip.OrderItemId==oi.Id

                         from p in _productRepository.GetAll()
                         where oip.ProductId==p.Id
                        
                         select new { UnitPrice=p.UnitPrice, Quantity=oi.Quantity }
                      );

            var totalPrice=0.0;
            foreach (var q in query)
            {
                totalPrice += q.UnitPrice*q.Quantity;
            }

            return totalPrice;
        }

        public PaymentDto CreateNewPayment(int orderId, double price)
        {
            PaymentDto paymentDto = new PaymentDto()
            {
                OrderId = orderId,
                Price = price,
                PaymentDate = DateTime.Now
            };

            Payment toAdd = Mapper.Map<Payment>(paymentDto);
            _paymentRepository.Add(toAdd);
            bool result = _paymentRepository.Save();
            return result ? Mapper.Map<PaymentDto>(toAdd) :  null;
        }
    }
}