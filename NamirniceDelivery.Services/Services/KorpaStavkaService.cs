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
    public class KorpaStavkaService : IKorpaStavka
    {
        private readonly MyContext _context;

        public KorpaStavkaService(MyContext context)
        {
            _context = context;
        }

        public void DodajUKorpu(int namirnicaPodruznicaId, int brojNamirnica, Kupac kupac)
        {
            var namirnicaPodruznica = _context.NamirnicaPodruznica.Find(namirnicaPodruznicaId);
            if(namirnicaPodruznica.Aktivna == false || namirnicaPodruznica.KolicinaNaStanju < brojNamirnica)
            {
                return;
            }
            var stavka = GetNamirniceUKorpi(kupac)
                .Where(k => k.NamirnicaPodruznicaId == namirnicaPodruznicaId)
                .ToList();
            if (stavka.Any())
            {
                if(brojNamirnica + stavka[0].Kolicina <= namirnicaPodruznica.KolicinaNaStanju)
                {
                    stavka[0].Kolicina += brojNamirnica;
                    _context.SaveChanges();
                }
                return;
            }
            DodajStavku(new KorpaStavka { 
                Kolicina = brojNamirnica,
                Kupac = kupac,
                NamirnicaPodruznica = namirnicaPodruznica
            });
        }

        public KorpaStavka GetKorpaStavka(int id)
        {
            return _context.KorpaStavka
                .Include(ks => ks.Kupac)
                .Include(ks => ks.NamirnicaPodruznica)
                    .ThenInclude(np=>np.Namirnica)
                .Include(ks => ks.NamirnicaPodruznica)
                    .ThenInclude(np => np.Popust)
                .Include(ks => ks.NamirnicaPodruznica)
                    .ThenInclude(np => np.Podruznica)
                .Where(ks => ks.Id == id)
                .FirstOrDefault();
        }

        public List<KorpaStavka> GetNamirniceUKorpi(Kupac kupac)
        {
            return _context.KorpaStavka
                .Include(ks => ks.Kupac)
                .Include(ks => ks.NamirnicaPodruznica)
                    .ThenInclude(np => np.Namirnica)
                .Include(ks => ks.NamirnicaPodruznica)
                    .ThenInclude(np => np.Popust)
                .Include(ks => ks.NamirnicaPodruznica)
                    .ThenInclude(np => np.Podruznica)
                .Where(ks => ks.KupacId == kupac.Id)
                .ToList();
        }

        public decimal GetTotalCijena(Kupac kupac)
        {
            return GetNamirniceUKorpi(kupac).
                Sum(k => k.CijenaStavkeTotal);
        }

        public void UkloniStavku(int id, Kupac kupac)
        {
            var stavka = GetKorpaStavka(id);
            if(stavka.KupacId == kupac.Id)
            {
                _context.KorpaStavka.Remove(stavka);
                _context.SaveChanges();
            }
        }
        private void DodajStavku(KorpaStavka stavka)
        {
            _context.KorpaStavka.Add(stavka);
            _context.SaveChanges();
        }
    }
}
