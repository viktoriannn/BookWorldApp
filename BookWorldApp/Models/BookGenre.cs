using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class BookGenre
    {
        public int IdBookGenre { get; set; }
        public int IdBook { get; set; }
        public int IdGenre { get; set; }
        public Book Book { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}

