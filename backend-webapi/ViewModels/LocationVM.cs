using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_webapi.ViewModels
{
    public class LocationVM
    {
        // int Id { get; set; }
        public int DelivererId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ConnectionId { get; set; }
    }
}
