using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public double CustomerLatitude { get; set; }
        public double CustomerLongitude { get; set; }
        public int SellerId { get; set; }
        public int CustomerId { get; set; }
        public int  DelivererId { get; set; }
    }
}
