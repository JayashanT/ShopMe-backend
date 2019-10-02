using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc;
using webapi.Dtos;
using webapi.Entities;
using webapi.Repositories;

namespace webapi.Services
{
    public class SellerService : ISellerService
    {
        private ICommonRepository<Seller> _sellerRepository;

        public SellerService(ICommonRepository<Seller> sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }
        
        //GetShopNearByLocation
        public IEnumerable<ShopDetails> GetShopsNearByLocation(double latitude, double longitude)
        {
            var source = new GeoCoordinate() { Latitude=latitude, Longitude=longitude };

            var query = (
                        from sellerShop in _sellerRepository.GetAll()

                        where new GeoCoordinate() { Latitude = sellerShop.ShopLocationLatitude, Longitude = sellerShop.ShopLocationLongitude }.GetDistanceTo(source) < 20000

                        select new ShopDetails{
                            Id=sellerShop.Id,
                            FirstName=sellerShop.FirstName,
                            LastName=sellerShop.LastName,
                            Image=sellerShop.Image,
                            MobileNumber=sellerShop.MobileNumber,
                            ShopAddress=sellerShop.ShopAddress,
                            Distance= new GeoCoordinate() { Latitude = sellerShop.ShopLocationLatitude, Longitude = sellerShop.ShopLocationLongitude }.GetDistanceTo(source)/1000,
                            ShopLocationLatitude=sellerShop.ShopLocationLatitude,
                            ShopLocationLongitude = sellerShop.ShopLocationLongitude,
                            ShopName = sellerShop.ShopName,
                            Rating= sellerShop.Rating
                        }
                        ).ToList();

            return query;
        }
        //UpdateSellerConnectionId
        public bool UpdateSellerConnectionId(int id, string connectionId)
        {
            var sellerinDb = _sellerRepository.Get(x => x.Id == id).First();

            if (sellerinDb != null)
            {
                sellerinDb.ConnectionId = connectionId;
                _sellerRepository.Update(sellerinDb);
                return _sellerRepository.Save();
            }
            return false;
        }

        //GetAllSellers
        public IEnumerable<SellerDto> GetAllSellers()
        {
            var allSellers = _sellerRepository.GetAll().ToList();
            var allSellersDetails = allSellers.Select(x => Mapper.Map<SellerDto>(x));
            return allSellersDetails;
        }

        //GetSellerById
        public SellerDto GetSellerById(int id)
        {
            Seller sellerrFromRepo = _sellerRepository.Get(id);
            return Mapper.Map<SellerDto>(sellerrFromRepo);
        }

        //GetSellersByLocation
        public IEnumerable<SellerDto> GetSellersByLocation(int locationId)
        {
            var allSellers = _sellerRepository.GetAll().ToList();
            var allSellersDto = allSellers.Select(x => Mapper.Map<SellerDto>(locationId));
            return allSellersDto;
        }

        //CreateNewSeller
        public bool CreateNewSeller(SellerDto sellerDto)
        {
            Seller toAdd = Mapper.Map<Seller>(sellerDto);

            _sellerRepository.Add(toAdd);

            bool result = _sellerRepository.Save();

            return result;
        }

        //DeleteSeller
        public bool DeleteSeller(int id)
        {
            var seller = _sellerRepository.Get(x => x.Id == id).First();

            if (seller != null)
            {
                _sellerRepository.Remove(seller);
                _sellerRepository.Save();
                return true;
            }
            return false;
        }

        //UpdateSeller
        public bool UpdateSeller(SellerDto sellerDto)
        {
            var sellerinDb = _sellerRepository.Get(x => x.Id == sellerDto.Id).First();

            if (sellerinDb != null)
            {
                Seller seller = Mapper.Map<Seller>(sellerDto);
                _sellerRepository.Update(seller);
                return _sellerRepository.Save();
            }
            return false;
        }
    }
}

