using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PensionSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContributionController : ControllerBase
    {
        private readonly IContributionService _contributionService;

        public ContributionController(IContributionService contributionService)
        {
            _contributionService = contributionService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ContributionCreateDto dto)
        {
            var result = await _contributionService.AddContributionAsync(dto);
            return Ok(result);
        }

        [HttpGet("member/{memberId}")]
        public async Task<IActionResult> GetForMember(Guid memberId)
        {
            var result = await _contributionService.GetMemberContributionsAsync(memberId);
            return Ok(result);
        }

        [HttpGet("total/{memberId}")]
        public async Task<IActionResult> GetTotal(Guid memberId)
        {
            var total = await _contributionService.GetTotalContributionsAsync(memberId);
            return Ok(new { Total = total });
        }
    }
}
