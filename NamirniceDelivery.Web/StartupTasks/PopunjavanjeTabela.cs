using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace NamirniceDelivery.Web.StartupTasks
{
    public static class PopunjavanjeTabela
    {
        public static async Task DodajKantone(IServiceProvider serviceProvider)
        {
            var _kantonService = serviceProvider.GetRequiredService<IKanton>();
            List<Tuple<string, string>> kantoni = new List<Tuple<string, string>> {
                Tuple.Create("Kanton Sarajevo", "KS"),
                Tuple.Create("Zeničko-Dobojski","ZDK"),
                Tuple.Create("Hercegovačko-neretvanski","HNK"),
                Tuple.Create("Zapadnohercegovački","ZHK"),
                Tuple.Create("Srednjobosanski","SBK"),
                Tuple.Create("Bosansko-podrinjski","BPK"),
                Tuple.Create("Tuzlanski","TK"),
                Tuple.Create("Posavski","PK"),
                Tuple.Create("Unsko-sanski","USK"),
                Tuple.Create("Hercegbosanski","HBK")
            };
            foreach(var t in kantoni)
            {
                var kanton = new Kanton
                {
                    Naziv = t.Item1,
                    Oznaka = t.Item2
                };
                await _kantonService.KreirajKanton(kanton);
            }
        }
        public static async Task DodajOpcine(IServiceProvider serviceProvider)
        {
            var _opcinaService = serviceProvider.GetRequiredService<IOpcina>();
            List<Tuple<string, int>> opcine = new List<Tuple<string, int>> {
                Tuple.Create("Banovići",7),Tuple.Create("Bosanska Krupa",9),Tuple.Create("Bihać",9),Tuple.Create("Bosanski Petrovac",9),Tuple.Create("Bosansko Grahovo",10),Tuple.Create("Breza",2),Tuple.Create("Bugojno",5),Tuple.Create("Busovača",5),Tuple.Create("Bužim",9),Tuple.Create("Čapljina",3),Tuple.Create("Cazin",9),Tuple.Create("Centar Sarajevo",1),Tuple.Create("Čitluk",3),Tuple.Create("Drvar",10),Tuple.Create("Doboj",2),Tuple.Create("Istočni Doboj",2),Tuple.Create("Vakuf",5),Tuple.Create("Fojnica",5),Tuple.Create("Glamoč",10),Tuple.Create("Goražde",6),Tuple.Create("Gornji Vakuf",5),Tuple.Create("Gračanica",7),Tuple.Create("Gradačac",7),Tuple.Create("Grude",4),Tuple.Create("Hadžići",1),Tuple.Create("Ilidža",1),Tuple.Create("Ilijaš",1),
                Tuple.Create("Jablanica",3),Tuple.Create("Jajce",5),Tuple.Create("Kakanj",2),Tuple.Create("Kalesija",7),Tuple.Create("Kiseljak",5),Tuple.Create("Kladanj",7),
                Tuple.Create("Ključ",9),Tuple.Create("Konjic",3),Tuple.Create("Kreševo",5),Tuple.Create("Kupres",4),Tuple.Create("Livno",4),Tuple.Create("Ljubuški",4),
                Tuple.Create("Lukavac",7),Tuple.Create("Maglaj",2),Tuple.Create("Mostar",3),Tuple.Create("Neum",3),Tuple.Create("Novi Grad Sarajevo",1),
                Tuple.Create("Novo Sarajevo",1),Tuple.Create("Travnik",5),Tuple.Create("Odžak",8),Tuple.Create("Olovo",2),
                Tuple.Create("Orašje",8),Tuple.Create("Pale",1),
                Tuple.Create("Posušje",4),Tuple.Create("Prozor",3),Tuple.Create("Sanski Most",9),Tuple.Create("Srebrenik",7),
                Tuple.Create("Stari Grad Sarajevo",1),Tuple.Create("Stolac",3),Tuple.Create("Teočak",7),
                Tuple.Create("Tešanj",2),
                Tuple.Create("Tomislavgrad",4),Tuple.Create("Trnovo",1),Tuple.Create("Tuzla",7),Tuple.Create("Usora",4),
                Tuple.Create("Vareš",2),
                Tuple.Create("Velika Kladuša",9),Tuple.Create("Visoko",2),Tuple.Create("Vitez",5),Tuple.Create("Vogošća",1),
                Tuple.Create("Zavidovići",2),Tuple.Create("Zenica",2),Tuple.Create("Žepče",2),Tuple.Create("Živinice",7)
            };
            foreach (var t in opcine)
            {
                var opcina = new Opcina
                {
                    Naziv = t.Item1,
                    KantonId = t.Item2
                };
                await _opcinaService.KreirajOpcinu(opcina);
            }
        }
        public static async Task DodajTipTransakcije(IServiceProvider serviceProvider)
        {
            var _tipTransakcijeService = serviceProvider.GetRequiredService<ITipTransakcije>();
            List<string> tip = new List<string> {
                "Keš"
            };
            foreach (var t in tip)
            {
                var tipTransakcije = new TipTransakcije
                {
                    NazivTipa = t
                };
                await _tipTransakcijeService.KreirajTipTransakcije(tipTransakcije);
            }
        }
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "AdministrativniRadnik", "Kupac", "Menadzer", "Vozac" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        public static async Task CreateFirstUsers(IServiceProvider serviceProvider)
        {
            //Create first users and add them to roles
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var _podruznicaService = serviceProvider.GetRequiredService<IPodruznica>();
            ApplicationUser menadzer = new ApplicationUser
            {
                UserName = "MenadzerMain",
                Email = "MenadzerMain@namirnica.com",
                Ime = "MenadzerIme",
                Prezime = "MenadzerPrezime",
                OpcinaRodjenjaId = 1,
                OpcinaBoravkaId = 1
            };
            Kupac kupac = new Kupac
            {
                UserName = "testKupac",
                Email = "kupacTest@namirnica.com",
                Ime = "kupacTestIme",
                Prezime = "kupacTestPrezime",
                Adresa = "kupacTest adresa BB",
                OpcinaRodjenjaId = 1,
                OpcinaBoravkaId = 1,
                RejtingKupac = 0
            };
            AdministrativniRadnik administrativniRadnik = new AdministrativniRadnik
            {
                UserName = "testRadnik",
                Email = "testRadnik@namirnica.com",
                Ime = "testRadnikIme",
                Prezime = "testRadnikPrezime",
                OpcinaRodjenjaId = 1,
                OpcinaBoravkaId = 1,
                RejtingRadnik = 0,
                JMBG = "1010101010101"
            };
            Podruznica podruznica = new Podruznica { 
                Adresa = "Adresa BB",
                OpcinaId = 1,
                Opis = "Glavna podružnica",
                Naziv = "Granap 1"
            };
            Podruznica podruznica2 = new Podruznica
            {
                Adresa = "Adresa BB",
                OpcinaId = 1,
                Opis = "Glavno sladište",
                Naziv = "Granap Skladište 1"
            };
            _podruznicaService.KreirajPodruznicu(podruznica);
            _podruznicaService.KreirajPodruznicu(podruznica2);

            administrativniRadnik.Podruznica = podruznica;

            //Create users
            await UserManager.CreateAsync(menadzer, "password");
            await UserManager.CreateAsync(administrativniRadnik, "password");
            await UserManager.CreateAsync(kupac, "password");
            //Add users to roles
            await UserManager.AddToRoleAsync(menadzer, "Menadzer");
            await UserManager.AddToRoleAsync(administrativniRadnik, "AdministrativniRadnik");
            await UserManager.AddToRoleAsync(kupac, "Kupac");
        }
    }
}
