using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IEmployerService
    {
        Task<IEnumerable<EmployerDto>> GetAllAsync();
        Task<EmployerDto?> GetByIdAsync(Guid id);
        Task<EmployerDto> CreateAsync(EmployerCreateDto employer);
        Task<EmployerDto> UpdateAsync(Guid id, EmployerUpdateDto employer);
    }
}
