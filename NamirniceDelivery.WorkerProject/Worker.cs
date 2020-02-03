using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _kupacService = scope.ServiceProvider.GetRequiredService<IKupac>();
                
                //var _userResolverService = scope.ServiceProvider.GetRequiredService<IUserResolverService>();
                //var currentUserName = _userResolverService.GetUsernameOfCurrentUser();
                //if (currentUserName != null)
                //{
                //    _kupacService.PovecajRejtingUkolikoZadovoljavaUslove(currentUserName);
                //}

                //var kupci = _kupacService.GetKupciZaSMSObavijest();

                //NexmoSend.PodsjetiKupce(kupci);
                //foreach (var k in kupci)
                //{
                //    _kupacService.SMSObavjestPoslana(k);
                //}
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
