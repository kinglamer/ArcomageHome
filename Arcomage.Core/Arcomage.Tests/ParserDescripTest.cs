using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using Arcomage.Tests.MoqStartParams;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Arcomage.Tests
{
    [TestFixture]
    class ParserDescripTest
    {

        LogTest log = new LogTest();
        private GameController gm;

        [Test]
        public void StringMustBeEqual()
        {
            string value =
               "<span style=\"font-weight: bold; color: #66ff00; font-style: italic; font-size: 8pt;\">Test</span>";

            Assert.AreEqual(ParseDescription.Parse(value), "<size=8><i><color=#66ff00><b>Test</b></color></i></size>",
                "Строка преобразована неправильно");


        }


        [Test]
        public void TestSpecialCardFromServer()
        {
            int countDesc = 0;
            string url = "http://arcomage.somee.com/ArcoServer.svc?wsdl"; 
            IArcoServer host = new ArcoServerClient(new BasicHttpBinding(), new EndpointAddress(url));

            string cardFromServer = host.GetRandomCard();

            List<Card> result = JsonConvert.DeserializeObject<List<Card>>(cardFromServer);

            foreach (var item in result)
            {
                string description = ParseDescription.Parse(item.description);

                if (item.description.Length > 0 && description.Length == 0)
                {
                  //  ParseDescription.Parse(item.description);
                    countDesc++;
                    Console.WriteLine("item " + item.name + " has no descript. Original text: " + Environment.NewLine + item.description + Environment.NewLine);
                }
               
            }

            Assert.AreEqual(countDesc, 0, "Не должно быть карт с отсутствующим описанием");
        }

       

    }
}
