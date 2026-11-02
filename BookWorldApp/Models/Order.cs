using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Order
    {
        public int IdOrder { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public int IdShop { get; set; }
        public int IdEmployee { get; set; }
        public Shop Shop { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
    }
}
