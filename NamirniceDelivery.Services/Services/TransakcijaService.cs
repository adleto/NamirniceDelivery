﻿using Microsoft.EntityFrameworkCore;
using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Data.HelperModel;
using NamirniceDelivery.Services.Additional;
using NamirniceDelivery.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Cijena = Math.Round(namirnicaPodruznica.CijenaSaPopustom, 2)
            });
            namirnicaPodruznica.KolicinaNaStanju -= brojNamirnica;
            _context.SaveChanges();

            ObavjestiRadnike(namirnicaPodruznica.PodruznicaId);
        }

        public List<Transakcija> GetZavrseneTransakcijeForRadnik(AdministrativniRadnik radnik)
        {
            return GetTransakcijeForRadnik(radnik)
                .Where(t => t.DostavaUspjesna == true)
                .ToList();
        }

        public List<Transakcija> GetTransakcijeUTokuForKupac(Kupac kupac)
        {
            return GetTransakcijeForKupac(kupac)
                .Where(t => t.NarudzbaPrihvacenaOdRadnika == true &&
                    t.DostavaUspjesna == false)
                .ToList();
        }

        public List<Transakcija> GetNepotvrdjeneTransakcijeForKupac(Kupac kupac)
        {
            return GetTransakcijeForKupac(kupac)
                .Where(t => t.NarudzbaPrihvacenaOdRadnika == false)
                .ToList();
        }

        public List<Transakcija> GetNepotvrdjeneTransakcijeForPodruznica(Podruznica podruznica)
        {
            return GetTransakcijeForPodruznica(podruznica)
                .Where(t => t.NarudzbaPrihvacenaOdRadnika == false)
                .ToList();
        }

        public List<Transakcija> GetTransakcije()
        {
            return _context.Transakcija
                .Include(t => t.Kupac)
                    .ThenInclude(k=>k.OpcinaBoravka)
                .Include(t => t.AdministrativniRadnik)
                    .ThenInclude(ar => ar.OpcinaBoravka)
                .Include(t => t.Podruznica)
                .Include(t => t.TipTransakcije)
                .Include(t => t.KupljeneNamirnice)
                    .ThenInclude(kn => kn.Namirnica)
                        .ThenInclude(kn => kn.Kategorija)
                .ToList();
        }
        public List<Transakcija> GetTransakcije(ApplicationUser user)
        {
            return _context.Transakcija
                .Include(t => t.Kupac)
                .Include(t => t.AdministrativniRadnik)
                .Include(t => t.Podruznica)
                .Include(t => t.TipTransakcije)
                .Include(t => t.KupljeneNamirnice)
                    .ThenInclude(kn => kn.Namirnica)
                        .ThenInclude(kn => kn.Kategorija)
                .Where(t => t.AdministrativniRadnikId == user.Id || t.KupacId == user.Id)
                .ToList();
        }

        public List<Transakcija> GetTransakcijeForKupac(Kupac kupac)
        {
            return GetTransakcije()
                .Where(t => t.Kupac == kupac)
                .ToList();
        }

        public List<Transakcija> GetTransakcijeForPodruznica(Podruznica podruznica)
        {
            return GetTransakcije()
                .Where(t => t.Podruznica == podruznica)
                .ToList();
        }

        public List<Transakcija> GetTransakcijeForRadnik(AdministrativniRadnik radnik)
        {
            var tList = GetTransakcije();
            var list = new List<Transakcija>();
            foreach (var t in tList)
            {
                if (t.AdministrativniRadnik != null && t.AdministrativniRadnik == radnik)
                {
                    list.Add(t);
                }
            }
            return list;
        }

        public void RealizujKupovine(List<KorpaStavka> list)
        {
            if (list.Count == 0) return;
            List<Podruznica> razlicitePodruznice = new List<Podruznica>();
            foreach (var item in list)
            {
                if (!razlicitePodruznice.Contains(item.NamirnicaPodruznica.Podruznica))
                {
                    razlicitePodruznice.Add(item.NamirnicaPodruznica.Podruznica);
                }
            }
            foreach (var podruznica in razlicitePodruznice)
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
                    if (stavka.NamirnicaPodruznica.PodruznicaId == podruznica.Id)
                    {
                        _context.KupljeneNamirnice.Add(new KupljeneNamirnice
                        {
                            Kolicina = stavka.Kolicina,
                            Cijena = Math.Round(stavka.NamirnicaPodruznica.CijenaSaPopustom, 2),
                            Namirnica = stavka.NamirnicaPodruznica.Namirnica,
                            Transakcija = t
                        });
                        _context.KorpaStavka.Remove(stavka);

                        if (stavka.NamirnicaPodruznica.KolicinaNaStanju - stavka.Kolicina > 0)
                        {
                            // kolicina to 0
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
            foreach (var p in razlicitePodruznice)
            {
                ObavjestiRadnike(p.Id);
            }
        }

        public List<Transakcija> GetZavrseneTransakcijeForKupac(Kupac kupac)
        {
            return GetTransakcijeForKupac(kupac)
                .Where(t => t.DostavaUspjesna == true)
                .ToList();
        }

        public List<Transakcija> GetTransakcijeUTokuForRadnik(AdministrativniRadnik radnik)
        {
            return GetTransakcijeForRadnik(radnik)
                .Where(t => t.NarudzbaPrihvacenaOdRadnika == true &&
                    t.DostavaUspjesna == false)
                .ToList();
        }

        public Transakcija GetTansakcija(int transakcijaId)
        {
            return _context.Transakcija
                .Include(t => t.Kupac)
                .Include(t => t.AdministrativniRadnik)
                .Include(t => t.Podruznica)
                .Include(t => t.TipTransakcije)
                .Include(t => t.KupljeneNamirnice)
                    .ThenInclude(kn => kn.Namirnica)
                        .ThenInclude(kn => kn.Kategorija)
                .Where(t => t.Id == transakcijaId)
                .FirstOrDefault();
        }

        public decimal GetTotalProtok(ApplicationUser user)
        {
            return _context.KupljeneNamirnice
                .Include(kn => kn.Transakcija)
                .Where(kn => kn.Transakcija.KupacId == user.Id || kn.Transakcija.AdministrativniRadnikId == user.Id)
                .Select(kn => kn.Cijena * kn.Kolicina)
                .Cast<decimal?>()
                .Sum() ?? 0;
        }

        public Transakcija GetNajvecaTransakcija(ApplicationUser user)
        {
            var t = _context.Transakcija
                .Include(t => t.KupljeneNamirnice)
                .Where(t => t.KupacId == user.Id || t.AdministrativniRadnikId == user.Id)
                .ToList();
            if (!t.Any()) return null;
            return t.Where(tr => tr.IznosTotal == (t.Max(t => t.IznosTotal)))
                .FirstOrDefault();
        }

        public Tuple<string, int> GetNajNamirnica(ApplicationUser user)
        {
            var t = _context.Transakcija
                .Include(t => t.KupljeneNamirnice)
                    .ThenInclude(t => t.Namirnica)
                .Where(t => t.KupacId == user.Id || t.AdministrativniRadnikId == user.Id)
                .ToList();
            if (!t.Any()) return null;
            List<KupljeneN> list = new List<KupljeneN>();
            foreach (var transakcija in t)
            {
                foreach (var namirnica in transakcija.KupljeneNamirnice)
                {
                    if (PostojiNamirnica(list, namirnica))
                    {
                        DodajKolicinu(list, namirnica);
                    }
                    else
                    {
                        list.Add(new KupljeneN
                        {
                            Kolicina = namirnica.Kolicina,
                            Namirnica = namirnica.Namirnica
                        });
                    }
                }
            }
            var naj = new KupljeneN
            {
                Namirnica = list[0].Namirnica,
                Kolicina = list[0].Kolicina
            };
            foreach (var n in list)
            {
                if (n.Kolicina > naj.Kolicina)
                {
                    naj = n;
                }
            }
            return Tuple.Create(naj.Namirnica.Naziv, naj.Kolicina);
        }

        public Tuple<ApplicationUser, int> GetNajPartner(ApplicationUser user)
        {
            var t = _context.Transakcija
                .Include(t => t.Kupac)
                .Include(t => t.AdministrativniRadnik)
                .Where(t => t.KupacId == user.Id || t.AdministrativniRadnikId == user.Id)
                .ToList();
            if (!t.Any()) return null;
            List<KorisnikKolicina> list = new List<KorisnikKolicina>();
            foreach (var transakcija in t)
            {
                if (transakcija.KupacId == user.Id)
                {
                    if (transakcija.AdministrativniRadnik != null)
                    {
                        DodajKorisnika(list, transakcija.AdministrativniRadnik);
                    }
                }
                else
                {
                    DodajKorisnika(list, transakcija.Kupac);
                }
            }
            KorisnikKolicina naj = new KorisnikKolicina
            {
                Kolicina = 1,
                User = list[0].User
            };
            foreach (var item in list)
            {
                if (naj.Kolicina < item.Kolicina)
                {
                    naj = item;
                }
            }
            return new Tuple<ApplicationUser, int>(naj.User, naj.Kolicina);
        }
        public List<TransakcijeNamirnica> GetNamirniceUTransakcijiama(ApplicationUser user)
        {
            var kn = _context.KupljeneNamirnice
                .Include(kn => kn.Namirnica)
                .Include(kn => kn.Transakcija)
                .Where(kn => kn.Transakcija.KupacId == user.Id || kn.Transakcija.AdministrativniRadnikId == user.Id)
                .ToList();
            //if (!kn.Any()) return null;
            List<TransakcijeNamirnica> list = new List<TransakcijeNamirnica>();
            foreach (var n in kn)
            {
                if (PostojiTransakcijeNamirnica(list, n))
                {
                    DodajTransakcijeNamirnica(list, n);
                }
                else
                {
                    list.Add(new TransakcijeNamirnica
                    {
                        name = n.Namirnica.Naziv,
                        value = n.Kolicina
                    });
                }
            }
            return list;
        }

        private void ObavjestiRadnike(int podruznicaId)
        {
            var radnici = _context.AdministrativniRadnik
                .Where(a => a.PodruznicaId == podruznicaId)
                .ToList();
            foreach (var r in radnici)
            {
                //ugaseno za sad
                //NexmoSend.ObavjestiRadnikaNovaNarudzba(r);
            }
        }
        private bool PostojiTransakcijeNamirnica(List<TransakcijeNamirnica> list, KupljeneNamirnice n)
        {
            foreach (var item in list)
            {
                if (item.name == n.Namirnica.Naziv)
                {
                    return true;
                }
            }
            return false;
        }

        private void DodajTransakcijeNamirnica(List<TransakcijeNamirnica> list, KupljeneNamirnice n)
        {
            foreach (var item in list)
            {
                if (item.name == n.Namirnica.Naziv)
                {
                    item.value += n.Kolicina;
                    return;
                }
            }
        }

        private void DodajKorisnika(List<KorisnikKolicina> list, ApplicationUser user)
        {
            foreach (var t in list)
            {
                if (t.User.Id == user.Id)
                {
                    t.Kolicina++;
                    return;
                }
            }
            list.Add(new KorisnikKolicina
            {
                Kolicina = 1,
                User = user
            });
        }

        private void DodajKolicinu(List<KupljeneN> list, KupljeneNamirnice namirnica)
        {
            foreach (var item in list)
            {
                if (item.Namirnica == namirnica.Namirnica)
                {
                    item.Kolicina += namirnica.Kolicina;
                    return;
                }
            }
        }

        private bool PostojiNamirnica(List<KupljeneN> list, KupljeneNamirnice namirnica)
        {
            foreach (var item in list)
            {
                if (item.Namirnica == namirnica.Namirnica)
                {
                    return true;
                }
            }
            return false;
        }

        class KorisnikKolicina
        {
            public ApplicationUser User { get; set; }
            public int Kolicina { get; set; }
        }
        class KupljeneN
        {
            public Namirnica Namirnica { get; set; }
            public int Kolicina { get; set; }
        }
    }
}
