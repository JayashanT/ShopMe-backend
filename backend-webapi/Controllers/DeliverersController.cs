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
        
        [HttpGet]
        [Route("getDelivererNearByShop")]
        public IActionResult GetDelivererNearByShop()
        {
            double lat = 6.795134521923838;//5.953118046485079;
            double lng = 79.9003317207098;// 80.55386066436768;
            var deliverers=_delivererService.GetDelivererNearByShop(lat, lng);

            return Ok(deliverers);
        }

        [HttpPost]  
        [Route("waiting")]
        public IActionResult WaitingForDelivery(LocationVM deliveryLocation) //ask by deliverer 
        {
            _delivererService.UpdateDeliveryStatus(deliveryLocation.DelivererId, "online");
            var location = new LocationDto()
            {
                Id = 0,
                DelivererId = deliveryLocation.DelivererId,
                connectionId = deliveryLocation.connectionId,
                Latitude = deliveryLocation.Latitude,
                Longitude = deliveryLocation.Longitude
            };
            _locationService.UpdateDeliveryLocation(location);
            return Ok();
        }
        
        [HttpGet]
        [Route("x")]
        public IActionResult x() 
        {
            double lat = 6.795134521923838;//5.953118046485079;
            double lng = 79.9003317207098;// 80.55386066436768;
            var deliverers = _delivererService.GetDelivererNearByShop(lat, lng);
            return Ok(deliverers);
        }

        [HttpGet]
        [Route("response")]
        public bool DelivererResponse(string response)
        {
            return (response == "confirm") ? true : false;
        }
    }
}