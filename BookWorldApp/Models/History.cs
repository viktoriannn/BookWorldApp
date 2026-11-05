namespace BookWorldApp.Models;

public class History
{
    public int IdHistory { get; set; }
    public DateTime Date { get; set; }
    public string OperationType { get; set; } = string.Empty; 
    public int Quantity { get; set; }
    public int? IdShopSource { get; set; } 
    public int IdShop { get; set; }
    public int IdBook { get; set; }
    public int IdEmployee { get; set; }

    public Shop? ShopSource { get; set; }
    public Shop Shop { get; set; } = null!;
    public Book Book { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
}