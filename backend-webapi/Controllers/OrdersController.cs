using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using webapi.ViewModels;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private IPaymentService _paymentService;
        private IOrderService _orderService;

        public OrdersController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpPost]
        [Route("getAllOrderDetailsByCustomer/{id?}")]
        public IActionResult GetAllOrderDetailsByCustomer(int id) //customerId
        {
            var result = _orderService.GetAllOrderDetailsByCustomer(id);
            return Ok(result);
        }
        [HttpPost]
        [Route("getWaitingOrderDetailsBySeller/{id?}")]
        public IActionResult GetWaitingOrderDetailsBySeller(int id) 
        {
            var result = _orderService.GetWaitingOrderDetailsBySeller(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("updateOrderStatus/{id},{status}")]
        public IActionResult UpdateOrderStatus(int id, string status)
        {
            _orderService.UpdateOrderStatus(id, status);
            return Ok();
        }

        [HttpPost]
        [Route("payment")]
        public IActionResult Payment() //orderId, price
        {
            var result = _paymentService.CreateNewPayment(1, 260000);
            return Ok(result);
        }

        [HttpPost]
        [Route("calculateOrderPrice")]
        public IActionResult CalculateOrderPrice() //cutomerId, orderId
        {
            var amount = _paymentService.CalculateOrderPrice(2, 3);
            return Ok(amount);
        }


        [HttpPost]
        [Route("createNewOrder")]
        public IActionResult CreateNewOrder() //orderVM
        {
            var orderVM = new OrderVM()
            {
                CustomerId = 2,
                Items = new List<ItemVM>()
                {
                    new ItemVM()
                    {
                        ProductId=6,
                        Quantity=5
                    },
                    new ItemVM()
                    {
                        ProductId=6,
                        Quantity=5
                    },
                }
            };

            _orderService.CreateNewOrder(orderVM);
            return Ok(orderVM);
        }
    }
}