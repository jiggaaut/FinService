using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinBack.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Balance> Balances { get; set; }
        public Client()
        {
            Balances = new List<Balance>();
        }
    }
}
