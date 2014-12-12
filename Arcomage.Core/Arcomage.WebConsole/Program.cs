using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Arcomage.WebConsole.Arcomage;

namespace Arcomage.WebConsole
{
    class Program
    {
        private const string url = "http://arcomage.somee.com/ArcoServer.svc?wsdl";//"http://kinglamer-001-site1.smarterasp.net/ArcoServer.svc?wsdl";
        static void Main(string[] args)
        {

          

            using (ArcoServerClient host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(url)))
            {
                string test = host.GetRandomCard();
                Console.WriteLine(test);
            }

            Console.ReadLine();
        }
    }
}
