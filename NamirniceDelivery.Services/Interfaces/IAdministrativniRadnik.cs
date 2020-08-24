using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IAdministrativniRadnik
    {
        AdministrativniRadnik GetRadnik(string username);
        AdministrativniRadnik GetRadnikById(string id);
        int GetPodruznicaIdOdRadnika(string username);
        List<AdministrativniRadnik> Get();
        void Deactivate(string radnikId);
        Task Radnik(AdminRadnikViewModel radnik);
    }
}
