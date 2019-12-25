using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IKupac
    {
        Kupac GetKupac(string username);
        void DodajSpremljenuNamiricu(string id, int namirnicaPodruznicaId);
        void DodajSpremljenuPodruznicu(string id, int podruznicaId);
    }
}
