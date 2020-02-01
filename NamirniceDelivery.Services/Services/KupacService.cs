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
    public class KupacService : IKupac
    {
        private readonly MyContext _context;

        public KupacService(MyContext context)
        {
            _context = context;
        }

        public void DodajSpremljenuNamiricu(string id, int namirnicaPodruznicaId)
        {
            _context.KupacSpremljeneNamirnice.Add(new KupacSpremljeneNamirnice
            {
                KupacId = id,
                NamirnicaPodruznicaId = namirnicaPodruznicaId
            });
            _context.SaveChanges();
        }

        public void DodajSpremljenuPodruznicu(string id, int podruznicaId)
        {
            _context.KupacSpremljenePodruznice.Add(new KupacSpremljenePodruznice
            {
                KupacId = id,
                PodruznicaId = podruznicaId
            });
            _context.SaveChanges();
        }

        public Kupac GetKupac(string username)
        {
            return _context.Kupac
                .Include(a => a.OpcinaRodjenja)
                .Include(a => a.OpcinaBoravka)
                .Include(a => a.Transakcije)
                .Include(a => a.SpreljeneNamirnice)
                    .ThenInclude(sn => sn.NamirnicaPodruznica)
                .Include(a => a.SpremljenePodruznice)
                    .ThenInclude(sp => sp.Podruznica)
                .Where(a => a.UserName == username)
                .FirstOrDefault();
        }

        public List<Kupac> GetKupci()
        {
            return _context.Kupac.ToList();
        }

        public List<Kupac> GetKupciZaSMSObavijest()
        {
            var rightNowMinusFiveDays = DateTime.Now.AddDays(-5);
            return _context.Kupac
                .Where(k=> (k.LastLoginTimestamp == null || rightNowMinusFiveDays > k.LastLoginTimestamp) &&
                    (k.ZadnjaSMSObavijest == null || rightNowMinusFiveDays > k.ZadnjaSMSObavijest))
                .Take(10)
                .ToList();
        }

        public void SMSObavjestPoslana(Kupac kupac)
        {
            GetKupac(kupac.UserName).ZadnjaSMSObavijest = DateTime.Now;
            _context.SaveChanges();
        }

        public List<KupacSpremljeneNamirnice> GetSpremljeneNamirnice(string id)
        {
            return _context.KupacSpremljeneNamirnice
                .Include(sn => sn.NamirnicaPodruznica)
                .Where(sn => sn.KupacId == id && sn.NamirnicaPodruznica.Aktivna == true)
                .ToList();
        }

        public List<KupacSpremljenePodruznice> GetSpremljenePodruznice(string id)
        {
            return _context.KupacSpremljenePodruznice
                .Where(sp => sp.KupacId == id)
                .ToList();
        }

        public void UkloniSpremljenuNamiricu(string id, int namirnicaPodruznicaId)
        {
            _context.KupacSpremljeneNamirnice.Remove(
                _context.KupacSpremljeneNamirnice
                    .Where(ksn => ksn.KupacId == id && ksn.NamirnicaPodruznicaId == namirnicaPodruznicaId)
                    .FirstOrDefault()
            );
            _context.SaveChanges();
        }

        public void UkloniSpremljenuPodruznicu(string id, int podruznicaId)
        {
            _context.KupacSpremljenePodruznice.Remove(
                _context.KupacSpremljenePodruznice
                    .Where(ksp => ksp.KupacId == id && ksp.PodruznicaId == podruznicaId)
                    .FirstOrDefault()
            );
            _context.SaveChanges();
        }
    }
}
