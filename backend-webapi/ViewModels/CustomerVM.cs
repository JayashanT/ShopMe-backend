﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.ViewModels
{
    public class CustomerVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string MobileNumber { get; set; }
        public string Token { get; set; }
    }
}
