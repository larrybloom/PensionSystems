using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<Member> Members { get; }
        public IRepository<Employer> Employers { get; }
        public IRepository<Contribution> Contributions { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Members = new Repository<Member>(_context);
            Employers = new Repository<Employer>(_context);
            Contributions = new Repository<Contribution>(_context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
