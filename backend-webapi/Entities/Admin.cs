using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Entities
{
    [Table("Admins")]
    public class Admin : User
    {
        public string Qualifications { get; set; }
    }
}
