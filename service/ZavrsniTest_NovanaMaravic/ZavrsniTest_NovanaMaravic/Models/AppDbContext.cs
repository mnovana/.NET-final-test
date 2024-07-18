using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ZavrsniTest_NovanaMaravic.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Projekat> Projekti { get; set; }
        public DbSet<Istrazivac> Istrazivaci { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Projekat>().HasData(
                new Projekat() { Id = 1, Naziv = "Gerudok", GodinaStart = 2020, GodinaKraj = 2025 },
                new Projekat() { Id = 2, Naziv = "Gemed", GodinaStart = 2022, GodinaKraj = 2028 },
                new Projekat() { Id = 3, Naziv = "Artist", GodinaStart = 2020, GodinaKraj = 2027 }
            );

            builder.Entity<Istrazivac>().HasData(
                new Istrazivac() { Id = 1, Ime = "Marko", Prezime = "Lukic", Zarada = 32126.32M, GodinaRodjenja = 2000, ProjekatId = 3 },
                new Istrazivac() { Id = 2, Ime = "Ana", Prezime = "Maric", Zarada = 105799.99M, GodinaRodjenja = 1977, ProjekatId = 1 },
                new Istrazivac() { Id = 3, Ime = "Petar", Prezime = "Lukic", Zarada = 42292.37M, GodinaRodjenja = 1995, ProjekatId = 2 },
                new Istrazivac() { Id = 4, Ime = "Luka", Prezime = "Panic", Zarada = 78264.88M, GodinaRodjenja = 1991, ProjekatId = 3 },
                new Istrazivac() { Id = 5, Ime = "Marija", Prezime = "Milic", Zarada = 424201.22M, GodinaRodjenja = 2002, ProjekatId = 1 }
            );


            base.OnModelCreating(builder);
        }
    }
}
