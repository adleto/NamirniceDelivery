using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IAdministrativniRadnik
    {
        AdministrativniRadnik GetRadnik(string username);
        int GetPodruznicaIdOdRadnika(string username);
    }
}
