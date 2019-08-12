using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Dtos;

namespace backend_webapi.Dtos
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
