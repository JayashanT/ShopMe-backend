using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Services;

namespace webapi.Controllers
{
    public class PaymentsController : Controller
    {
        private IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdatePayment(int order_id, int status_message)
        {
            var result = _paymentService.UpdatePayment(order_id, status_message);
            return Ok(result);
        }
    }
}
