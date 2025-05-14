using AutoMapper;
using Moq;
using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;
using WalletApi.Application.Interfaces.Repositories;
using WalletApi.Application.Services;
using WalletApi.Domain.Entities;
using Xunit;

namespace WalletApi.test.Service;

public class WalletServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly WalletService _service;

    public WalletServiceTests()
    {
        _service = new WalletService(
            _userRepositoryMock.Object,
            _mapperMock.Object
            );
    }
    
    [Fact]
    public async Task GetBalanceAsync_ReturnsMappedUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new AppUser { Id = userId.ToString(), WalletBalance = 100 };
        var expectedResponse = new AppUserResponse { Id = userId, WalletBalance = 100 };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId.ToString()))
            .ReturnsAsync(userEntity);
        _mapperMock.Setup(m => m.Map<AppUserResponse>(userEntity))
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBalanceAsync(userId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.WalletBalance, result.WalletBalance);
    }

    [Fact]
    public async Task GetBalanceAsync_ReturnsEmptyResponse_WhenUserNotFound()
    {
        // Arrange
        const string userId = "nonexistent";
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((AppUser)null);

        // Act
        var result = await _service.GetBalanceAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AppUserResponse>(result);
        Assert.Equal(0, result.WalletBalance);
    }
    
    [Fact]
    public async Task AddBalanceAsync_ShouldAddAmountAndReturnMappedResponse_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var model = new WalletModel { Amount = 50m };
        var user = new AppUser { Id = userId.ToString(), WalletBalance = 100m };

        var expectedResponse = new WalletResponse
        {
            UserId = Guid.NewGuid(),
            Amount = 150m
        };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId.ToString())).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<WalletResponse>(user)).Returns(expectedResponse);

        // Act
        var result = await _service.AddBalanceAsync(userId.ToString(), model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.UserId, result.UserId);
        Assert.Equal(expectedResponse.Amount, result.Amount);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddBalanceAsync_ShouldReturnEmptyResponse_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var model = new WalletModel { Amount = 50m };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId.ToString())).ReturnsAsync((AppUser)null);

        // Act
        var result = await _service.AddBalanceAsync(userId.ToString(), model);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<WalletResponse>(result);
        Assert.Equal(0, result.Amount);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
  
}