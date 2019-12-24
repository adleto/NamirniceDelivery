using System;
using System.Collections.Generic;
using System.Text;
using NamirniceDelivery.Data.Entities;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IKorpaStavka
    {
        void DodajUKorpu(int namirnicaPodruznicaId, int brojNamirnica, Kupac kupac);
        List<KorpaStavka> GetNamirniceUKorpi(Kupac kupac);
        decimal GetTotalCijena(Kupac kupac);
        void UkloniStavku(int id, Kupac kupac);
        KorpaStavka GetKorpaStavka(int id);
    }
}
