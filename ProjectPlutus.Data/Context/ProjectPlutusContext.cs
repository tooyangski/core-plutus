using Microsoft.EntityFrameworkCore;
using ProjectPlutus.Domain.Models;

namespace ProjectPlutus.Data.Context
{
    public class ProjectPlutusContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

        public ProjectPlutusContext(DbContextOptions<ProjectPlutusContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Temperature = 37.5
                },
                new Employee()
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Temperature = 36.5
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "superadmin@plutus.com",
                    Password = "T$3EhU6XfTytkRaD62LAoSiC"
                }
            );
        }
    }
}
