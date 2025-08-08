using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public Guid EmployerId { get; set; }
        public bool IsEligibleForBenefit { get; set; } = false;
        public Employer Employer { get; set; } = null!;

        public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();

        public int Age => (int)((DateTime.UtcNow - DateOfBirth).TotalDays / 365.25);

    }
}
