using AutoMapper;
using backend_webapi;
using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using webapi.Dtos;
using webapi.Entities;
using webapi.Repositories;

namespace webapi.Services
{
    public class DelivererService : IDelivererService
    {
        private ICommonRepository<Deliverer> _delivererRepository;
        private ICommonRepository<Location> _locationRepository;
        private ILocationService _locationService;

        public DelivererService(ICommonRepository<Deliverer> delivererRepository, ICommonRepository<Location> locationRepository, 
                                                    ILocationService locationService)
        {
            _delivererRepository = delivererRepository;
            _locationRepository = locationRepository;
            _locationService = locationService;
        }

        //GetDelivererNearByShop
        public IEnumerable<DeliveryDetails> GetDelivererNearByShop(double latitude, double longitude)
        {
            var source = new GeoCoordinate() { Latitude = latitude, Longitude = longitude }; //shop location

            var query = (
                        from deliverer in _delivererRepository.GetAll()
                        from location in _locationRepository.GetAll()
                        where deliverer.Id == location.DelivererId && deliverer.DeliveryStatus == "online" && new GeoCoordinate() { Latitude = location.Latitude, Longitude = location.Longitude }.GetDistanceTo(source)<100000
                        orderby new GeoCoordinate() { Latitude = location.Latitude, Longitude = location.Longitude }.GetDistanceTo(source) ascending
                        select new DeliveryDetails
                        {
                            delivererId=deliverer.Id,
                            shopLatitude=latitude,
                            shopLongitude=longitude
                        }
                        ).ToList();

            return query;
        }

        //availableDelivery
        public bool AvailableDelivery(bool availability)
        {

            return availability;
        }

        //updateDeliveryStatus
        public void UpdateDeliveryStatus(int id, string deliveryStatus)
        {
            Deliverer deliverer = _delivererRepository.Get(id);
            if (deliverer != null)
            {
                deliverer.DeliveryStatus = deliveryStatus;
                _delivererRepository.Update(deliverer);
                _delivererRepository.Save();
            }
        }

        //GetAllDeliverers
        public IEnumerable<DelivererDto> GetAllDeliverers()
        {
            var allDeliverers = _delivererRepository.GetAll().ToList();
            var allDeliverersDetails = allDeliverers.Select(x => Mapper.Map<DelivererDto>(x));
            return allDeliverersDetails;
        }

        //GetDelivererById
        public DelivererDto GetDelivererById(int id)
        {
            Deliverer delivererFromRepo = _delivererRepository.Get(id);
            return Mapper.Map<DelivererDto>(delivererFromRepo);
        }

        //CreateNewDeliverer
        public bool CreateNewDeliverer(DelivererDto delivererDto)
        {
            Deliverer toAdd = Mapper.Map<Deliverer>(delivererDto);

            _delivererRepository.Add(toAdd);

            bool result = _delivererRepository.Save();

            return result;
        }

        //DeleteDeliverer
        public bool DeleteDeliverer(int id)
        {
            var deliverer = _delivererRepository.Get(x => x.Id == id).First();

            if (deliverer != null)
            {
                _delivererRepository.Remove(deliverer);
                _delivererRepository.Save();
                return true;
            }
            return false;
        }

        //UpdateDeliverer
        public bool UpdateDeliverer(DelivererDto delivererDto)
        {
            var delivererinDb = _delivererRepository.Get(x => x.Id == delivererDto.Id).First();

            if (delivererinDb != null)
            {
                Deliverer deliverer = Mapper.Map<Deliverer>(delivererDto);
                _delivererRepository.Update(deliverer);
                return _delivererRepository.Save();
            }
            return false;
        }
    }
}
