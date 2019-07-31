using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    [Table("Sellers")]
    public class Seller : User
    {
        public string ShopName { get; set; }
        public string AccountNo { get; set; }
        public string ShopAddress { get; set; }
        public string Image { get; set; }
        public double ShopLocationLatitude { get; set; }
        public double ShopLocationLongitude { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
