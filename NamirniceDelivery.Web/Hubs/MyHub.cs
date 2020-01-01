using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.Hubs
{
    public class MyHub:Hub
    {
        public async Task BroadcastData()
        {
            await Clients.All.SendAsync("Repopulate");
        }
    }
}
