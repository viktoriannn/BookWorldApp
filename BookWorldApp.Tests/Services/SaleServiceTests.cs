using BookWorldApp.Data;
using BookWorldApp.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookWorldApp.Tests.Services;

public class SaleServiceTests
{
    private readonly Mock<IDbContextFactory<BookStoreContext>> _factoryMock;
    private readonly UserSession _session;
    private readonly SaleService _saleService;

    public SaleServiceTests()
    {
        // Arrange
        _factoryMock = new Mock<IDbContextFactory<BookStoreContext>>();
        _session = new UserSession();

        _session.SignIn(
            new CurrentUser(1, "cashier1", Roles.Cashier, 1, "Магазин 1"));

        _saleService = new SaleService(_factoryMock.Object, _session);
    }

    [Fact]
    public async Task Confirm_EmptyCart_ReturnsErrorWithoutDatabaseOperation()
    {
        // Arrange
        var cart = new BookCart();

        // Act
        var result = await _saleService.ConfirmAsync(cart);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Чек пуст", result.Error);

        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Confirm_InvalidCartQuantity_ReturnsErrorWithoutDatabaseOperation()
    {
        // Arrange
        var cart = new BookCart();
        cart.Add(1, "1984", 610m, 1, 10);
        cart.Lines[0].Quantity = 0;

        // Act
        var result = await _saleService.ConfirmAsync(cart);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Confirm_UnauthorizedUser_ThrowsException()
    {
        // Arrange
        var session = new UserSession();
        var service = new SaleService(_factoryMock.Object, session);
        var cart = new BookCart();

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => service.ConfirmAsync(cart));

        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Confirm_AdminUser_ThrowsException()
    {
        // Arrange
        var session = new UserSession();
        session.SignIn(
            new CurrentUser(2, "admin1", Roles.Admin, null, null));

        var service = new SaleService(_factoryMock.Object, session);

        // Act and Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => service.ConfirmAsync(new BookCart()));

        _factoryMock.Verify(
            f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}