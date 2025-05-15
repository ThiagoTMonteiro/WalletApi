using AutoMapper;
using MockQueryable;
using Moq;
using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;
using WalletApi.Application.Interfaces.Repositories;
using WalletApi.Application.Services;
using WalletApi.Domain.Entities;
using Xunit;

namespace WalletApi.test.Service;

public class TransferServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ITransferRepository> _transferRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly TransferService _service;

    public TransferServiceTests()
    {
        _service = new TransferService(
            _userRepositoryMock.Object,
            _transferRepositoryMock.Object,
            _mapperMock.Object
            );
    }
    
    
    [Fact]
    public async Task Transfer_ShouldTransferAmount_WhenDataIsValid()
    {
        // Arrange
        const string fromUserId = "user1";
        const string toUserId = "user2";
        const decimal amount = 50m;

        var senderUser = new AppUser { Id = fromUserId, WalletBalance = 100m };
        var receiverUser = new AppUser { Id = toUserId, WalletBalance = 20m };

        var model = new TransferModel { ReceiverUserId = toUserId, Amount = amount };
        var expectedResponse = new TransferResponse { Amount = amount };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(fromUserId)).ReturnsAsync(senderUser);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(toUserId)).ReturnsAsync(receiverUser);

        _transferRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Transfer>())).Returns(Task.CompletedTask);
        _transferRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<TransferResponse>(It.IsAny<Transfer>())).Returns(expectedResponse);

        // Act
        var result = await _service.Transfer(model, fromUserId);

        // Assert
        Assert.Equal(amount, result.Amount);
        Assert.Equal(70m, receiverUser.WalletBalance);  
        Assert.Equal(50m, senderUser.WalletBalance); 

        _transferRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transfer>()), Times.Once);
        _transferRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Transfer_ShouldReturnEmptyResponse_WhenUserNotFound()
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.GetByIdAsync("user1")).ReturnsAsync((AppUser)null);

        var model = new TransferModel { ReceiverUserId = "user2", Amount = 10m };

        // Act
        var result = await _service.Transfer(model, "user1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(default(decimal), result.Amount);
        _transferRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transfer>()), Times.Never);
    }

    [Fact]
    public async Task Transfer_ShouldThrowException_WhenInsufficientBalance()
    {
        // Arrange
        var senderUser = new AppUser { Id = "user1", WalletBalance = 5m };
        var receiverUser = new AppUser { Id = "user2", WalletBalance = 0m };

        _userRepositoryMock.Setup(r => r.GetByIdAsync("user1")).ReturnsAsync(senderUser);
        _userRepositoryMock.Setup(r => r.GetByIdAsync("user2")).ReturnsAsync(receiverUser);

        var model = new TransferModel { ReceiverUserId = "user2", Amount = 10m };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.Transfer(model, "user1"));

        _transferRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transfer>()), Times.Never);
        _transferRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task GetTransfers_ShouldReturnTransfers_WhenTransfersExist()
    {
        // Arrange
        const string userId = "user123";
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 12, 31);

        var transfers = new List<Transfer>
        {
            new Transfer { SenderUserId = "user123", ReceiverUserId = "user456", TransferDate = new DateTime(2025, 6, 1) },
            new Transfer { SenderUserId = "user789", ReceiverUserId = "user123", TransferDate = new DateTime(2025, 7, 1) }
        };

        var mockQueryable = transfers.AsQueryable().BuildMock();
        _transferRepositoryMock.Setup(repo => repo.AsQueryable()).Returns(mockQueryable);
        
        _mapperMock.Setup(m => m.Map<List<TransferResponse>>(It.IsAny<List<Transfer>>())).Returns(new List<TransferResponse>
        {
            new TransferResponse { SenderUserId = "user123", ReceiverUserId = "user456", Date = new DateTime(2025, 6, 1) },
            new TransferResponse { SenderUserId = "user789", ReceiverUserId = "user123", Date = new DateTime(2025, 7, 1) }
        });

        // Act
        var result = await _service.GetTransfers(startDate, endDate, userId);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("user123", result[0].SenderUserId);
        Assert.Equal("user123", result[1].ReceiverUserId);
    }
    
    [Fact]
    public async Task GetTransfers_ShouldReturnEmptyList_WhenNoTransfersFound()
    {
        // Arrange
        const string userId = "user123";
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 12, 31);
        
        var mockQueryable = new List<Transfer>().AsQueryable().BuildMock();
        _transferRepositoryMock.Setup(repo => repo.AsQueryable()).Returns(mockQueryable);

        // Act
        var result = await _service.GetTransfers(startDate, endDate, userId);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetTransfers_ShouldReturnFilteredTransfers_WhenDateRangeIsSpecified()
    {
        // Arrange
        const string userId = "user123";
        var startDate = new DateTime(2025, 6, 1);
        var endDate = new DateTime(2025, 12, 31);

        var transfers = new List<Transfer>
        {
            new Transfer { SenderUserId = "user123", ReceiverUserId = "user456", TransferDate = new DateTime(2025, 6, 1) },
            new Transfer { SenderUserId = "user789", ReceiverUserId = "user123", TransferDate = new DateTime(2025, 7, 1) },
            new Transfer { SenderUserId = "user123", ReceiverUserId = "user456", TransferDate = new DateTime(2025, 1, 1) }  // This one is outside the date range
        };

        var mockQueryable = transfers.AsQueryable().BuildMock();
        _transferRepositoryMock.Setup(repo => repo.AsQueryable()).Returns(mockQueryable);

        _mapperMock.Setup(m => m.Map<List<TransferResponse>>(It.IsAny<List<Transfer>>())).Returns(new List<TransferResponse>
        {
            new TransferResponse { SenderUserId = "user123", ReceiverUserId = "user456", Date = new DateTime(2025, 6, 1) },
            new TransferResponse { SenderUserId = "user789", ReceiverUserId = "user123", Date = new DateTime(2025, 7, 1) }
        });

        // Act
        var result = await _service.GetTransfers(startDate, endDate, userId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, t => t.Date < startDate || t.Date > endDate);
    }
    
}