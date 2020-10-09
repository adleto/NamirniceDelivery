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
    public class VoznjaService:IVoznja
    {
        private readonly MyContext _context;

        public VoznjaService(MyContext context)
        {
            _context = context;
        }

        public void DeleteVoznja(int voznjaId)
        {
            try
            {
                _context.Voznja.Remove(_context.Voznja.Find(voznjaId));
                _context.SaveChanges();
            }
            catch
            {
                //not found
            }
        }

        public Voznja GetVoznja(int voznjaId)
        {
            try 
            {
                return _context.Voznja
                    .Include(v => v.PodruznicaPocetak)
                    .Include(v => v.PodruznicaKraj)
                    .Include(v => v.Vozac)
                    .Where(v => v.Id == voznjaId)
                    .FirstOrDefault();
            }
            catch
            {
                throw new ArgumentException("ne postoji voznja sa ovim id");
            }
        }

        public List<Voznja> GetVoznje()
        {
            return _context.Voznja
                .Include(v=>v.PodruznicaPocetak)
                .Include(v => v.PodruznicaKraj)
                .Include(v=>v.Vozac)
                .OrderByDescending(v => v.Id)
                .ToList();
        }
        public List<Voznja> GetVoznjeForVozacNeObavljenje(string vozacUsername)
        {
            return _context.Voznja
                .Include(v => v.PodruznicaPocetak)
                .Include(v => v.PodruznicaKraj)
                .Include(v => v.Vozac)
                .Where(v => v.Vozac.UserName == vozacUsername && v.ObavljenaVoznja == false)
                .OrderByDescending(v => v.Id)
                .ToList();
        }

        public void OznaciKaoZavrsenu(string name, int voznjaId)
        {
            try
            {
                var voznja = _context.Voznja
                    .Include(v => v.Vozac)
                    .Where(v => v.Id == voznjaId)
                    .First();
                if (voznja.Vozac.UserName != name) throw new Exception("Vozač nije na ovoj vožnji.");
                if (voznja.PreuzetaRoba == false) throw new Exception("Roba nije preuzeta, tako da vožnja ne može biti završena.");
                if (voznja.ObavljenaVoznja) throw new Exception("Vožnja je već završena.");
                voznja.ObavljenaVoznja = true;
                voznja.VoznjaKraj = DateTime.Now;
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                //nothing if goes wrong, skip
            }
        }

        public void PreuzmiRobu(string name, int voznjaId)
        {
            try
            {
                var voznja = _context.Voznja
                    .Include(v => v.Vozac)
                    .Where(v => v.Id == voznjaId)
                    .First();
                if (voznja.Vozac.UserName != name) throw new Exception("Vozač nije na ovoj vožnji.");
                if (voznja.ObavljenaVoznja) throw new Exception("Vožnja je već završena.");
                if (voznja.PreuzetaRoba) throw new Exception("Roba je već preuzeta.");

                voznja.VoznjaPocetak = DateTime.Now;
                voznja.PreuzetaRoba = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //nothing if goes wrong, skip
            }
        }

        public void Voznja(VoznjaViewModel model)
        {
            try
            {
                if(model.VoznjaId != null && model.VoznjaId != 0)
                {
                    //edit here
                    var voznja = _context.Voznja.Find(model.VoznjaId);
                    voznja.VozacId = model.VozacId;
                    voznja.PodruznicaKrajId = model.PodruznicaKrajId;
                    voznja.PodruznicaPocetakId = model.PodruznicaPocetakId;
                }
                else
                {
                    //create here
                    _context.Voznja.Add(new Data.Entities.Voznja
                    {
                        ObavljenaVoznja = false,
                        PodruznicaKrajId = model.PodruznicaKrajId,
                        PodruznicaPocetakId = model.PodruznicaPocetakId,
                        PreuzetaRoba = false,
                        VozacId = model.VozacId,
                        VoznjaPocetak = new DateTime(1940,1,1),
                        VoznjaKraj = new DateTime(1940, 1, 1)
                    });
                }
                _context.SaveChanges();
            }
            catch
            {

            }
        }
    }
}
