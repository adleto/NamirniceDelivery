using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface INamirnica
    {
        void KreirajNamirnica(Namirnica namirnica);
        Namirnica GetNamirnica(int id);
        List<Namirnica> GetNamirnice();
        List<Namirnica> GetNamirnicePoKategorijama(Kategorija kategorija);
        void EditNamirnica(Namirnica namirnica);
        void UkloniNamirnica(int namirnicaId);
        List<bool> GetIsDeletable(List<Namirnica> namirnicaList);
    }
}
