using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SSProfilingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSProfilingApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<IndividualData> Individuals => Set<IndividualData>();
        public DbSet<DataProfile> DataProfiles => Set<DataProfile>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndividualData>(entity=>{
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.MiddleName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.BirthPlace).HasMaxLength(100);
                entity.Property(e => e.BirthDate).HasMaxLength(20);
                entity.Property(e => e.Nationality).HasMaxLength(100);
                entity.Property(e => e.IdentityNumber).HasMaxLength(20);
            });


            modelBuilder.Entity<DataProfile>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProfileId).IsRequired();

                entity.HasOne(e => e.IndividualData)
                      .WithMany()
                      .HasForeignKey(e => e.IndividualDataId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }



    }

}
