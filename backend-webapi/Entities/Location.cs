using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int DelivererId { get; set; }
        [ForeignKey("DelivererId")]
        public virtual Deliverer Deliverer { get; set; }
    }
}

