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
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MemberDto> CreateAsync(MemberCreateDto dto)
        {
            var member = new Member
            {
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                EmployerId = dto.EmployerId
            };

            if (member.Age < 18 || member.Age > 70)
                throw new Exception("Member age must be between 18 and 70.");

            await _unitOfWork.Members.AddAsync(member);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(member);
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member == null) return false;

            member.IsDeleted = true;
            _unitOfWork.Members.Update(member);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<MemberDto>> GetAllAsync()
        {
            var members = await _unitOfWork.Members.GetAllAsync();
            return members.Select(MapToDto);
        }

        public async Task<MemberDto?> GetByIdAsync(Guid id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            return member == null ? null : MapToDto(member);
        }

        public async Task<MemberDto> UpdateAsync(Guid id, MemberUpdateDto dto)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);
            if (member == null) throw new Exception("Member not found");

            member.FullName = dto.FullName;
            member.DateOfBirth = dto.DateOfBirth;
            member.Email = dto.Email;
            member.PhoneNumber = dto.PhoneNumber;
            member.Address = dto.Address;

            _unitOfWork.Members.Update(member);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(member);
        }

        private static MemberDto MapToDto(Member member) => new()
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            Address = member.Address,
            Age = member.Age,
            EmployerId = member.EmployerId
        };
    }
}
