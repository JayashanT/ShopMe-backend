using System.Collections.Generic;
using webapi.Dtos;

namespace webapi.Services
{
    public interface ILocationService
    {
        IEnumerable<LocationDto> GetAllLocations();
        bool UpdateDeliveryLocation(LocationDto deliveryLocation);
    }
}