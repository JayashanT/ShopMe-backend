using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Entities;

namespace webapi.ViewModels
{
    public class OrderVM
    {
        public int CustomerId { get; set; }
        public int SellerId { get; set; }
        public string Status { get; set; }
        public double CustomerLatitude { get; set; }
        public double CustomerLongitude { get; set; }
        public IEnumerable<ItemVM> Items { get; set; }
    }
}
