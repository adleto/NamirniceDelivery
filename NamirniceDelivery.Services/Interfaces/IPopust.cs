using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IPopust
    {
        void KreirajPopust(Popust popust);
        List<Popust> GetPopusti();
        Popust GetPopust(int id);
        void EditPopust(Popust popust);
        void UkloniPopust(int popustId);
        List<bool> GetIsDeletable(List<Popust> popustList);
    }
}
