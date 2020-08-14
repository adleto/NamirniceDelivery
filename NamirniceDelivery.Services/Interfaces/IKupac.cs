using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IKupac
    {
        void SMSObavjestPoslana(Kupac kupac);
        List<Kupac> GetKupciZaSMSObavijest();
        Kupac GetKupac(string username);
        void DodajSpremljenuNamiricu(string id, int namirnicaPodruznicaId);
        void DodajSpremljenuPodruznicu(string id, int podruznicaId);
        void UkloniSpremljenuNamiricu(string id, int namirnicaPodruznicaId);
        void UkloniSpremljenuPodruznicu(string id, int podruznicaId);
        List<KupacSpremljenePodruznice> GetSpremljenePodruznice(string id);
        List<KupacSpremljeneNamirnice> GetSpremljeneNamirnice(string id);
        List<Kupac> GetKupci();
        KupacProfilViewModel GetKupacData(string username);
    }
}
