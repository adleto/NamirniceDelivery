using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Services
{
    public class PodruznicaService : IPodruznica
    {
        private readonly MyContext _context;

        public PodruznicaService(MyContext context)
        {
            _context = context;
        }

        public void KreirajPodruznicu(Podruznica podruznica)
        {
            _context.Podruznica.Add(podruznica);
            _context.SaveChanges();
        }
    }
}
