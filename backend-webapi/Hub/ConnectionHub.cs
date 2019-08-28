using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Entities;
using webapi.Repositories;
using webapi.Services;
using Microsoft.AspNetCore.SignalR;
using webapi.Dtos;

namespace webapi.Hub
{
    public class ConnectionHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private IDelivererService _delivererService;
        private ILocationService _LocationService;
        private ICommonRepository<Location> _locationRepository;
        private IOrderService _orderService;
        private ISellerService _sellerService;

        public ConnectionHub(IDelivererService delivererService, ICommonRepository<Location> locationRepository,
                                ILocationService LocationService, IOrderService orderService, ISellerService sellerService)
        {
            _delivererService = delivererService;
            _locationRepository = locationRepository;
            _LocationService = LocationService;
            _orderService = orderService;
            _sellerService = sellerService;
        }

        public async Task GoOnline(LocationDto locationDto)
        {
            string connectionId = Context.ConnectionId;
            locationDto.ConnectionId = connectionId;
            var location = _LocationService.UpdateDeliveryLocation(locationDto);
            _delivererService.UpdateDeliveryStatus(locationDto.DelivererId, "online");

            await Clients.Client(connectionId).SendAsync("ReceiveMessage", "you are online");
        }

        public async Task SellerOnline(int id)
        {
            string connectionId = Context.ConnectionId;
            _sellerService.UpdateSellerConnectionId(id, connectionId);
            await Clients.Client(connectionId).SendAsync("SellerOnline", "updated connectionId");
        }

        public async Task SendRequest(double shopLatitude, double shopLongitude,int orderId)
        {
            var availableDelivery = _delivererService.GetDelivererNearByShop(shopLatitude, shopLongitude);
            await Clients.Client(availableDelivery.ConnectionId).SendAsync("SendRequest", orderId);
        }

        public async Task Reply(int sellerId, string message)
        {
            var seller = _sellerService.GetSellerById(sellerId);
            await Clients.Clients(seller.ConnectionId).SendAsync("Reply", message);
        }
    }
}
