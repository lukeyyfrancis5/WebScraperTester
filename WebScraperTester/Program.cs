using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data.Entity;

// Todo Finish building WebScaperTester
// Todo Classes are needed- *done* 
// Todo Fix variable definitions- *done*
// Todo Scrape more data for "main application"
// Todo- make sure you understand how to correctly scrape specific data
// Todo- make sure you use xchro
// Todo- make sure you understand what all methods are doing when building browser
//      

namespace WebScraperTester
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadLine();
        }

        private static async Task GetHtmlAsync()
        {
            using (var db = new LedgerContext())
            {
                var url = "https://coinmarketcap.com/";

                var htmlWeb = new HtmlWeb();

                var htmlDocument = new HtmlDocument();
                htmlDocument = htmlWeb.Load(url);

                HtmlNodeCollection allRows = htmlDocument.DocumentNode.SelectNodes("//table[1]/tbody[1]/tr[*]");
                var rowNumber = 0;
                List<Coin> currencyDataList = new List<Coin>();

                foreach (var row in allRows)
                {
                    try
                    {
                        Console.WriteLine("Attempting to process row: " + rowNumber++);

                        var CoinPosition = row.ChildNodes[1].InnerText;
                        /*var CoinSymbol = row.ChildNodes[3].ChildNodes[3].InnerText;
                        var CoinName = row.ChildNodes[3].ChildNodes[5].InnerText;
                        var CoinPrice = row.ChildNodes[7].InnerText; */

                        Coin coin = new Coin(CoinPosition /*CoinSymbol, CoinName, CoinPrice*/);
                        currencyDataList.Add(coin);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to find value at specified coordinates");
                        Console.WriteLine(e);
                        throw;
                    }

                    Console.WriteLine("Process Done !");
                }

                Console.WriteLine(currencyDataList);

                //db.Ledgers.Add(Ledger);
                //db.SaveChanges();
            }
        }

        public class Ledger
        {
            public int LedgerId { get; set; }

            public virtual List<Coin> Coins { get; set; }
        }

        public class LedgerContext : DbContext
        {
            public DbSet<Ledger> Ledgers { get; set; }
            public DbSet <Coin> Coins { get; set; }
        }   

        public class Coin
        {       
            /*public string CoinSymbol { get; set; }                                
            public string CoinName { get; set; }
            public double CoinPrice { get; set; } */
            public string CoinPosition { get; set; }    
            public int LedgerId { get; set; }
            public virtual Ledger Ledger { get; set; }


           public Coin(string CoinPosition/*string coinSymbol, string coinName, string coinPrice*/)
           {
               this.CoinPosition = CoinPosition;
               /*this.CoinSymbol = CoinSymbol;
               this.CoinName = CoinName;
               this.CoinPrice = double.Parse(coinPrice, NumberStyles.Currency);
               */
           }
        }
    }
}
