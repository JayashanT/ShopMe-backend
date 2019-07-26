using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Entities;

namespace backend_webapi.Entities
{
    public class Login
    {
        public int Id { get; set; }
        public string Role  { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual  Customer Customer { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual Deliverer Deliverer { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
