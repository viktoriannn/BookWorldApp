using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Series
    {
        public int IdSeries { get; set; }
        public string Title { get; set; } = "";
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
