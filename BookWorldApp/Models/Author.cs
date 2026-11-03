using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Author
    {
        public int IdAuthor { get; set; }
        public string Surname { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Patronymic { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
