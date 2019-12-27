using System;
using System.Collections.Generic;
using System.Text;
using NamirniceDelivery.Data.Entities;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IAkcijeTransakcija
    {
        void OdobriTranskaciju(int transakcijaId, AdministrativniRadnik radnik);
        void DostavaUspjela(int transakcijaId, Kupac kupac);
    }
}
