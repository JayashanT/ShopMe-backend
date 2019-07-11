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
using webapi.Logic;

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
        [Route("getShopsNearBy")]
        public IActionResult GetDelivererNearByShop()
        {
            double lat = 6.795134521923838;//5.953118046485079;
            double lng = 79.9003317207098;// 80.55386066436768;
            var deliverer = _delivererService.GetDelivererNearByShop(lat, lng);

            return Ok(deliverer);
        }

        [HttpPost]
        [Route("waiting")]
        public IActionResult WaitingForDelivery(LocationDto deliveryLocation) //ask by deliverer 
        {
            _delivererService.UpdateDeliveryStatus(deliveryLocation.Id, "online");
            _locationService.UpdateDeliveryLocation(deliveryLocation);
            return Ok();
        }
        
        [HttpGet]
        [Route("x")]
        public IActionResult x() //ask by seller
        {
            double[,] arr = new double[1,2];
            arr[0, 0] = 6.795134521923838;
            arr[0, 1] = 79.9003317207098;
            return Ok(arr);
        }

        [HttpGet]
        [Route("response")]
        public bool DelivererResponse(string response)
        {
           // response = response;
            return (response == "confirm") ? true : false;
        }
    }
}