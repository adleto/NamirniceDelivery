using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NamirniceDelivery.Data.Entities;

namespace NamirniceDelivery.Data.Context
{
    public class MyContext : IdentityDbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Transakcija>()
                .HasOne(t => t.Kupac)
                .WithMany(p => p.Transakcije)
                .IsRequired();
            modelBuilder.Entity<KupljeneNamirnice>()
                .HasOne(k => k.Transakcija)
                .WithMany(t => t.KupljeneNamirnice)
                .IsRequired();
            modelBuilder.Entity<KorpaStavka>()
                .HasOne(k => k.NamirnicaPodruznica)
                .WithMany()
                .IsRequired();
            modelBuilder.Entity<NamirnicaPodruznica>()
                .HasOne(n => n.Podruznica)
                .WithMany(p => p.NamirnicaPodruznica)
                .IsRequired();
            modelBuilder.Entity<KupacSpremljeneNamirnice>()
                .HasOne(k => k.NamirnicaPodruznica)
                .WithMany()
                .IsRequired();
            modelBuilder.Entity<KupacSpremljeneNamirnice>()
                .HasOne(k => k.Kupac)
                .WithMany(k => k.SpreljeneNamirnice)
                .IsRequired();
            modelBuilder.Entity<KupacSpremljenePodruznice>()
                .HasOne(k => k.Podruznica)
                .WithMany()
                .IsRequired();
            modelBuilder.Entity<KupacSpremljenePodruznice>()
                .HasOne(k => k.Kupac)
                .WithMany(k => k.SpremljenePodruznice)
                .IsRequired();
            modelBuilder.Entity<AdministrativniRadnik>()
                .HasOne(r => r.Podruznica)
                .WithMany()
                .IsRequired();
            modelBuilder.Entity<NamirnicaVoznja>()
                .HasOne(nv => nv.Voznja)
                .WithMany(v => v.NamirnicaVoznja)
                .IsRequired();
        }
        public DbSet<KupljeneNamirnice> KupljeneNamirnice { get; set; }
        public DbSet<Transakcija> Transakcija { get; set; }
        public DbSet<TipTransakcije> TipTransakcije { get; set; }
        public DbSet<Popust> Popust { get; set; }
        public DbSet<KupacSpremljeneNamirnice> KupacSpremljeneNamirnice { get; set; }
        public DbSet<KupacSpremljenePodruznice> KupacSpremljenePodruznice { get; set; }
        public DbSet<Opcina> Opcina { get; set; }
        public DbSet<Kanton> Kanton { get; set; }
        public DbSet<Kategorija> Kategorija { get; set; }
        public DbSet<Namirnica> Namirnica { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Kupac> Kupac { get; set; }
        public DbSet<Vozac> Vozac { get; set; }
        public DbSet<AdministrativniRadnik> AdministrativniRadnik { get; set; }
        public DbSet<Podruznica> Podruznica { get; set; }
        public DbSet<NamirnicaPodruznica> NamirnicaPodruznica { get; set; }
        public DbSet<KorpaStavka> KorpaStavka { get; set; }
        public DbSet<Voznja> Voznja { get; set; }
        public DbSet<Vozilo> Vozilo { get; set; }
        public DbSet<NamirnicaVoznja> NamirnicaVoznja { get; set; }
    }
}
