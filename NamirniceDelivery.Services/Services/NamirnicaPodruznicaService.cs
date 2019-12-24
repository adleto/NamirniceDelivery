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
    public class NamirnicaPodruznicaService:INamirnicaPodruznica
    {
        private readonly MyContext _context;

        public NamirnicaPodruznicaService(MyContext context)
        {
            _context = context;
        }

        public void DodajNamirnicu(NamirnicaPodruznica namirnicaPodruznica)
        {
            _context.NamirnicaPodruznica.Add(namirnicaPodruznica);
            _context.SaveChanges();
        }

        public void EditNamirnicaPodruznica(NamirnicaPodruznica namirnicaPodruznica)
        {
            var namirnica = GetNamirnicaPodruznica(namirnicaPodruznica.Id);
            namirnica.Aktivna = namirnicaPodruznica.Aktivna;
            namirnica.Cijena = namirnicaPodruznica.Cijena;
            namirnica.PopustId = namirnicaPodruznica.PopustId;
            namirnica.KolicinaNaStanju = namirnicaPodruznica.KolicinaNaStanju;
            _context.SaveChanges();
        }

        public NamirnicaPodruznica GetNamirnicaPodruznica(int id)
        {
            return _context.NamirnicaPodruznica
                .Include(np => np.Podruznica)
                .Include(np => np.Popust)
                .Include(np => np.Namirnica)
                    .ThenInclude(n => n.Kategorija)
                .Where(np => np.Id == id)
                .FirstOrDefault();
        }

        public List<NamirnicaPodruznica> GetNamirniceForKupac(Kupac kupac)
        {
            return GetNamirnicePodruznica()
                .Where(np => np.Podruznica.OpcinaId == kupac.OpcinaBoravkaId)
                .ToList();
        }

        public List<NamirnicaPodruznica> GetNamirnicePodruznica()
        {
            return _context.NamirnicaPodruznica
                .Include(np => np.Podruznica)
                .Include(np => np.Popust)
                .Include(np => np.Namirnica)
                    .ThenInclude(n => n.Kategorija)
                .ToList();
        }
        public List<NamirnicaPodruznica> GetNamirnicePodruznica(int podruznicaId)
        {
            return GetNamirnicePodruznica()
                .Where(np => np.PodruznicaId == podruznicaId)
                .ToList();
        }

        public List<NamirnicaPodruznica> GetNamirnicePodruznicaKategorija(Kategorija kategorija)
        {
            return GetNamirnicePodruznica()
                .Where(np => np.Namirnica.Kategorija == kategorija)
                .ToList();
        }

        public List<NamirnicaPodruznica> GetNamirnicePodruznicaKategorija(Kategorija kategorija, int podruznicaId)
        {
            return GetNamirnicePodruznicaKategorija(kategorija)
                .Where(np => np.PodruznicaId == podruznicaId)
                .ToList();
        }

        public List<NamirnicaPodruznica> GetNamirnicePodruznicaVrsta(Namirnica namirnica)
        {
            return GetNamirnicePodruznica()
                .Where(np => np.Namirnica == namirnica)
                .ToList();
        }

        public List<NamirnicaPodruznica> GetNamirnicePodruznicaVrsta(Namirnica namirnica, int podruznicaId)
        {
            return GetNamirnicePodruznicaVrsta(namirnica)
                .Where(np => np.PodruznicaId == podruznicaId)
                .ToList();
        }

        public void ToogleStatusNamirnicaPodruznica(int id)
        {
            var np = GetNamirnicaPodruznica(id);
            if (np.Aktivna) np.Aktivna = false;
            else
            {
                np.Aktivna = true;
            }
            _context.SaveChanges();
        }

        public void UkloniNamirnicaPodruznica(int id)
        {
            _context.NamirnicaPodruznica.Remove(GetNamirnicaPodruznica(id));
            _context.SaveChanges();
        }
    }
}
