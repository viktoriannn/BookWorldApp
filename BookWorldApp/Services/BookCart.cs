using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookWorldApp.Services;

public class NotifyBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void Raise([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

public class CartLine : NotifyBase
{
    public const string InvalidQuantityMessage =
        "Количество должно быть целым числом не меньше 1.";

    private int _quantity;

    public CartLine(int bookId, string title, decimal price, int quantity, int? maxQuantity)
    {
        BookId = bookId;
        Title = title;
        Price = price;
        MaxQuantity = maxQuantity;
        _quantity = quantity;
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

    public string? Error
    {
        get
        {
            if (Quantity < 1) return InvalidQuantityMessage;
            if (MaxQuantity.HasValue && Quantity > MaxQuantity.Value)
                return $"В магазине всего {MaxQuantity} экз.";

            return null;
        }
    }
}

public class BookCart : NotifyBase
{
    public ObservableCollection<CartLine> Lines { get; } = new();

    public BookCart()
    {
        Lines.CollectionChanged += OnLinesChanged;
    }

    public bool IsEmpty => Lines.Count == 0;
    public decimal Total => Lines.Sum(x => x.Sum);

    public Result Add(int bookId, string title, decimal price, int quantity, int? stockLimit)
    {
        if (quantity < 1)
            return Result.Fail(CartLine.InvalidQuantityMessage);

        var line = Lines.FirstOrDefault(x => x.BookId == bookId);
        int newQuantity = (line?.Quantity ?? 0) + quantity;

        if (stockLimit.HasValue && newQuantity > stockLimit.Value)
            return Result.Fail($"В магазине всего {stockLimit} экз. книги «{title}».");

        if (line == null)
            Lines.Add(new CartLine(bookId, title, price, quantity, stockLimit));
        else
            line.Quantity = newQuantity;

        return Result.Ok();
    }

    public bool Remove(int bookId)
    {
        var line = Lines.FirstOrDefault(x => x.BookId == bookId);
        return line != null && Lines.Remove(line);
    }

    public void Clear()
    {
        Lines.Clear();
    }

    public string? FirstError()
    {
        var line = Lines.FirstOrDefault(x => x.Error != null);
        return line == null ? null : $"«{line.Title}»: {line.Error}";
    }

    private void OnLinesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
            foreach (CartLine line in e.OldItems)
                line.PropertyChanged -= OnLineChanged;

        if (e.NewItems != null)
            foreach (CartLine line in e.NewItems)
                line.PropertyChanged += OnLineChanged;

        Raise(nameof(Total));
        Raise(nameof(IsEmpty));
    }

    private void OnLineChanged(object? sender, PropertyChangedEventArgs e)
    {
        Raise(nameof(Total));
    }
}