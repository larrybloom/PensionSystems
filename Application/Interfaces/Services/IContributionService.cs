using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IContributionService
    {
        Task<ContributionDto> AddContributionAsync(ContributionCreateDto contribution);
        Task<IEnumerable<ContributionDto>> GetMemberContributionsAsync(Guid memberId);
        Task<decimal> GetTotalContributionsAsync(Guid memberId);
    }
}
