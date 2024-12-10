using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Portfolio>().HasOne(p => p.AppUser).WithMany(a => a.Portfolios).HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>().HasOne(p => p.Stock).WithMany(s => s.Portfolios).HasForeignKey(p => p.StockId);

            // builder.Entity<IdentityRole>().HasData(
            //     new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            //     new IdentityRole { Name = "User", NormalizedName = "USER" });


        }
    }
}