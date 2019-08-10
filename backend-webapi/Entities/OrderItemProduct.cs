using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    public class OrderItemProduct
    {
        [Key]
        public int Id { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int OrderItemId { get; set; }
        [ForeignKey("OrderItemId")]
        public virtual OrderItem OrderItem { get; set; }
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
