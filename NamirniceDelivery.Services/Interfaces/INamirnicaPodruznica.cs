using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface INamirnicaPodruznica
    {
        void DodajNamirnicu(NamirnicaPodruznica namirnicaPodruznica);
        List<NamirnicaPodruznica> GetNamirnicePodruznica();
        List<NamirnicaPodruznica> GetNamirnicePodruznica(int podruznicaId);
        List<NamirnicaPodruznica> GetNamirnicePodruznicaVrsta(Namirnica namirnica);
        List<NamirnicaPodruznica> GetNamirnicePodruznicaKategorija(Kategorija kategorija);
        List<NamirnicaPodruznica> GetNamirnicePodruznicaKategorija(Kategorija kategorija, int podruznicaId);
        List<NamirnicaPodruznica> GetNamirnicePodruznicaVrsta(Namirnica namirnica, int podruznicaId);
        void UkloniNamirnicaPodruznica(int id);
        NamirnicaPodruznica GetNamirnicaPodruznica(int id);
        void ToogleStatusNamirnicaPodruznica(int id);
    }
}
