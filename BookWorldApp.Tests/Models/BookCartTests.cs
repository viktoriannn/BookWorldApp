using BookWorldApp.Services;

namespace BookWorldApp.Tests.Models;

public class BookCartTests
{
    [Fact]
    public void Add_ValidBook_AddsPosition()
    {
        // Arrange
        var cart = new BookCart();

        // Act
        var result = cart.Add(1, "1984", 610m, 2, 10);

        // Assert
        Assert.True(result.IsSuccess);
        var line = Assert.Single(cart.Lines);
        Assert.Equal(2, line.Quantity);
        Assert.Equal(1220m, line.Sum);
    }

    [Fact]
    public void Add_MultipleBooks_CalculatesTotal()
    {
        // Arrange
        var cart = new BookCart();

        // Act
        cart.Add(1, "1984", 610m, 2, 10);
        cart.Add(2, "Война и мир", 450m, 1, 10);

        // Assert
        Assert.Equal(1670m, cart.Total);
    }

    [Fact]
    public void Add_ExistingBook_IncreasesQuantity()
    {
        // Arrange
        var cart = new BookCart();
        cart.Add(1, "1984", 610m, 2, 10);

        // Act
        var result = cart.Add(1, "1984", 610m, 3, 10);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(cart.Lines);
        Assert.Equal(5, cart.Lines[0].Quantity);
        Assert.Equal(3050m, cart.Total);
    }

    [Fact]
    public void ChangeQuantity_RecalculatesTotal()
    {
        // Arrange
        var cart = new BookCart();
        cart.Add(1, "1984", 610m, 2, 10);

        // Act
        cart.Lines[0].Quantity = 3;

        // Assert
        Assert.Equal(1830m, cart.Total);
    }

    [Fact]
    public void Remove_RemovesPositionAndRecalculatesTotal()
    {
        // Arrange
        var cart = new BookCart();
        cart.Add(1, "1984", 610m, 2, 10);
        cart.Add(2, "Война и мир", 450m, 1, 10);

        // Act
        var removed = cart.Remove(1);

        // Assert
        Assert.True(removed);
        Assert.Single(cart.Lines);
        Assert.Equal(450m, cart.Total);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Add_InvalidQuantity_ReturnsError(int quantity)
    {
        // Arrange
        var cart = new BookCart();

        // Act
        var result = cart.Add(1, "1984", 610m, quantity, 10);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Empty(cart.Lines);
    }

    [Fact]
    public void Add_QuantityExceedsStock_ReturnsError()
    {
        // Arrange
        var cart = new BookCart();

        // Act
        var result = cart.Add(1, "1984", 610m, 15, 10);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Empty(cart.Lines);
    }

    [Fact]
    public void Clear_RemovesAllPositionsAndResetsTotal()
    {
        // Arrange
        var cart = new BookCart();
        cart.Add(1, "1984", 610m, 2, 10);
        cart.Add(2, "Война и мир", 450m, 1, 10);

        // Act
        cart.Clear();

        // Assert
        Assert.True(cart.IsEmpty);
        Assert.Equal(0m, cart.Total);
    }

    [Fact]
    public void FirstError_DetectsInvalidQuantityChangedAfterAdding()
    {
        // Arrange
        var cart = new BookCart();
        cart.Add(1, "1984", 610m, 2, 10);
        cart.Lines[0].Quantity = 0;

        // Act
        var error = cart.FirstError();

        // Assert
        Assert.NotNull(error);
    }

    [Fact]
    public void Remove_NonExistingBook_ReturnsFalse()
    {
        // Arrange
        var cart = new BookCart();
        cart.Add(1, "1984", 610m, 2, 10);

        // Act
        var result = cart.Remove(999);

        // Assert
        Assert.False(result);
        Assert.Single(cart.Lines);
    }
}