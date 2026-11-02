using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorldApp.Models
{
    public class Employee
    {
        public int IdEmployee { get; set; }
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";
        public int? IdShop { get; set; }
        public Shop? Shop { get; set; }
    }
}
