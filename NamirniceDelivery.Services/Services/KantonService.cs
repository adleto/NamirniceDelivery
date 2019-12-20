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
    public class KantonService : IKanton
    {
        private readonly MyContext _context;

        public KantonService(MyContext context)
        {
            _context = context;
        }

        public List<Kanton> GetKantoni()
        {
            return _context.Kanton.ToList();
        }

        public async Task KreirajKanton(Kanton kanton)
        {
            if (!KantonPostoji(kanton))
            {
                _context.Kanton.Add(kanton);
                await _context.SaveChangesAsync();
            }
        }

        private bool KantonPostoji(Kanton kanton)
        {
            var kantoni = GetKantoni();
            foreach(var k in kantoni)
            {
                if(k.Naziv == kanton.Naziv)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
