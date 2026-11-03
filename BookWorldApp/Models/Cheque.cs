using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Cheque
    {
        public int IdCheque { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalSum { get; set; }
        public int IdEmployee { get; set; }
        public int IdShop { get; set; }
        public Employee Employee { get; set; } = null!;
        public Shop Shop { get; set; } = null!;
        public ICollection<ChequeBook> ChequeBooks { get; set; } = new List<ChequeBook>();
    }
}
