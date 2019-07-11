using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Dtos;
using webapi.Entities;
using webapi.Repositories;

namespace webapi.Services
{
    public class LocationService : ILocationService
    {
        private ICommonRepository<Location> _locationRepository;

        public LocationService( ICommonRepository<Location> locationRepository)
        {
            _locationRepository = locationRepository;
        }


        //GetAllLocations
        public IEnumerable<LocationDto> GetAllLocations()
        {
            var allLocations = _locationRepository.GetAll().ToList();
            var allLocationsDetails = allLocations.Select(x => Mapper.Map<LocationDto>(x));
            return allLocationsDetails;
        }

        public bool UpdateDeliveryLocation(LocationDto deliveryLocation)
        {
            var locationDb = _locationRepository.Get(x=> x.Id==deliveryLocation.Id).FirstOrDefault();
            if (locationDb != null)
            {
                Location location = Mapper.Map<Location>(deliveryLocation);
                _locationRepository.Update(location);
                return _locationRepository.Save();
            }
            return false;
        }
        
    }
}
