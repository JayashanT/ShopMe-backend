using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Entities;
using webapi.ViewModels;
using webapi.Services;
using webapi.Dtos;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class UserAuthController : Controller
    {
        private IUserService _userService;

        public UserAuthController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public IActionResult SignIn([FromBody]LoginVM login)
        {
            var user = _userService.SignIn(login.Email, login.Password);

            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });
        
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("SignUp-Customer")]
        public IActionResult SignUp([FromBody]CustomerVM customerVM)
        {
            var customer = _userService.SignUp(customerVM);

            if (customer == null)
                return BadRequest(new { message = "fail" });

            return Ok(customer);
        }

        [AllowAnonymous]
        [HttpPost("SignUp-Seller")]
        public IActionResult SignUp([FromBody]SellerVM sellerVM)
        {
            var seller = _userService.SignUp(sellerVM);

            if (seller == null)
                return BadRequest(new { message = "fail" });

            return Ok(seller);
        }

        [AllowAnonymous]
        [HttpPost("SignUp-Deliverer")]
        public IActionResult SignUp([FromBody]DelivererVM delivererVM)
        {
            var deliverer = _userService.SignUp(delivererVM);

            if (deliverer == null)
                return BadRequest(new { message = "fail" });

            return Ok(deliverer);
        }
    }
}