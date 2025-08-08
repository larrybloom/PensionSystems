using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Member> Members => Set<Member>();
        public DbSet<Employer> Employers => Set<Employer>();
        public DbSet<Contribution> Contributions => Set<Contribution>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasQueryFilter(m => !m.IsDeleted);

            modelBuilder.Entity<Contribution>()
                .Property(c => c.Amount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
