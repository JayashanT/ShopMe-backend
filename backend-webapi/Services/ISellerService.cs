using System;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using webapi.Dtos;

namespace webapi.Services
{
    public interface ISellerService
    {
        bool CreateNewSeller(SellerDto sellerDto);
        bool DeleteSeller(int id);
        IEnumerable<SellerDto> GetAllSellers();
        SellerDto GetSellerById(int id);
        IEnumerable<SellerDto> GetSellersByLocation(int locationId);
        IEnumerable<ShopDetails> GetShopsNearByLocation(double latitude, double longitude);
        bool UpdateSellerConnectionId(int id, string connectionId);
        bool UpdateSeller(SellerDto sellerDto);
    }
}