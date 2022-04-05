using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangements.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }

        public DbSet<Employee> employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach(var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;

                //modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                //    new IdentityUserRole<string>
                //    {
                //        UserId= "b1cdc902-6906-4275-abca-435221b29212",
                //        RoleId= "65d5c2b1-5456-4f33-92b1-8715046fe05b"
                //    }
                //    );
            }
        }
        
    }
}
