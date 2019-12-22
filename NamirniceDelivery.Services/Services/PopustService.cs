using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class PopustService : IPopust
    {
        private readonly MyContext _context;

        public PopustService(MyContext context)
        {
            _context = context;
        }

        public List<Popust> GetPopusti()
        {
            return _context.Popust.ToList();
        }

        public void KreirajPopust(Popust popust)
        {
            _context.Popust.Add(popust);
            _context.SaveChanges();
        }
    }
}
