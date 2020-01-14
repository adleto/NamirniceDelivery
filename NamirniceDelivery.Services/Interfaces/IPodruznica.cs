using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IPodruznica
    {
        void KreirajPodruznicu(Podruznica podruznica);
        List<Podruznica> GetPodruzniceForKupac(Kupac kupac);
        List<Podruznica> GetPodruznice();
        Podruznica GetPodruznica(int podruznicaId);
        void EditPodruznica(Podruznica podruznica);
    }
}
