using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Dtos
{
    public class OrderItemProductDto
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
    }
}
