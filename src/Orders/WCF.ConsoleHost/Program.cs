using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Wcf;

namespace WCF.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(
                typeof(OrderService),
                new Uri("http://localhost:8089/OrderService"));

            host.AddServiceEndpoint(
                typeof(IOrderService),
                new BasicHttpBinding(), 
                "");

            var serviceBehavior = new ServiceMetadataBehavior();
            serviceBehavior.HttpGetEnabled = true;

            var debugBehaviour = host.Description.Behaviors.FirstOrDefault(m => m is ServiceDebugBehavior) as ServiceDebugBehavior;
            if (debugBehaviour != null)
            {
                debugBehaviour.IncludeExceptionDetailInFaults = true;
            }

            host.Description.Behaviors.Add(serviceBehavior);

            host.Open();

            Console.ReadKey();

            host.Close();
        }
    }
}
