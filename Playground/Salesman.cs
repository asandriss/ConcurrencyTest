using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Playground
{
    class Salesman
    {
        public string Name { get; private set; }
        private readonly IList<string> _products;
        private OrderLedger _ledger;

        public Salesman(string name, OrderLedger ledger, IList<string> products)
        {
            Name = name;
            _ledger = ledger;
            _products = products;
        }

        public void DoWork(DateTime shiftEnd)
        {
            var random = new Random(Name.GetHashCode());

            while (DateTime.Now <= shiftEnd)
            {
                Thread.Sleep(random.Next(100));
                var action = random.Next(5);

                var randomProduct = _products[random.Next(_products.Count - 1)];
                if (action == 0)
                {
                    // buy a random amount (between 1-10) number of products
                    var boughtAmount = random.Next(9) + 1;

                    _ledger.BuyProduct(randomProduct, boughtAmount);
                    Console.WriteLine($"{Name} bought {boughtAmount} pieces of {randomProduct}.");
                }
                else
                {
                    // sell one product
                    var success = _ledger.SellProduct(randomProduct);

                    if (success)
                        Console.WriteLine($"{Name} sold a {randomProduct}.");
                    else
                        Console.WriteLine($"{Name} couldn't sell {randomProduct} - out of stock.");
                }
            }
        }
    }
}
