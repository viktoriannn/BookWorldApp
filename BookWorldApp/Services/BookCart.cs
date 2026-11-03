using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookWorldApp.Services;

public abstract class NotifyBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void Raise([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public sealed class CartLine : NotifyBase
{
    public const string InvalidQuantityMessage = "Количество должно быть целым числом не меньше 1.";

    private int _quantity;

    public CartLine(int bookId, string title, decimal price, int quantity, int? maxQuantity)
    {
        BookId = bookId;
        Title = title;
        Price = price;
        _quantity = quantity;
        MaxQuantity = maxQuantity;
    }

    public int BookId { get; }
    public string Title { get; }
    public decimal Price { get; }
    public int? MaxQuantity { get; }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity == value) return;
            _quantity = value;
            Raise();
            Raise(nameof(Sum));
            Raise(nameof(Error));
        }
    }

    public decimal Sum => Price * Quantity;

    public string? Error =>
        Quantity < 1 ? InvalidQuantityMessage
        : MaxQuantity is int max && Quantity > max ? $"В магазине всего {max} экз."
        : null;
}

public sealed class BookCart : NotifyBase
{
    public ObservableCollection<CartLine> Lines { get; } = new();

    public BookCart() => Lines.CollectionChanged += OnLinesChanged;

    public bool IsEmpty => Lines.Count == 0;

    public decimal Total => Lines.Sum(line => line.Sum);

    public Result Add(int bookId, string title, decimal price, int quantity, int? stockLimit)
    {
        if (quantity < 1) return Result.Fail(CartLine.InvalidQuantityMessage);

        var existing = Lines.FirstOrDefault(line => line.BookId == bookId);
        var newQuantity = (existing?.Quantity ?? 0) + quantity;

        if (stockLimit is int limit && newQuantity > limit)
            return Result.Fail(existing is null
                ? $"В магазине всего {limit} экз. книги «{title}»."
                : $"В магазине всего {limit} экз. книги «{title}», в чеке уже {existing.Quantity}.");

        if (existing is null) Lines.Add(new CartLine(bookId, title, price, quantity, stockLimit));
        else existing.Quantity = newQuantity;
        return Result.Ok();
    }

    public bool Remove(int bookId)
    {
        var line = Lines.FirstOrDefault(item => item.BookId == bookId);
        return line is not null && Lines.Remove(line);
    }

    public void Clear()
    {
        foreach (var line in Lines) line.PropertyChanged -= OnLineChanged;
        Lines.Clear();
    }

    public string? FirstError()
    {
        var line = Lines.FirstOrDefault(item => item.Error is not null);
        return line is null ? null : $"«{line.Title}»: {line.Error}";
    }

    private void OnLinesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
            foreach (CartLine line in e.OldItems) line.PropertyChanged -= OnLineChanged;
        if (e.NewItems is not null)
            foreach (CartLine line in e.NewItems) line.PropertyChanged += OnLineChanged;
        Raise(nameof(Total));
        Raise(nameof(IsEmpty));
    }

    private void OnLineChanged(object? sender, PropertyChangedEventArgs e) => Raise(nameof(Total));
}
