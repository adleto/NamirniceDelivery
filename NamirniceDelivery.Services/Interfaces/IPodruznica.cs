﻿using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NamirniceDelivery.Services.Interfaces
{
    public interface IPodruznica
    {
        Task KreirajPodruznicu(Podruznica podruznica);
    }
}
