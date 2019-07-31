using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.ViewModels
{
    public class SellerVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public string MobileNumber { get; set; }
        public string Token { get; set; }
        public string ShopName { get; set; }
        public string AccountNo { get; set; }
        public string ShopAddress { get; set; }
        public string Image { get; set; }
        public double ShopLocationLatitude { get; set; }
        public double ShopLocationLongitude { get; set; }
        public LoginVM LoginVM { get; set; }
    }
}
