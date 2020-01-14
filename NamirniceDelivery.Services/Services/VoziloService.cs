using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class VoziloService : IVozilo
    {
        private readonly MyContext _context;

        public VoziloService(MyContext context)
        {
            _context = context;
        }

        public List<Vozilo> GetVozila()
        {
            return _context.Vozilo.ToList();
        }
    }
}
