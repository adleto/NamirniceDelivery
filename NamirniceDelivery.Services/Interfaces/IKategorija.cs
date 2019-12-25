using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IKategorija
    {
        void KreirajKategoriju(Kategorija kategorija);
        List<Kategorija> GetKategorije();
        Kategorija GetKategorija(int id);
        void EditKategorija(Kategorija kategorija);
        void UkloniKategorija(int id);
        List<bool> GetIsDeletable(List<Kategorija> kategorijaList);
    }
}
