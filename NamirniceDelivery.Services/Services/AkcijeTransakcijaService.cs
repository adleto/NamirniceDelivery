using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class AkcijeTransakcijaService : IAkcijeTransakcija
    {
        private readonly MyContext _context;

        public AkcijeTransakcijaService(MyContext context)
        {
            _context = context;
        }

        public void OdobriTranskaciju(int transakcijaId, AdministrativniRadnik radnik)
        {
            var transakcija = GetTransakcija(transakcijaId);
            if (transakcija.NarudzbaPrihvacenaOdRadnika == false &&
                transakcija.PodruznicaId == radnik.PodruznicaId)
            {
                transakcija.NarudzbaPrihvacenaOdRadnika = true;
                transakcija.DatumPrihvacanjaNarudzbe = DateTime.Now;
                _context.SaveChanges();
            }
        }
        private Transakcija GetTransakcija(int transakcijaId)
        {
            return _context.Transakcija.Find(transakcijaId);
        }
    }
}
