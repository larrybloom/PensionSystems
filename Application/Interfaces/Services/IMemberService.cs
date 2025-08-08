using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllAsync();
        Task<MemberDto?> GetByIdAsync(Guid id);
        Task<MemberDto> CreateAsync(MemberCreateDto member);
        Task<MemberDto> UpdateAsync(Guid id, MemberUpdateDto member);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
