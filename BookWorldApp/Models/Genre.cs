using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Genre
    {
        public int IdGenre { get; set; }
        public string Title { get; set; } = "";
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
}
