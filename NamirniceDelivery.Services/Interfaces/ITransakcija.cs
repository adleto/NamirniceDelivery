using System;
using System.Collections.Generic;
using System.Text;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Data.HelperModel;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface ITransakcija
    {
        void BrzaKupovina(NamirnicaPodruznica namirnicaPodruznica, int brojNamirnica, Kupac kupac);
        void RealizujKupovine(List<KorpaStavka> list);
        List<Transakcija> GetTransakcije();
        public List<Transakcija> GetTransakcijeForKupac(Kupac kupac);
        public List<Transakcija> GetNepotvrdjeneTransakcijeForKupac(Kupac kupac);
        public List<Transakcija> GetTransakcijeForPodruznica(Podruznica podruznica);
        public List<Transakcija> GetTransakcijeForRadnik(AdministrativniRadnik radnik);
        public List<Transakcija> GetNepotvrdjeneTransakcijeForPodruznica(Podruznica podruznica);
        List<Transakcija> GetTransakcijeUTokuForKupac(Kupac kupac);
        List<Transakcija> GetZavrseneTransakcijeForRadnik(AdministrativniRadnik radnik);
        List<Transakcija> GetZavrseneTransakcijeForKupac(Kupac kupac);
        List<Transakcija> GetTransakcijeUTokuForRadnik(AdministrativniRadnik radnik);
        Transakcija GetTansakcija(int transakcijaId);
        decimal GetTotalProtok(ApplicationUser user);
        Transakcija GetNajvecaTransakcija(ApplicationUser user);
        Tuple<string, int> GetNajNamirnica(ApplicationUser user);
        Tuple<ApplicationUser,int> GetNajPartner(ApplicationUser user);
        public List<Transakcija> GetTransakcije(ApplicationUser user);
        List<TransakcijeNamirnica> GetNamirniceUTransakcijiama(ApplicationUser user);
    }
}
