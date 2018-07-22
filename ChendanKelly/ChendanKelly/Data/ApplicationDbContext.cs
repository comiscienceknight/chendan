using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChendanKelly.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}
        
        public DbSet<Baobei> Baobeis { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Settle> Settles { get; set; }
    }
}
