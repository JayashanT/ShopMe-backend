using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Entities;

namespace webapi.Dtos
{
    public class OrderDeliveryDetails
    {
        public Customer customer;
        public Seller seller;
    }
}
