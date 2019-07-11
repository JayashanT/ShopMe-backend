using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    public class Product
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

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual Seller Seller { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual List<OrderItemProduct> OrderItemProducts { get; set; }
    }
}
