using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NamirniceDelivery.Services.Additional;
using NamirniceDelivery.Services.Interfaces;

namespace NamirniceDelivery.WorkerProject
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _kupacService = scope.ServiceProvider.GetRequiredService<IKupac>();
                var kupci = _kupacService.GetKupciZaSMSObavijest();

                NexmoSend.PodsjetiKupce(kupci);
                foreach (var k in kupci)
                {
                    _kupacService.SMSObavjestPoslana(k);
                }

                await Task.Delay(2*24*60*60*1000, stoppingToken);
            }
        }
    }
}
