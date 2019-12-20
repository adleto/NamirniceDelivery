using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
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
        public async Task KreirajKategoriju(Kategorija kategorija)
        {
            _context.Kategorija.Add(kategorija);
            await _context.SaveChangesAsync();
        }
    }
}
