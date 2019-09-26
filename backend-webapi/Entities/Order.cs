using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int CustomerId { get; set; }
        public int SellerId { get; set; }
        public int DelivererId { get; set; }
        public string Status { get; set; }
        public double CustomerLatitude { get; set; }
        public double CustomerLongitude { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("SellerId")]
        public virtual Seller Seller { get; set; }
        [ForeignKey("DelivererId")]
        public virtual Deliverer Deliverer { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}