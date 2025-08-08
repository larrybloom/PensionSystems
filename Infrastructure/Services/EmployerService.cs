using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployerDto> CreateAsync(EmployerCreateDto dto)
        {
            var employer = new Employer
            {
                CompanyName = dto.CompanyName,
                RegistrationNumber = dto.RegistrationNumber,
                IsActive = true,
                DateRegistered = DateTime.UtcNow

            };

            await _unitOfWork.Employers.AddAsync(employer);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(employer);
        }

        public async Task<IEnumerable<EmployerDto>> GetAllAsync()
        {
            var employers = await _unitOfWork.Employers.GetAllAsync();
            return employers.Select(MapToDto);
        }

        public async Task<EmployerDto?> GetByIdAsync(Guid id)
        {
            var employer = await _unitOfWork.Employers.GetByIdAsync(id);
            return employer == null ? null : MapToDto(employer);
        }

        public async Task<EmployerDto> UpdateAsync(Guid id, EmployerUpdateDto dto)
        {
            var employer = await _unitOfWork.Employers.GetByIdAsync(id);
            if (employer == null) throw new Exception("Employer not found");

            employer.CompanyName = dto.CompanyName;
            employer.RegistrationNumber = dto.RegistrationNumber;

            _unitOfWork.Employers.Update(employer);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(employer);
        }

        private static EmployerDto MapToDto(Employer e) => new()
        {
            Id = e.Id,
            CompanyName = e.CompanyName,
            RegistrationNumber = e.RegistrationNumber,
            IsActive = e.IsActive
        };
    }
}
