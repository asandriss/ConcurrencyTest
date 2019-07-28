using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderLedger ledger = new OrderLedger();
            var products = new List<string> { "shirt", "pants", "t-shirt", "belt", "hat", "jacket" };
            var salesmen = new List<Salesman>
            {
                new Salesman("George", ledger, products),
                new Salesman("Steve", ledger, products),
                new Salesman("Mike", ledger, products),
                new Salesman("Peter", ledger, products),
                new Salesman("Ann", ledger, products)
            };

            var shiftEnds = DateTime.Now.AddSeconds(2);

            Parallel.ForEach(salesmen, s => s.DoWork(shiftEnds));
            //salesmen[0].DoWork(shiftEnds);

            ledger.LedgerReport();
        }
    }
}
