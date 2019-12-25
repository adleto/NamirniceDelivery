using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class PopustService : IPopust
    {
        private readonly MyContext _context;

        public PopustService(MyContext context)
        {
            _context = context;
        }

        public void EditPopust(Popust popust)
        {
            var p = GetPopust(popust.Id);
            p.Iznos = popust.Iznos;
            p.Opis = popust.Opis;
            _context.SaveChanges();
        }

        public List<bool> GetIsDeletable(List<Popust> popustList)
        {
            List<bool> list = new List<bool>();
            foreach (var item in popustList)
            {
                if (_context.NamirnicaPodruznica.Where(np => np.Popust == item).Any())
                {
                    list.Add(false);
                }
                else
                {
                    list.Add(true);
                }
            }
            return list;
        }

        public Popust GetPopust(int id)
        {
            return _context.Popust.Find(id);
        }

        public List<Popust> GetPopusti()
        {
            return _context.Popust.ToList();
        }

        public void KreirajPopust(Popust popust)
        {
            _context.Popust.Add(popust);
            _context.SaveChanges();
        }

        public void UkloniPopust(int popustId)
        {
            _context.Popust.Remove(GetPopust(popustId));
            _context.SaveChanges();
        }
    }
}
