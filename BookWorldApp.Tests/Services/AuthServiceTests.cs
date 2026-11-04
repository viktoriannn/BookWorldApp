using BookWorldApp.Data;
using BookWorldApp.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookWorldApp.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IDbContextFactory<BookStoreContext>> _factoryMock;
    private readonly UserSession _session;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        // Arrange
        _factoryMock = new Mock<IDbContextFactory<BookStoreContext>>();
        _session = new UserSession();
        _authService = new AuthService(_factoryMock.Object, _session);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "password")]
    [InlineData("login", "")]
    [InlineData("   ", "password")]
    public async Task Login_EmptyCredentials_ReturnsValidationError(
        string? login, string? password)
    {
        // Act
        var result = await _authService.LoginAsync(login, password);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthService.EmptyCredentials, result.Error);
        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public void SignOut_ClearsCurrentUser()
    {
        // Arrange
        _session.SignIn(
            new CurrentUser(1, "cashier1", Roles.Cashier, 1, "Москва"));

        // Act
        _session.SignOut();

        // Assert
        Assert.Null(_session.User);
        Assert.Throws<UnauthorizedAccessException>(
            () => _session.RequireCashier());
    }

    [Fact]
    public void Admin_CannotUseCashierOnlyOperations()
    {
        // Arrange
        _session.SignIn(
            new CurrentUser(2, "admin1", Roles.Admin, null, null));

        // Act and Assert
        Assert.Throws<UnauthorizedAccessException>(
            () => _session.RequireCashier());
    }
}