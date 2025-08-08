using Application.DTOs;
using Application.Interfaces.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PensionSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmployerService _employerService;
        private readonly IJwtTokenGenerator _jwt;

        public AuthController(IEmployerService employerService, IJwtTokenGenerator jwt)
        {
            _employerService = employerService;
            _jwt = jwt;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterEmployer([FromBody] EmployerCreateDto dto)
        {
            var result = await _employerService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmployerCreateDto dto)
        {
            // This is a placeholder — real login should use hashed passwords
            var employers = await _employerService.GetAllAsync();
            var employer = employers.FirstOrDefault(e =>
                e.CompanyName == dto.CompanyName &&
                e.RegistrationNumber == dto.RegistrationNumber);

            if (employer == null) return Unauthorized("Invalid credentials");

            var token = _jwt.GenerateToken(employer.Id, "Employer");
            return Ok(new { Token = token });
        }
    }
}
