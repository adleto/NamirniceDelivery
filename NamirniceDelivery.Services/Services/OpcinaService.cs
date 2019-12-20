using Microsoft.EntityFrameworkCore;
using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Services
{
    public class OpcinaService : IOpcina
    {
        private readonly MyContext _context;

        public OpcinaService(MyContext context)
        {
            _context = context;
        }

        public async Task<List<Opcina>> GetOpcine()
        {
            return await _context.Opcina.Include(o => o.Kanton).ToListAsync();
        }

        public async Task KreirajOpcinu(Opcina opcina)
        {
            _context.Opcina.Add(opcina);
            await _context.SaveChangesAsync();
        }
    }
}
