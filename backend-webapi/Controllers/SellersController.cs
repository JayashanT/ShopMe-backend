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

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellersController : Controller
    {
        private ICommonRepository<Seller> _sellerRepository;
        private ISellerService _sellerService;

        public SellersController(ICommonRepository<Seller> sellerRepository , ISellerService sellerService )
        {
            _sellerRepository = sellerRepository;
            _sellerService = sellerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allSellersDto = _sellerService.GetAllSellers();
            return Ok(allSellersDto);
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSingle(int id)
        {
            SellerDto sellerDto = _sellerService.GetSellerById(id); 
            if (sellerDto == null)
            {
                return NotFound();
            }
            return Ok(sellerDto);
        }

        [HttpPost]
        [Route("addSeller")]
        public IActionResult Add([FromBody] SellerDto  seller)
        {
            Seller toAdd = Mapper.Map<Seller>(seller);
                
            _sellerRepository.Add(toAdd);

            bool result = _sellerRepository.Save();

            if (!result)
            {
                return new StatusCodeResult(400);
            }
            return Ok(Mapper.Map<Seller>(toAdd));
        }

        [HttpGet]
        [Route("{latValue},{lngValue}")]
        public IActionResult GetNearestShop(double latValue, double lngValue)
        {
            var shop = _sellerService.GetShopsNearByLocation(latValue, lngValue);
            return Ok(shop);
        }
    }
}