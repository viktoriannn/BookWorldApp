using BookWorldApp.Data;
using BookWorldApp.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookWorldApp.Tests.Services;

public class ReplenishmentServiceTests
{
    private readonly Mock<IDbContextFactory<BookStoreContext>> _factoryMock;
    private readonly UserSession _session;
    private readonly ReplenishmentService _service;

    public ReplenishmentServiceTests()
    {
        // Arrange
        _factoryMock = new Mock<IDbContextFactory<BookStoreContext>>();
        _session = new UserSession();

        _session.SignIn(
            new CurrentUser(1, "cashier1", Roles.Cashier, 1, "Магазин 1"));

        _service = new ReplenishmentService(_factoryMock.Object, _session);
    }

    [Fact]
    public async Task Create_EmptyCart_ReturnsErrorWithoutDatabaseOperation()
    {
        // Arrange
        var cart = new BookCart();

        // Act
        var result = await _service.CreateAsync(cart);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Добавьте в заказ", result.Error);

        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_InvalidCartQuantity_ReturnsErrorWithoutDatabaseOperation()
    {
        // Arrange
        var cart = new BookCart();
        cart.Add(1, "1984", 610m, 1, 10);
        cart.Lines[0].Quantity = 0;

        // Act
        var result = await _service.CreateAsync(cart);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_UnauthorizedUser_ThrowsException()
    {
        // Arrange
        var session = new UserSession();
        var service = new ReplenishmentService(_factoryMock.Object, session);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => service.CreateAsync(new BookCart()));

        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_AdminUser_ThrowsException()
    {
        // Arrange
        var session = new UserSession();
        session.SignIn(
            new CurrentUser(2, "admin1", Roles.Admin, null, null));

        var service = new ReplenishmentService(_factoryMock.Object, session);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => service.CreateAsync(new BookCart()));

        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}