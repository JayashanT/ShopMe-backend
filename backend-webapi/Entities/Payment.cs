using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public int Status { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
