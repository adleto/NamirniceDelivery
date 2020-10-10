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
    public class VozacService : IVozac
    {
        private readonly MyContext _context;
        static UserManager<ApplicationUser> _userManager;


        public VozacService(UserManager<ApplicationUser> userManager, MyContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<VozaciZaDisplay> GetVozaciSimple()
        {
            return _context.Vozac.Select(
                v => new VozaciZaDisplay
                {
                    Id = v.Id,
                    ImeIPrezime = v.Ime + " " + v.Prezime
                }).ToList();
        }
        public void Deactivate(string vozacId)
        {
            var radnik = _context.Vozac.Find(vozacId);
            radnik.LockoutEnabled = true;
            radnik.LockoutEnd = new DateTimeOffset(DateTime.MaxValue);
            _context.SaveChanges();
        }
        public List<Vozac> Get()
        {
            return _context.Vozac
                .Include(a => a.Vozilo)
                .Where(a => a.LockoutEnd == null || a.LockoutEnd <= DateTime.Now)
                .ToList();
        }
        public Vozac GetVozac(string username)
        {
            return _context.Vozac
                .Include(a => a.OpcinaRodjenja)
                .Include(a => a.OpcinaBoravka)
                .Include(a => a.Vozilo)
                .Where(a => a.UserName == username)
                .FirstOrDefault();
        }
        public Vozac GetVozacById(string id)
        {
            return _context.Vozac
                .Include(a => a.OpcinaRodjenja)
                .Include(a => a.OpcinaBoravka)
                .Include(a => a.Vozilo)
                .Where(a => a.Id == id)
                .FirstOrDefault();
        }
        public async Task Vozac(VozacViewModel vozac)
        {
            if (!string.IsNullOrEmpty(vozac.Id))
            {
                var postojeciRadnik = _context.Vozac.Find(vozac.Id);
                postojeciRadnik.Ime = vozac.Ime;
                postojeciRadnik.Prezime = vozac.Prezime;
                postojeciRadnik.OpcinaBoravkaId = vozac.OpcinaIdBoravka;
                if (vozac.VoziloId != 0) postojeciRadnik.VoziloId = vozac.VoziloId;
                _context.SaveChanges();
                if (!string.IsNullOrEmpty(vozac.Password))
                {
                    await _userManager.RemovePasswordAsync(postojeciRadnik);
                    await _userManager.AddPasswordAsync(postojeciRadnik, vozac.Password);
                }
            }
            else
            {
                //create
                var newRadnik = new Vozac
                {
                    Ime = vozac.Ime,
                    Prezime = vozac.Prezime,
                    KategorijaVozackeDozvole = vozac.KategorijaVozackeDozvole,
                    OpcinaBoravkaId = vozac.OpcinaIdBoravka,
                    OpcinaRodjenjaId = vozac.OpcinaIdRodjenja,
                    UserName = vozac.Username,
                    Email = vozac.Username + "@namirnice.com"
                };
                if (vozac.VoziloId != 0) newRadnik.VoziloId = vozac.VoziloId;
                var result = await _userManager.CreateAsync(newRadnik, vozac.Password);
                var isOk = await _userManager.AddToRoleAsync(newRadnik, "Vozac");
            }
        }

    }
}
