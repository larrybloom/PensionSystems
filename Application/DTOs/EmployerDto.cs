using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EmployerDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public bool IsActive { get; set; }
    }


    public class EmployerCreateDto
    {
        public string CompanyName { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public DateTime? DateRegistered   { get; set; }
        public string? Address { get; set; }

    }

    public class EmployerUpdateDto : EmployerCreateDto { }
}
