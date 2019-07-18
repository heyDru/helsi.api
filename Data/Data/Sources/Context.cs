using Common.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Sources
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Patient>(patient =>
                {
                    patient.HasKey(k => k.Id);
                    patient.Property(p => p.Id);
                    patient.Property(p => p.UserId);
                    patient.Property(p => p.FirstName);
                    patient.Property(p => p.LastName);
                    patient.Property(p => p.Birthday);
                    patient.Property(p => p.Gender).HasDefaultValue(Gender.Other);
                    patient.Property(p => p.Phone);
                    //patient.Property(p => p.Activated);
                }
            );
        }
    }
}
