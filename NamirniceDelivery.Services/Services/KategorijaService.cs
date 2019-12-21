using Microsoft.EntityFrameworkCore;
using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Services
{
    public class KategorijaService:IKategorija
    {
        private readonly MyContext _context;

        public KategorijaService(MyContext context)
        {
            _context = context;
        }
        public void KreirajKategoriju(Kategorija kategorija)
        {
            _context.Kategorija.Add(kategorija);
            _context.SaveChanges();
        }
        public List<Kategorija> GetKategorije()
        {
            return _context.Kategorija.ToList();
        }

        public Kategorija GetKategorija(int id)
        {
            return _context.Kategorija.Find(id);
        }

        public void EditKategorija(Kategorija kategorija)
        {
            var k = GetKategorija(kategorija.Id);
            k.Naziv = kategorija.Naziv;
            _context.SaveChanges();
        }
    }
}
