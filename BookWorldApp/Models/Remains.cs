using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Remains
    {
        public int IdRemains { get; set; }
        public int NumberOfCopies { get; set; }
        public int IdBook { get; set; }
        public int IdShop { get; set; }
        public Book Book { get; set; } = null!;
        public Shop Shop { get; set; } = null!;
    }
}
