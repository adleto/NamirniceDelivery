using System;
using System.Collections.Generic;
using System.Text;
using NamirniceDelivery.Data.Entities;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface ITransakcija
    {
        void BrzaKupovina(NamirnicaPodruznica namirnicaPodruznica, int brojNamirnica, Kupac kupac);
        void RealizujKupovine(List<KorpaStavka> list);
    }
}
