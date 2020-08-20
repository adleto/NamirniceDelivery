using Microsoft.EntityFrameworkCore;
using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class VoziloService : IVozilo
    {
        private readonly MyContext _context;

        public VoziloService(MyContext context)
        {
            _context = context;
        }

        public void Add_EditVozilo(int? id, VoziloModel model)
        {
            if (id == null)
            {
                var addModel = new Vozilo
                {
                    MarkaVozila = model.MarkaVozila,
                    RegistarskeOznake = model.RegistarskeOznake,
                    TipVozila = model.TipVozila
                };
                if (!string.IsNullOrEmpty(model.VozacId)) addModel.VozacId = model.VozacId;
                _context.Vozilo.Add(addModel);
            }
            else
            {
                var vozilo = _context.Vozilo.Find(model.Id);
                vozilo.MarkaVozila = model.MarkaVozila;
                vozilo.RegistarskeOznake = model.RegistarskeOznake;
                vozilo.TipVozila = model.TipVozila;
                if (!string.IsNullOrEmpty(model.VozacId)) vozilo.VozacId = model.VozacId;
            }
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var vozilo = _context.Vozilo.Find(id);
            _context.Vozilo.Remove(vozilo);
            _context.SaveChanges();
        }

        public List<Vozilo> GetVozila()
        {
            return _context.Vozilo.Include(v => v.Vozac).ToList();
        }

        public Vozilo GetVozilo(int id)
        {
            var vozilo = _context.Vozilo.Find(id);
            return vozilo;
        }
    }
}
