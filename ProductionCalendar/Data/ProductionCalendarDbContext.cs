using Microsoft.EntityFrameworkCore;
using ProductionCalendar.Models;

namespace ProductionCalendar.Data
{
    public class ProductionCalendarDbContext : DbContext
    {
        public DbSet<Calendar> days { get; set; }
        public ProductionCalendarDbContext(DbContextOptions opt) : base(opt)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Calendar>().HasKey(d => d.Date);
            modelBuilder.Entity<Calendar>().ToTable(nameof(Calendar).ToLower());
        }
    }
}
