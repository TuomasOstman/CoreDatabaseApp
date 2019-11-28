using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.Entities.DB
{
    public class ThesisContext : DbContext
    {
        public DbSet<Seed> Seed { get; set; }
        public DbSet<RandomObject> RandomObject { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=OSTMYLLY\\SQLEXPRESS;Database=Thesis;Trusted_Connection=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RandomObject>(entity =>
            {
                entity.Property(e => e.RandomObjectID).ValueGeneratedNever();
            });
        }
    }

    public class Seed
    {
        public int SeedID { get; set; }
        public int SeedValue { get; set; }
    }

    public class RandomObject
    {
        public int RandomObjectID { get; set; }
        public string RandomString { get; set; }
        public DateTimeOffset RandomDateTimeOffset { get; set; }
        public int RandomInt { get; set; }
        public int SeedID { get; set; }
    }
}