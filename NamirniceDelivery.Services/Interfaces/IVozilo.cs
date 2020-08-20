using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IVozilo
    {
        List<Vozilo> GetVozila();
        void Add_EditVozilo(int? id, VoziloModel model);
        void Delete(int id);
        Vozilo GetVozilo(int id);
    }
}
