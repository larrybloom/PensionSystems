using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CompanyName { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime DateRegistered { get; set; }
        public string Address { get; set; }

        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}
