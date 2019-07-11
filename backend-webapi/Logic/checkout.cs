using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Services;
using webapi.ViewModels;
using webapi.Controllers;

namespace  webapi.Logic
{
    public class Checkout
    {
        private IDelivererService _delivererService;
        private IOrderService _orderService;
        private DeliverersController deliverersController;
        public int availableId = 0;

        public Checkout(IDelivererService delivererService, IOrderService orderService)
        {
            _delivererService = delivererService;
            _orderService = orderService;
        }

        //checkout
        public void checkout(OrderVM orderVM, double latitude, double longitude)
        {
            var deliverers = _delivererService.GetDelivererNearByShop(latitude, longitude);
            foreach (var deliverer in deliverers)
            {
            }
        }
    }
}
