using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {
        private IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdatePayment(int order_id, int status_code)
        {
            var result = _paymentService.UpdatePayment(order_id, status_code);
            return Ok(result);
        }
    }
}
