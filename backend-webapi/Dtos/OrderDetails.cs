using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Dtos;

namespace webapi.Dtos
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OrderStatus { get; set; }
        public double TotalPrice { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
