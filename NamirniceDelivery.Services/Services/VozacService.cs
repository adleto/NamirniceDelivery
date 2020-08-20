using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class VozacService : IVozac
    {
        private readonly MyContext _context;

        public VozacService(MyContext context)
        {
            _context = context;
        }

        public List<VozaciZaDisplay> GetVozaciSimple()
        {
            return _context.Vozac.Select(
                v => new VozaciZaDisplay
                {
                    Id = v.Id,
                    ImeIPrezime = v.Ime + " " + v.Prezime
                }).ToList();
        }
    }
}
