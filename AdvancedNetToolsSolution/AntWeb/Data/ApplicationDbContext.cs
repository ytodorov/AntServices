using SmartAdminMvc.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Data
{
    public class ApplicationDbContext : IdentityDbContect<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}