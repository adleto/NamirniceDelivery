using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Services
{
    public class TipTransakcijeService : ITipTransakcije
    {
        private readonly MyContext _context;

        public TipTransakcijeService(MyContext context)
        {
            _context = context;
        }

        public async Task KreirajTipTransakcije(TipTransakcije tipTransakcije)
        {
            _context.TipTransakcije.Add(tipTransakcije);
            await _context.SaveChangesAsync();
        }
    }
}
