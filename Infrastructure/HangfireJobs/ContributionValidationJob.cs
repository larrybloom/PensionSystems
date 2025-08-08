using Domain.Enums;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HangfireJobs
{
    public class ContributionValidationJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ContributionValidationJob> _logger;

        public ContributionValidationJob(IUnitOfWork unitOfWork, ILogger<ContributionValidationJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            var members = await _unitOfWork.Members.GetAllAsync();
            var today = DateTime.UtcNow;

            foreach (var member in members)
            {
                var hasPaidThisMonth = (await _unitOfWork.Contributions.FindAsync(c =>
                    c.MemberId == member.Id &&
                    c.Type == ContributionType.Monthly &&
                    c.ContributionDate.Month == today.Month &&
                    c.ContributionDate.Year == today.Year)).Any();

                if (!hasPaidThisMonth)
                {
                    _logger.LogWarning($"Member {member.FullName} has not made a monthly contribution for {today:MMMM yyyy}");
                }
            }
        }
    }
}
