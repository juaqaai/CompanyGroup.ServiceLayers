using System;
using System.Collections.Generic;
using System.Linq;
using CompanyGroup.Sync.Msmq;

namespace CompanyGroup.MsmqConsoleHost
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                CompanyGroup.Sync.Msmq.Receiver receiver = new CompanyGroup.Sync.Msmq.Receiver();

                receiver.Start();

                Console.WriteLine("Service is running...");

                Console.WriteLine("Press <ENTER> to terminate service.");

                Console.ReadLine();

                receiver.Stop();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Message: {0} Source: {1} StackTrace: {2}", ex.Message, ex.Source, ex.StackTrace);
            }
        }
    }
}
