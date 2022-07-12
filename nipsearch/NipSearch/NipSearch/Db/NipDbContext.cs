using Microsoft.EntityFrameworkCore;
using NipSearch.Entities;

namespace NipSearch.Db
{
    public class NipDbContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Account> AccountNumbers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\sql2019;Database=NipSearchDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.IdSubject);

                entity.Ignore(e => e.HasVirtualAccountsValue);

                entity.Ignore(e => e.AccountNumbers);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.IdPerson);

                entity.HasOne(e => e.SubjectRepresentative)
                .WithMany(e => e.Representatives)
                .HasForeignKey(e => e.IdSubjectRepresentative);

                entity.HasOne(e => e.SubjectAuthorizedClerk)
                .WithMany(e => e.AuthorizedClerks)
                .HasForeignKey(e => e.IdSubjectAuthorizedClerk);

                entity.HasOne(e => e.SubjectPartner)
                .WithMany(e => e.Partners)
                .HasForeignKey(e => e.IdSubjectPartner);
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.IdAccountNumber);

                entity.HasOne(e => e.Subject)
                .WithMany(e => e.Accounts)
                .HasForeignKey(e => e.IdSubject);
            });
        }
    }
}
