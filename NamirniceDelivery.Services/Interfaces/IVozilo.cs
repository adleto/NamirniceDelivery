using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IVozilo
    {
        List<Vozilo> GetVozila();
    }
}
