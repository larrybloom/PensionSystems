using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Services;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.Services
{
    public class ContributionServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly ContributionService _service;

        public ContributionServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _service = new ContributionService(_mockUow.Object);
        }

        [Fact]
        public async Task GetTotalContributionsAsync_ShouldSumCorrectly()
        {
            var memberId = Guid.NewGuid();
            var contributions = new List<Contribution>
        {
            new() { MemberId = memberId, Amount = 100 },
            new() { MemberId = memberId, Amount = 200 },
            new() { MemberId = memberId, Amount = 300 }
        };

            _mockUow.Setup(u => u.Contributions.FindAsync(It.IsAny<Expression<Func<Contribution, bool>>>()))
                .ReturnsAsync(contributions);


            var total = await _service.GetTotalContributionsAsync(memberId);

            Assert.Equal(600, total);
        }
    }
}
