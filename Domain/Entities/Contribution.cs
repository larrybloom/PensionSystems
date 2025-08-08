using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Contribution
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public DateTime ContributionDate { get; set; }
        public decimal Amount { get; set; }
        public ContributionType Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
