using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class BookAuthor
    {
        public int IdBookAuthor { get; set; }
        public int IdBook { get; set; }
        public int IdAuthor { get; set; }
        public Book Book { get; set; } = null!;
        public Author Author { get; set; } = null!;
    }
}
