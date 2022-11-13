using Horoscope.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Horoscope.DAL
{
    public class HoroscopeContext : DbContext
    {
        public DbSet<ZodiakSign> ZodiakSigns { get; set; }

        public DbSet<Prediction> Predictions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Horoscope;Trusted_Connection=True;");
        }
    }
}
