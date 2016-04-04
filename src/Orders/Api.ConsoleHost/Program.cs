using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API;
using Microsoft.Owin.Hosting;

namespace Api.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8090"))
            {
                Console.ReadKey();
            }
        }
    }
}
