using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime PaymentDate { get; set; }
        public int OrderId { get; set; }
    }
}
