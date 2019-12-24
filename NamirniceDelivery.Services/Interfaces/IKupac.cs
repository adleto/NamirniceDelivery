using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IKupac
    {
        Kupac GetKupac(string username);
    }
}
