using System.Data.Entity;
using Calendar.Domain.Entities;

namespace Calendar.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Day> Days { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}
