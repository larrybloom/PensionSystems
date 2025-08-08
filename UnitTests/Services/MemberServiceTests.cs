using Moq;
using Xunit;
using System;
using Infrastructure.Data;
using Infrastructure.Services;
using Application.DTOs;
using Domain.Entities;


namespace UnitTests;

public class MemberServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly MemberService _service;

    public MemberServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _service = new MemberService(_mockUow.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedMember()
    {
        // Arrange
        var dto = new MemberCreateDto
        {
            FullName = "Jane Doe",
            DateOfBirth = DateTime.Today.AddYears(-30),
            Email = "jane@example.com",
            PhoneNumber = "1234567890",
            Gender = "Female",
            MaritalStatus = "Single",
            EmployerId = Guid.NewGuid()
        };

        _mockUow.Setup(u => u.Members.AddAsync(It.IsAny<Member>()))
            .ReturnsAsync((Member m) => m);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.Equal(dto.FullName, result.FullName);
        _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
