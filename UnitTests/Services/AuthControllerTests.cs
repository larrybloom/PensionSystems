using Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterEmployer_ShouldReturn200()
        {
            var payload = new EmployerCreateDto
            {
                CompanyName = "Test Co",
                RegistrationNumber = "RC123456"
            };

            var response = await _client.PostAsJsonAsync("/api/Auth/register", payload);
            response.EnsureSuccessStatusCode(); // 200 OK
        }
    }
}
