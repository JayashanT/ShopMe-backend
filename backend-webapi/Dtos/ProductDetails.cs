using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Dtos
{
    public class ProductDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public int UnitPrice { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double Rating { get; set; }
        public int Like { get; set; }
        public double Discount { get; set; }
        public double SellingPrice { get; set; }
        public string Category { get; set; }
        public ShopDetails ShopDetails { get; set; }
    }
}
