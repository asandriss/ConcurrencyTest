using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Playground
{
    class OrderLedger
    {
        private ConcurrentDictionary<string, int> _ledger = new ConcurrentDictionary<string, int>();
        int productsBought;
        int productsSold;

        public void BuyProduct(string productName, int qunatity)
        {
            var total = _ledger.AddOrUpdate(productName, qunatity, (key, oldValue) => oldValue += qunatity);
            Interlocked.Add(ref productsBought, qunatity);

            //if (_ledger.ContainsKey(productName))
            //    _ledger[productName] += qunatity;
            //else
            //    _ledger[productName] = qunatity;

            //productsBought+=qunatity;
            //return _ledger[productName];
        }

        public bool SellProduct(string productName)
        {
            bool success = false;
            _ledger.AddOrUpdate(productName,
                (key) => { success = false;  return 0; },
                (key, oldvalue) =>
                {
                    if (oldvalue > 0)
                    {
                        success = true;
                        return oldvalue--;
                    }
                    else
                    {
                        success = false;
                        return oldvalue;
                    }
                });

            if(success)
                Interlocked.Increment(ref productsSold);

            return success;
            //int qunatity = 0;
            //var success = _ledger.TryGetValue(productName, out qunatity);

            //if (qunatity > 0)
            //{
            //    _ledger[productName]--;
            //    productsSold++;
            //    return true;
            //}
            //return false;
        }

        public void LedgerReport()
        {
            int sum = _ledger.Values.Sum();

            Console.WriteLine("LEDGER REPORT");
            Console.WriteLine();
            Console.WriteLine("Current stock:");

            foreach (var product in _ledger)
            {
                Console.WriteLine($"{product.Key}: {product.Value}");
            }

            Console.WriteLine("Sales numbers:");
            Console.WriteLine($"Total number of bought items: {productsBought}");
            Console.WriteLine($"Total number of sold items: {productsSold}");

            if (productsBought - productsSold == sum)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Total numbers match.");
                Console.ForegroundColor = oldColor;
                
            }
            else
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Total number don't match");
                Console.ForegroundColor = oldColor;
            }
        }
    }
}
