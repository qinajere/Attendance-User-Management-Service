using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace AttendanceUserManagementSystem.API.Authentication
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department>Departments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasIndex(u => u.EmployeeCode).IsUnique();

            builder.Entity<ApplicationUser>()
                .HasOne<Branch>(s => s.Branch)
                .WithMany(ad => ad.Users)
                .HasForeignKey(s => s.BranchId);

            builder.Entity<ApplicationUser>()
               .HasOne<Department>(s => s.Department)
               .WithMany(ad => ad.Users)
               .HasForeignKey(s => s.DepartmentId);

            base.OnModelCreating(builder);

        }

    }
}
