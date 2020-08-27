using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IVozac
    {
        List<VozaciZaDisplay> GetVozaciSimple();
        void Deactivate(string vozacId);
        List<Vozac> Get();
        Vozac GetVozac(string username);
        Vozac GetVozacById(string id);

        Task Vozac(VozacViewModel vozac);




    }
}
