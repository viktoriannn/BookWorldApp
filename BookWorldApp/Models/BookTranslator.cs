using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class BookTranslator
    {
        public int IdBookTranslator { get; set; }
        public int IdBook { get; set; }
        public int IdTranslator { get; set; }
        public Book Book { get; set; } = null!;
        public Translator Translator { get; set; } = null!;
    }
}
