using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Dtos
{
    public class DeliveryDetails
    {
        public int delivererId { get; set; }
        public double shopLatitude { get; set; }
        public double shopLongitude { get; set; }
        //public string shopName { get; set; }

    }
}
