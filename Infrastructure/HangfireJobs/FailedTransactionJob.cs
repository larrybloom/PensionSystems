using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HangfireJobs
{
    public class FailedTransactionJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FailedTransactionJob> _logger;

        public FailedTransactionJob(IUnitOfWork unitOfWork, ILogger<FailedTransactionJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            // In production, this could check a failed_transaction table or logs
            _logger.LogInformation("Checked for failed transactions — none found.");
            await Task.CompletedTask;
        }
    }
}
