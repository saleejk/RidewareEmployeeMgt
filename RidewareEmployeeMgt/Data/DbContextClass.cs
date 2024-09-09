using Microsoft.EntityFrameworkCore;
using RidewareEmployeeMgt.Models;

namespace RidewareEmployeeMgt.Data
{
    public class DbContextClass : DbContext
    {
        public readonly string connectionString;
        public DbContextClass(IConfiguration config)
        {
            connectionString = config["ConnectionStrings:DefaultConnection"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var hashedPassword1 = BCrypt.Net.BCrypt.HashPassword("12345678");
            var hashedPassword2 = BCrypt.Net.BCrypt.HashPassword("12345678");
            modelBuilder.Entity<Employee>().Property(e => e.Salary).HasPrecision(18, 2);
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Email = "admin@gmail.com", Password = hashedPassword1, Role = "Admin" }, new User { Id = 2, Email = "rideware@gmail.com", Password = hashedPassword2, Role = "Admin" });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
