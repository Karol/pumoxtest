using Microsoft.EntityFrameworkCore;
using PumoxTest.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PumoxTest.DataBase
{
    public class PumoxTestContext : DbContext
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<Employe> Employe { get; set; }

        public PumoxTestContext(DbContextOptions<PumoxTestContext> options) : base(options)
        {

        }

        public static PumoxTestContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PumoxTestContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PumoxTestContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Employe>().ToTable("Employe");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
