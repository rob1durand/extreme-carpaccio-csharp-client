using System;
using System.Collections.Generic;

namespace xCarpaccio.client
{
    class Bill
    {
        public decimal total { get; set; }

        public void CalculMttTotal(Bill bill, Order order)
        {
            for (var i = 0; i < order.Prices.Length; i++)
            {
                bill.total += (order.Prices[i] * order.Quantities[i]);
            }
            Console.WriteLine("Total : " + bill.total);
        }

        public void AddTva(Bill bill, Order order)
        {
            var taxesDictionary = new Dictionary<string, int>
            {
                { "DE", 20 },
                { "UK", 21 },
                { "FR", 20 },
                { "IT", 25 },
                { "ES", 19 },
                { "PL", 21 },
                { "RO", 20 },
                { "NL", 20 },
                { "BE", 24 },
                { "EL", 20 },
                { "CZ", 19 },
                { "PT", 23 },
                { "HU", 27 },
                { "SE", 23 },
                { "AT", 22 },
                { "BG", 21 },
                { "DK", 21 },
                { "FI", 17 },
                { "SK", 18 },
                { "IE", 21 },
                { "HR", 23 },
                { "LT", 23 },
                { "SI", 24 },
                { "LV", 20 },
                { "EE", 22 },
                { "CY", 21 },
                { "LU", 25 },
                { "MT", 20 }
            };

            var tva = 0;
            if (taxesDictionary.ContainsKey(order.Country))
            {
                tva = taxesDictionary[order.Country]; //get the tva for the country
            }
            bill.total += bill.total * tva / 100; //apply the tva

            Console.WriteLine("Avec tva : " + bill.total);
        }

        public void SubstractReduce(Bill bill, Order order)
        {
            //apply the reduces
            if (order.Reduction == "STANDARD")
            {
                if (bill.total >= 50000)
                {
                    bill.total -= bill.total * 0.15m;
                }
                else if (bill.total >= 10000)
                {
                    bill.total -= bill.total * 0.1m;
                }
                else if (bill.total >= 7000)
                {
                    bill.total -= bill.total * 0.07m;
                }
                else if (bill.total >= 5000)
                {
                    bill.total -= bill.total * 0.05m;
                }
                else if (bill.total >= 1000)
                {
                    bill.total -= bill.total * 0.03m;
                }
            }

            Console.WriteLine("Avec reduction : " + bill.total.ToString());
        }
    }
}
