using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class ChequeBook
    {
        public int IdChequeBook { get; set; }
        public int IdCheque { get; set; }
        public int IdBook { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public Cheque Cheque { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}
