using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Translator
    {
        public int IdTranslator { get; set; }
        public string Surname { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Patronymic { get; set; }
        public ICollection<BookTranslator> BookTranslators { get; set; } = new List<BookTranslator>();
    }
}
