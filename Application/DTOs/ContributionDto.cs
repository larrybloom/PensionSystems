using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ContributionDto
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public DateTime ContributionDate { get; set; }
        public decimal Amount { get; set; }
        public ContributionType Type { get; set; }
    }



    public class ContributionCreateDto
    {
        public Guid MemberId { get; set; }
        public DateTime ContributionDate { get; set; }
        public decimal Amount { get; set; }
        public ContributionType Type { get; set; }
    }
}
