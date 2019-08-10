using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Dtos
{
    public class LocationDto
    {
        public int Id { get; set; }
        public int DelivererId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ConnectionId { get; set; }
    }
}
