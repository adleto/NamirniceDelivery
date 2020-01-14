using Microsoft.EntityFrameworkCore;
using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly MyContext _context;

        public ApplicationUserService(MyContext context)
        {
            _context = context;
        }

        public ApplicationUser GetUser(string username)
        {
            return _context.ApplicationUser
               .Include(a => a.OpcinaRodjenja)
               .Include(a => a.OpcinaBoravka)
               .Where(a => a.UserName == username)
               .FirstOrDefault();
        }
    }
}
