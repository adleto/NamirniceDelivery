﻿using Microsoft.EntityFrameworkCore;
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
    }
}
