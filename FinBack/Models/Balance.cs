using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinBack.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public int Amount { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
