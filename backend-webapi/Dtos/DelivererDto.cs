using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Dtos
{
    public class DelivererDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public string MobileNumber { get; set; }
        public string Token { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
        public string DeliveryStatus { get; set; }
        public string NIC { get; set; }
        public int LocationId { get; set; }
        public int LoginId { get; set; }
    }
}
