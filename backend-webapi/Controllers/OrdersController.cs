using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.Services;
using webapi.ViewModels;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            
            _orderService = orderService;
        }

        [HttpGet]
        [Route("GetOrderDetailsById/{id?}")]
        public IActionResult GetOrderDetailsById(int id)
        {
            var result = _orderService.GetOrderDetailsById(id);
            return Ok(result);
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
        /*
        [HttpPost]
        [Route("getAllOrderDetailsByDeliverer/{id?}")]
        public IActionResult getAllOrderDetailsByDeliverer(int id) 
        {
            var result = _orderService.GetAllOrderDetailsByDeliverer(id);
            return Ok(result);
        }
        */
        [HttpGet]
        [Route("updateOrderStatus/{id},{status}")]
        public IActionResult UpdateOrderStatus(int id, string status)
        {
            _orderService.UpdateOrderStatus(id, status);
            return Ok();
        }

        [HttpPost]
        [Route("rate/{id},{sellerRate},{delivererRate}")]
        public IActionResult RateShop(int id, double sellerRate, double delivererRate)
        {
            _orderService.Rate(id, sellerRate, delivererRate);
            return Ok();
        }

        [HttpPost]
        [Route("createNewOrder")]
        public IActionResult CreateNewOrder(OrderVM orderVM) //orderVM
        {
            /*
            var orderVM = new OrderVM()
            {
                CustomerId = 1,
                CustomerLatitude = 6.7,
                CustomerLongitude=79,
                SellerId=4,
                Status="to be confirmed",
                Items = new List<ItemVM>()
                {
                    new ItemVM()
                    {
                        ProductId=5,
                        Quantity=5
                    },
                    new ItemVM()
                    {
                        ProductId=6,
                        Quantity=5
                    },
                }
            };
            */
            return Ok(_orderService.CreateNewOrder(orderVM));
        }

        [HttpPost]
        [Route("deleteOrder/{id?}")]
        public IActionResult DeleteOrder(int id) 
        {
            return Ok(_orderService.DeleteOrder(id));
        }
    }
}