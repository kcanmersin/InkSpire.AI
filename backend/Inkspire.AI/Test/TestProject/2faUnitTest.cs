using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Core.Service.JWT;
using Core.Shared;
using Core.Data.Entity.User;
using Core.Features.UserFeatures.Verify2FA;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit.Abstractions;

public class Verify2FAHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly Mock<IJwtService> _mockJwtService;
    private readonly Mock<ILogger<Verify2FAHandler>> _mockLogger;

    private readonly Verify2FAHandler _handler;

    private readonly ITestOutputHelper _output;

    public Verify2FAHandlerTests(ITestOutputHelper output)
    {
        _mockUserManager = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        _mockJwtService = new Mock<IJwtService>();
        _mockLogger = new Mock<ILogger<Verify2FAHandler>>();
        _output = output;

        _handler = new Verify2FAHandler(_mockUserManager.Object, _mockJwtService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var request = new Verify2FACommand { UserId = Guid.NewGuid(), TwoFactorCode = "123456" };
        _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);

        _output.WriteLine("Testing Handle with non-existent user.");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        
        _output.WriteLine("Result: " + result.IsFailure);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("InvalidUser", result.Error.Code);
    }



    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCodeIsInvalidOrExpired()
    {
        // Arrange
        var user = new AppUser { Id = Guid.NewGuid(), TwoFactorEnabled = true, TwoFactorCode = "654321", TwoFactorExpiryTime = DateTime.UtcNow.AddMinutes(-1) };
        var request = new Verify2FACommand { UserId = user.Id, TwoFactorCode = "123456" };
        _mockUserManager.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);

        _output.WriteLine("Testing Handle with an invalid or expired Two-Factor Authentication code.");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("InvalidCode", result.Error.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCodeIsValid()
    {
        // Arrange
        var user = new AppUser { Id = Guid.NewGuid(), Email = "test@example.com", TwoFactorEnabled = true, TwoFactorCode = "123456", TwoFactorExpiryTime = DateTime.UtcNow.AddMinutes(5) };
        var request = new Verify2FACommand { UserId = user.Id, TwoFactorCode = "123456" };
        var token = "jwt_token";

        _mockUserManager.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
        _mockUserManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        _mockJwtService.Setup(x => x.GenerateToken(user.Email, user.Id.ToString(), It.IsAny<IList<string>>())).Returns(token);

        _output.WriteLine("Testing Handle with a valid Two-Factor Authentication code.");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(token, result.Value);
    }
}