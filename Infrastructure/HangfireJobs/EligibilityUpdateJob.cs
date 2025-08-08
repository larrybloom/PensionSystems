using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HangfireJobs
{
    public class EligibilityUpdateJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EligibilityUpdateJob> _logger;

        public EligibilityUpdateJob(IUnitOfWork unitOfWork, ILogger<EligibilityUpdateJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            var members = await _unitOfWork.Members.GetAllAsync();
            var now = DateTime.UtcNow;

            foreach (var member in members)
            {
                var totalMonths = (await _unitOfWork.Contributions.FindAsync(c => c.MemberId == member.Id))
                    .Where(c => c.Type == Domain.Enums.ContributionType.Monthly)
                    .Select(c => new { c.ContributionDate.Year, c.ContributionDate.Month })
                    .Distinct()
                    .Count();

                if (totalMonths >= 12 && !member.IsEligibleForBenefit)
                {
                    member.IsEligibleForBenefit = true;
                    _unitOfWork.Members.Update(member);
                    _logger.LogInformation($"Member {member.FullName} is now eligible for benefit.");
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
