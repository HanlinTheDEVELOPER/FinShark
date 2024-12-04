using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Model;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}