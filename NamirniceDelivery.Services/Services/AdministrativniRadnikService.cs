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
    public class AdministrativniRadnikService : IAdministrativniRadnik
    {
        private readonly MyContext _context;

        public AdministrativniRadnikService(MyContext context)
        {
            _context = context;
        }

        public int GetPodruznicaIdOdRadnika(string username)
        {
            return _context.AdministrativniRadnik
                .Include(a => a.Podruznica)
                .Where(a => a.UserName == username)
                .FirstOrDefault()
                .PodruznicaId??1;
        }

        public AdministrativniRadnik GetRadnik(string username)
        {
            return _context.AdministrativniRadnik
                .Include(a => a.OpcinaRodjenja)
                .Include(a => a.OpcinaBoravka)
                .Include(a => a.Podruznica)
                .Include(a => a.Transakcije)
                .Where(a => a.UserName == username)
                .FirstOrDefault();
        }
    }
}
