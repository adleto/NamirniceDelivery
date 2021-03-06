using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NamirniceDelivery.Data.Context;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Services;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.Hubs;

namespace NamirniceDelivery.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("PleskConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MyContext>();
            services.AddControllersWithViews();

            services.AddSignalR();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Account/Login";
                options.LogoutPath = $"/Account/Logout";
                options.AccessDeniedPath = $"/Account/Login";
            });

            services.AddScoped<IKanton, KantonService>();
            services.AddScoped<IOpcina, OpcinaService>();
            services.AddScoped<ITipTransakcije, TipTransakcijeService>();
            services.AddScoped<IPodruznica, PodruznicaService>();
            services.AddScoped<IKategorija, KategorijaService>();
            services.AddScoped<INamirnica, NamirnicaService>();
            services.AddScoped<IPopust, PopustService>();
            services.AddScoped<INamirnicaPodruznica, NamirnicaPodruznicaService>();
            services.AddScoped<IAdministrativniRadnik, AdministrativniRadnikService>();
            services.AddScoped<IKupac, KupacService>();
            services.AddScoped<IKorpaStavka, KorpaStavkaService>();
            services.AddScoped<ITransakcija, TransakcijaService>();
            services.AddScoped<IAkcijeTransakcija, AkcijeTransakcijaService>();
            services.AddScoped<IApplicationUser, ApplicationUserService>();
            services.AddScoped<IVozilo, VoziloService>();
            services.AddScoped<IVozac, VozacService>();
            services.AddScoped<IVoznja, VoznjaService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<MyHub>("/myHub");
            });
        }
    }
}
