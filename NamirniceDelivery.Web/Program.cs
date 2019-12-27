using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NamirniceDelivery.Web.StartupTasks;

namespace NamirniceDelivery.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostsBuilder = CreateHostBuilder(args).Build();

            //using (var scope = hostsBuilder.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var serviceProvider = services.GetRequiredService<IServiceProvider>();
            //        PopunjavanjeTabela.DodajKantone(serviceProvider).Wait();
            //        PopunjavanjeTabela.DodajOpcine(serviceProvider).Wait();
            //        PopunjavanjeTabela.DodajTipTransakcije(serviceProvider).Wait();
            //        PopunjavanjeTabela.CreateRoles(serviceProvider).Wait();
            //        PopunjavanjeTabela.CreateFirstUsers(serviceProvider).Wait();
            //        PopunjavanjeTabela.CreateNamirnice(serviceProvider);
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "Error!");
            //    }
            //}

            hostsBuilder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
