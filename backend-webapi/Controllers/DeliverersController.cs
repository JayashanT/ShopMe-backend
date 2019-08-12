using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Entities;
using webapi.ViewModels;
using webapi.Repositories;
using webapi.Services;
using System.Threading;
using backend_webapi;
using backend_webapi.ViewModels;

namespace webapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DeliverersController : Controller
    {
        private ICommonRepository<Deliverer> _delivererRepository;
        private IDelivererService _delivererService;
        private ILocationService _locationService;

        public DeliverersController(ICommonRepository<Deliverer> delivererRepository, IDelivererService delivererService,
                                    ILocationService locationService)
        {
            _delivererRepository = delivererRepository;
            _delivererService = delivererService;
            _locationService = locationService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allDeliverers = _delivererRepository.GetAll().ToList();
            var allDeliverersDto = allDeliverers.Select(x => Mapper.Map<DelivererDto>(x));
            return Ok(allDeliverersDto);
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSingle(int id)
        {
            Deliverer delivererFromRepo = _delivererRepository.Get(id);
            if (delivererFromRepo == null)
            {
                return NotFound();
            }
            return Ok(delivererFromRepo);
        }
        
        [HttpGet]
        [Route("updateDeliveryStatus/{id},{status}")]
        public IActionResult UpdateDeliveryStatus(int id, string status)
        {
            _delivererService.UpdateDeliveryStatus(id, status);
            return Ok();
        }

        [HttpPost]
        [Route("addDeliverer")]
        public IActionResult Add([FromBody] DelivererDto deliverer)
        {
            Deliverer toAdd = Mapper.Map<Deliverer>(deliverer);

            _delivererRepository.Add(toAdd);

            bool result = _delivererRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(400);
            }
            return Ok(Mapper.Map<DelivererDto>(toAdd));
        }
    }
}