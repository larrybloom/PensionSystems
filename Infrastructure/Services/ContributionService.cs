using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ContributionService : IContributionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContributionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ContributionDto> AddContributionAsync(ContributionCreateDto dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Contribution amount must be greater than 0");

            var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
            if (member == null) throw new Exception("Member not found");

            if (dto.Type == ContributionType.Monthly)
            {
                var existing = await _unitOfWork.Contributions.FindAsync(c =>
                    c.MemberId == dto.MemberId &&
                    c.Type == ContributionType.Monthly &&
                    c.ContributionDate.Month == dto.ContributionDate.Month &&
                    c.ContributionDate.Year == dto.ContributionDate.Year);

                if (existing.Any())
                    throw new Exception("Monthly contribution already exists for this month");
            }

            var contribution = new Contribution
            {
                MemberId = dto.MemberId,
                ContributionDate = dto.ContributionDate,
                Amount = dto.Amount,
                Type = dto.Type
            };

            await _unitOfWork.Contributions.AddAsync(contribution);
            await _unitOfWork.SaveChangesAsync();

            return new ContributionDto
            {
                Id = contribution.Id,
                MemberId = contribution.MemberId,
                ContributionDate = contribution.ContributionDate,
                Amount = contribution.Amount,
                Type = contribution.Type
            };
        }

        public async Task<IEnumerable<ContributionDto>> GetMemberContributionsAsync(Guid memberId)
        {
            var contributions = await _unitOfWork.Contributions.FindAsync(c => c.MemberId == memberId);
            return contributions.Select(c => new ContributionDto
            {
                Id = c.Id,
                MemberId = c.MemberId,
                Amount = c.Amount,
                ContributionDate = c.ContributionDate,
                Type = c.Type
            });
        }

        public async Task<decimal> GetTotalContributionsAsync(Guid memberId)
        {
            var contributions = await _unitOfWork.Contributions.FindAsync(c => c.MemberId == memberId);
            return contributions.Sum(c => c.Amount);
        }
    }
}
