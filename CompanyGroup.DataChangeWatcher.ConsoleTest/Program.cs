using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.DataChangeWatcher.ConsoleTest
{
    class Program
    {

        private static DataChangeWatcher.PriceDiscNotifier priceWatcher;

        private static DataChangeWatcher.StockNotifier stockWatcher;

        static void Main(string[] args)
        {
            //priceWatcher = new DataChangeWatcher.PriceDiscNotifier();

            stockWatcher = new DataChangeWatcher.StockNotifier();

            //watcher.OnChange += new DataChangeWatcher.PriceDiscNotifier.PriceDiscWatcherEventHandler(QueueSQLWatcher_OnChange);

            //priceWatcher.Start();

            stockWatcher.Start();

            Console.WriteLine("Watcher started");

            Console.ReadLine();

            //priceWatcher.Stop();

            stockWatcher.Stop();
        }

        //private static void QueueSQLWatcher_OnChange()
        //{
        //    //Do something with the updated DataSet object
        //    Console.WriteLine("QueueSQLWatcher_OnChange");
        //}
    }
}
