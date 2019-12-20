using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IKanton
    {
        List<Kanton> GetKantoni();
        Task KreirajKanton(Kanton kanton);
    }
}
