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

        public void EditNamirnica(Namirnica namirnica)
        {
            var n = GetNamirnica(namirnica.Id);
            n.KategorijaId = namirnica.KategorijaId;
            n.Naziv = namirnica.Naziv;
            _context.SaveChanges();
        }

        public List<bool> GetIsDeletable(List<Namirnica> namirnicaList)
        {
            List<bool> list = new List<bool>();
            foreach (var item in namirnicaList)
            {
                if (_context.NamirnicaPodruznica.Where(np => np.Namirnica == item).Any())
                {
                    list.Add(false);
                }
                else
                {
                    list.Add(true);
                }
            }
            return list;
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

        public void UkloniNamirnica(int namirnicaId)
        {
            _context.Namirnica.Remove(GetNamirnica(namirnicaId));
            _context.SaveChanges();
        }
    }
}
