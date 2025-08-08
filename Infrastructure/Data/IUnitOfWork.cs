using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Member> Members { get; }
        IRepository<Employer> Employers { get; }
        IRepository<Contribution> Contributions { get; }

        Task<int> SaveChangesAsync();
    }
}
