using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Services
{
    public class TransakcijaService : ITransakcija
    {
        private readonly MyContext _context;

        public TransakcijaService(MyContext context)
        {
            _context = context;
        }

        public void BrzaKupovina(NamirnicaPodruznica namirnicaPodruznica, int brojNamirnica, Kupac kupac)
        {
            Transakcija t = new Transakcija
            {
                DatumIniciranjaTransakcije = DateTime.Now,
                DostavaUspjesna = false,
                Kupac = kupac,
                KupacOstavioDojam = false,
                NarudzbaPrihvacenaOdRadnika = false,
                RadnikOstavioDojam = false,
                TipTransakcije = _context.TipTransakcije.Find(1),
                PodruznicaId = namirnicaPodruznica.PodruznicaId
            };
            _context.Transakcija.Add(t);
            _context.KupljeneNamirnice.Add(new KupljeneNamirnice
            {
                Kolicina = brojNamirnica,
                NamirnicaId = namirnicaPodruznica.NamirnicaId,
                Transakcija = t,
                Cijena = Math.Round(namirnicaPodruznica.CijenaSaPopustom,2)
            });
            namirnicaPodruznica.KolicinaNaStanju -= brojNamirnica;
            _context.SaveChanges();
        }

        public void RealizujKupovine(List<KorpaStavka> list)
        {
            if (list.Count == 0) return;
            List<Podruznica> razlicitePodruznice = new List<Podruznica>();
            foreach(var item in list)
            {
                if (!razlicitePodruznice.Contains(item.NamirnicaPodruznica.Podruznica))
                {
                    razlicitePodruznice.Add(item.NamirnicaPodruznica.Podruznica);
                }
            }
            foreach(var podruznica in razlicitePodruznice)
            {
                Transakcija t = new Transakcija
                {
                    DatumIniciranjaTransakcije = DateTime.Now,
                    DostavaUspjesna = false,
                    Kupac = list[0].Kupac,
                    KupacOstavioDojam = false,
                    NarudzbaPrihvacenaOdRadnika = false,
                    RadnikOstavioDojam = false,
                    TipTransakcije = _context.TipTransakcije.Find(1),
                    PodruznicaId = podruznica.Id
                };

                foreach (var stavka in list)
                {
                    if (stavka.NamirnicaPodruznicaId == podruznica.Id)
                    {
                        _context.KupljeneNamirnice.Add(new KupljeneNamirnice
                        {
                            Kolicina = stavka.Kolicina,
                            Cijena = Math.Round(stavka.NamirnicaPodruznica.CijenaSaPopustom, 2),
                            Namirnica = stavka.NamirnicaPodruznica.Namirnica,
                            Transakcija = t
                        });
                        _context.KorpaStavka.Remove(stavka);

                        if(stavka.NamirnicaPodruznica.KolicinaNaStanju- stavka.Kolicina > 0) {
                            // this is you know
                            stavka.NamirnicaPodruznica.KolicinaNaStanju -= stavka.Kolicina;
                        }
                        else
                        {
                            stavka.NamirnicaPodruznica.KolicinaNaStanju = 0;
                            stavka.NamirnicaPodruznica.Aktivna = false;
                        }
                    }
                }
                _context.Transakcija.Add(t);
            }
            _context.SaveChanges();
        }
    }
}
