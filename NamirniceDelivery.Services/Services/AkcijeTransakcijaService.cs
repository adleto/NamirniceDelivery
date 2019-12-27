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

        public void DostavaUspjela(int transakcijaId, Kupac kupac)
        {
            var transakcija = GetTransakcija(transakcijaId);
            if(transakcija.NarudzbaPrihvacenaOdRadnika == true &&
                transakcija.DostavaUspjesna == false &&
                transakcija.KupacId == kupac.Id)
            {
                transakcija.DostavaUspjesna = true;
                transakcija.DatumUspjesneDostave = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public void KupacOstaviNegativan(int transakcijaId)
        {
            var t = GetTransakcija(transakcijaId);
            t.KupacOstavioDojam = true;
            t.DojamKupac = "Negativan";
            _context.SaveChanges();
        }

        public void KupacOstaviPozitivan(int transakcijaId)
        {
            var t = GetTransakcija(transakcijaId);
            t.KupacOstavioDojam = true;
            t.DojamKupac = "Pozitivan";
            _context.SaveChanges();
        }

        public void OdobriTranskaciju(int transakcijaId, AdministrativniRadnik radnik)
        {
            var transakcija = GetTransakcija(transakcijaId);
            if (transakcija.NarudzbaPrihvacenaOdRadnika == false &&
                transakcija.DostavaUspjesna == false &&
                transakcija.PodruznicaId == radnik.PodruznicaId)
            {
                transakcija.AdministrativniRadnik = radnik;
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
