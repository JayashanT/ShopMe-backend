using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Dtos
{
    public class ShopDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public string MobileNumber { get; set; }
        public string shopName { get; set; }
        public string AccountNo { get; set; }
        public string ShopAddress { get; set; }
        public double ShopLocationLatitude { get; set; }
        public double ShopLocationLongitude { get; set; }
        public double Distance { get; set; }
    }
}
