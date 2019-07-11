using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.ViewModels
{
    public class DelivererVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string ProfileImage { get; set; }
        public string MobileNumber { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
        public string DeliveryStatus { get; set; }
    }
}
