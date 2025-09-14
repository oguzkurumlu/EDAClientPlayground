using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerDemo
{
    public class MoneyTransfer
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
