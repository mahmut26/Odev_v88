using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model_Login
{
    public class AppUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
        public bool IsYazar { get; set; }
        public bool IsKullanici { get; set; }

    }
}
