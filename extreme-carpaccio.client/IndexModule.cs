using System.Collections.Generic;
using System.IO;
using System.Text;

namespace xCarpaccio.client
{
    using Nancy;
    using System;
    using Nancy.ModelBinding;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => "It works !!! You need to register your server on main server.";

            Post["/order"] = _ =>
            {
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    Console.WriteLine("Order received: {0}", reader.ReadToEnd());
                }

                var order = this.Bind<Order>();
                Bill bill = new Bill { total = 0 };
                //TODO: do something with order and return a bill if possible
                try
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
                    
                    bill = CalculMttTotArt(bill, order);

                    AddTva(taxesDictionary, order, bill);
                    
                    SubstractReduce(order, bill);
                    
                    // If you manage to get the result, return a Bill object (JSON serialization is done automagically)
                    // Else return a HTTP 404 error : return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                    
                    return bill;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                    throw;
                }
            };

            Post["/feedback"] = _ =>
            {
                var feedback = this.Bind<Feedback>();
                Console.Write("Type: {0}: ", feedback.Type);
                Console.WriteLine(feedback.Content);
                return Negotiate.WithStatusCode(HttpStatusCode.OK);
            };
        }

        private static Bill CalculMttTotArt(Bill bill, Order order)
        {
            for (var i = 0; i < order.Prices.Length; i++)
            {
                bill.total += (order.Prices[i]*order.Quantities[i]);
            }
            Console.WriteLine("Total : " + bill.total.ToString());
            return bill;
        }

        private static void AddTva(Dictionary<string, int> taxesDictionary, Order order, Bill bill)
        {
            var tva = 0;
            if (taxesDictionary.ContainsKey(order.Country))
            {
                tva = taxesDictionary[order.Country]; //get the tva for the country
            }
            bill.total += bill.total * tva / 100; //apply the tva

            Console.WriteLine("Avec tva : " + bill.total.ToString());
        }

        private static void SubstractReduce(Order order, Bill bill)
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