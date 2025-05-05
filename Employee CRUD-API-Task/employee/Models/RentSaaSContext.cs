using Microsoft.EntityFrameworkCore;

namespace employee.Models
{
    public class RentSaaSContext :DbContext
    {
        public DbSet<Employees> Employees { get; set; }
        public RentSaaSContext (DbContextOptions<RentSaaSContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed initial data
            modelBuilder.Entity<Employees>().HasData(
                new Employees { Id = 1, FirstName = "Moaz", LastName = "Hamed", Email = "MoazHamed123@example.com", Position = "Developer" },
                new Employees { Id = 2, FirstName = "Doha", LastName = "Medhat", Email = "DohaMedhat12@example.com", Position = "Designer" },
                new Employees { Id = 3, FirstName = "Shereen", LastName = "Ahmed", Email = "ShereenAhmed23@example.com", Position = "Manager" }
            );
        }
    }
}
