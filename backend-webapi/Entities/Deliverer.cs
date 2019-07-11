using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    [Table("Deliverers")]
    public class Deliverer : User
    {
        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
        public string DeliveryStatus { get; set; }
        public string NIC { get; set; }
        public virtual Location Location { get; set; }
    }
}
