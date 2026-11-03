using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class OrderBook
    {
        public int IdOrderBook { get; set; }
        public int IdOrder { get; set; }
        public int IdBook { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}
