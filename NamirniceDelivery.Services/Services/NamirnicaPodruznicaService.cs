using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class NamirnicaPodruznicaService:INamirnicaPodruznica
    {
        private readonly MyContext _context;

        public NamirnicaPodruznicaService(MyContext context)
        {
            _context = context;
        }

        public void DodajNamirnicu(NamirnicaPodruznica namirnicaPodruznica)
        {
            _context.NamirnicaPodruznica.Add(namirnicaPodruznica);
            _context.SaveChanges();
        }
    }
}
