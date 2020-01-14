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
    public class PodruznicaService : IPodruznica
    {
        private readonly MyContext _context;

        public PodruznicaService(MyContext context)
        {
            _context = context;
        }

        public Podruznica GetPodruznica(int podruznicaId)
        {
            return _context.Podruznica
                .Include(p=>p.Opcina)
                .Include(p=>p.NamirnicaPodruznica)
                .Where(p=>p.Id == podruznicaId)
                .FirstOrDefault();
        }

        public List<Podruznica> GetPodruznice()
        {
            return _context.Podruznica
                .Include(p=>p.Opcina)
                .Include(p=>p.NamirnicaPodruznica)
                    .ThenInclude(np=>np.Namirnica)
                        .ThenInclude(n=>n.Kategorija)
                .ToList();
        }

        public List<Podruznica> GetPodruzniceForKupac(Kupac kupac)
        {
            return GetPodruznice()
                .Where(p => p.OpcinaId == kupac.OpcinaBoravkaId)
                .ToList();
        }

        public void KreirajPodruznicu(Podruznica podruznica)
        {
            _context.Podruznica.Add(podruznica);
            _context.SaveChanges();
        }
    }
}
