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
    public class OpcinaService : IOpcina
    {
        private readonly MyContext _context;

        public OpcinaService(MyContext context)
        {
            _context = context;
        }

        public List<Opcina> GetOpcine()
        {
            return _context.Opcina.Include(o => o.Kanton).ToList();
        }

        public async Task KreirajOpcinu(Opcina opcina)
        {
            _context.Opcina.Add(opcina);
            await _context.SaveChangesAsync();
        }
    }
}
