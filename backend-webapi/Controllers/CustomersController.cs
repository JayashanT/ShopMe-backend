using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Entities;
using webapi.ViewModels;
using webapi.Repositories;
using webapi.Services;
using Microsoft.AspNetCore.Cors;

namespace webapi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private ICommonRepository<Customer> _customerRepository;
        public CustomersController(ICommonRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allcustomers = _customerRepository.GetAll().ToList();
            var allcustomersDto = allcustomers.Select(x => Mapper.Map<CustomerDto>(x));
            return Ok(allcustomersDto);
        }


        [HttpPost]
        [Route("{id}")]
        public IActionResult GetSingle(int id)
        {
            Customer customerFromRepo = _customerRepository.Get(id);
            if (customerFromRepo == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<CustomerDto>(customerFromRepo));
        }
        

        //post  api/addCustomers
        [HttpPost]
        [Route("addCustomer")]
        public IActionResult Add([FromBody] CustomerDto customer)
        {
            Customer toAdd = Mapper.Map<Customer>(customer);

            _customerRepository.Add(toAdd);

            bool result = _customerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(400);
            }
            return Ok(Mapper.Map<CustomerDto>(toAdd));
        }
    }
}