using System;
using System.Collections.Generic;
using webapi.Dtos;

namespace webapi.Services
{
    public interface IDelivererService
    {
        bool CreateNewDeliverer(DelivererDto delivererDto);
        bool DeleteDeliverer(int id);
        IEnumerable<DelivererDto> GetAllDeliverers();
        DelivererDto GetDelivererById(int id);
        IEnumerable<DeliveryDetails> GetDelivererNearByShop(double latitude, double longitude);  
        bool UpdateDeliverer(DelivererDto delivererDto);
        void UpdateDeliveryStatus(int id, string deliveryStatus);
        bool AvailableDelivery(bool availability);
    }
}