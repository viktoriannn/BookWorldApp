using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Book
    {
        public int IdBook { get; set; }
        public string Title { get; set; } = "";
        public string Isbn { get; set; } = "";
        public decimal Price { get; set; }
        public int AgeLimit { get; set; }
        public int IdPublishingHouse { get; set; }
        public int? IdSeries { get; set; }
        public PublishingHouse PublishingHouse { get; set; } = null!;
        public Series? Series { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        public ICollection<BookTranslator> BookTranslators { get; set; } = new List<BookTranslator>();
        public ICollection<Remains> Remains { get; set; } = new List<Remains>();
    }
}
