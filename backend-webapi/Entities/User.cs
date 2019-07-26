using backend_webapi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    public abstract class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public string MobileNumber { get; set; }
        public string Token { get; set; }
        public int LoginId { get; set; }
        [ForeignKey("LoginId")]
        public virtual Login login { get; set; }
    }
}
