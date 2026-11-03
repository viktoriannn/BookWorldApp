using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Shop
    {
        public int IdShop { get; set; }
        public string Title { get; set; } = "";
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string House { get; set; } = "";
        public string? Flat { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Remains> Remains { get; set; } = new List<Remains>();
    }
}
