using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.WebConsole.Arcomage;

namespace Arcomage.WebConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ArcoServerClient host = new ArcoServerClient())
            {
                string test = host.GetRandomCard();
                Console.WriteLine(test);
            }

            Console.ReadLine();
        }
    }
}
