using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Services
{
    public class AdministrativniRadnikService : IAdministrativniRadnik
    {
        private readonly MyContext _context;
        static UserManager<ApplicationUser> _userManager;

        public AdministrativniRadnikService(UserManager<ApplicationUser> userManager, MyContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Deactivate(string radnikId)
        {
            var radnik = _context.AdministrativniRadnik.Find(radnikId);
            radnik.LockoutEnabled = true;
            radnik.LockoutEnd = new DateTimeOffset(DateTime.MaxValue);
            _context.SaveChanges();
        }

        public List<AdministrativniRadnik> Get()
        {
            return _context.AdministrativniRadnik
                .Include(a => a.Podruznica)
                .Include(a => a.Transakcije)
                .Where(a => a.LockoutEnd == null || a.LockoutEnd<=DateTime.Now)
                .ToList();
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
        public AdministrativniRadnik GetRadnikById(string id)
        {
            return _context.AdministrativniRadnik
                .Include(a => a.OpcinaRodjenja)
                .Include(a => a.OpcinaBoravka)
                .Include(a => a.Podruznica)
                .Include(a => a.Transakcije)
                .Where(a => a.Id == id)
                .FirstOrDefault();
        }

        public async Task Radnik(AdminRadnikViewModel radnik)
        {
            if (!string.IsNullOrEmpty(radnik.Id))
            {
                var postojeciRadnik = _context.AdministrativniRadnik.Find(radnik.Id);
                postojeciRadnik.Ime = radnik.Ime;
                postojeciRadnik.Prezime = radnik.Prezime;
                postojeciRadnik.OpcinaBoravkaId = radnik.OpcinaIdBoravka;
                _context.SaveChanges();
                if (!string.IsNullOrEmpty(radnik.Password)){ 
                await _userManager.RemovePasswordAsync(postojeciRadnik);
                await _userManager.AddPasswordAsync(postojeciRadnik, radnik.Password); }
            }
            else
            {
                //create
                var newRadnik = new AdministrativniRadnik
                {
                    Ime = radnik.Ime,
                    Prezime = radnik.Prezime,
                    JMBG = radnik.JMBG,
                    OpcinaBoravkaId = radnik.OpcinaIdBoravka,
                    OpcinaRodjenjaId = radnik.OpcinaIdRodjenja,
                    PodruznicaId = radnik.PodruznicaId,
                    UserName = radnik.Username,
                    RejtingRadnik = 0
                    , Email = radnik.Username + "@namirnice.com"
                };
                var result = await _userManager.CreateAsync(newRadnik, radnik.Password);
            }
        }
    }
}
