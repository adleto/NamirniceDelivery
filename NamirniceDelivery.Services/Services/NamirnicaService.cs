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
    public class NamirnicaService : INamirnica
    {
        private readonly MyContext _context;

        public NamirnicaService(MyContext context)
        {
            _context = context;
        }

        public Namirnica GetNamirnica(int id)
        {
            return _context.Namirnica
                .Include(n => n.Kategorija)
                .Where(n => n.Id == id)
                .FirstOrDefault();
        }

        public List<Namirnica> GetNamirnice()
        {
            return _context.Namirnica
                .Include(n => n.Kategorija)
                .ToList();
        }

        public List<Namirnica> GetNamirnicePoKategorijama(Kategorija kategorija)
        {
            return _context.Namirnica
                .Include(n => n.Kategorija)
                .Where(n=>n.Kategorija == kategorija)
                .ToList();
        }

        public void KreirajNamirnica(Namirnica namirnica)
        {
            _context.Namirnica.Add(namirnica);
            _context.SaveChanges();
        }
    }
}
