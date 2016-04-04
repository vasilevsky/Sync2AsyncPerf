using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Wcf;

namespace API.Tests
{
    public class WcfTestHost
    {
        private ServiceHost host;

        public WcfTestHost()
        {
            host = new ServiceHost(
                typeof(OrderService),
                new Uri("http://localhost:8088/OrderService"));

            host.AddServiceEndpoint(
                typeof(Wcf.IOrderService),
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
        }

        public void Start()
        {
            host.Open();
        }

        public void Stop()
        {
            host.Close();
        }
    }
}
