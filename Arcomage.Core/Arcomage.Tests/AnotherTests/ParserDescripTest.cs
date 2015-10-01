using System;
using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Core.ArcomageService;
using Arcomage.Core.Common;
using Arcomage.Entity.Cards;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Arcomage.Tests.AnotherTests
{
    [TestFixture]
    class ParserDescripTest
    {

        [Test]
        public void StringMustBeEqual()
        {
            string value =
               "<span style=\"font-weight: bold; color: #66ff00; font-style: italic; font-size: 8pt;\">Test</span>";

            Assert.AreEqual(ParseDescription.Parse(value), "<size=8><i><color=#66ff00><b>Test</b></color></i></size>",
                "������ ������������� �����������");
        }


        [Test]
        public void TestSpecialCardFromServer()
        {
            int countDesc = 0;
            IArcoServer host = new ArcoSQLLiteServer(@"arcomageDB.db");
            string cardFromServer = host.GetRandomCard();
            List<Card> result = JsonConvert.DeserializeObject<List<Card>>(cardFromServer);

            foreach (var item in result)
            {
                string description = ParseDescription.Parse(item.description);

                if (item.description.Length > 0 && description.Length == 0)
                {
                    countDesc++;
                    Console.WriteLine("item " + item.name + " has no descript. Original text: " + Environment.NewLine + item.description + Environment.NewLine);
                }
               
            }

            Assert.AreEqual(countDesc, 0, "�� ������ ���� ���� � ������������� ���������");
        }

        [Test]
        public void SpecialSymbolsInCardTest()
        {
            IArcoServer host = new ArcoSQLLiteServer(@"arcomageDB.db");
            string cardFromServer = host.GetRandomCard();
            List<Card> result = JsonConvert.DeserializeObject<List<Card>>(cardFromServer);
            string description = "";

            foreach (var item in result.Where(x=>x.id == 90))
            {
                 description = ParseDescription.Parse(item.description);
            }

            Assert.AreEqual(description.IndexOf("&"), -1, "�� ������ ���� ������ ���������");
        }

        [Test]
        public void SpecialSymbolsInCard2Test()
        {
            IArcoServer host = new ArcoSQLLiteServer(@"arcomageDB.db");
            string cardFromServer = host.GetRandomCard();
            List<Card> result = JsonConvert.DeserializeObject<List<Card>>(cardFromServer);
            string description = "";

            foreach (var item in result.Where(x => x.id == 8))
            {
                description = ParseDescription.Parse(item.description);
            }

            Assert.AreEqual(description.IndexOf("&"), -1, "�� ������ ���� ������ ���������");
        }


        [Test]
        public void SqliteDBTest()
        {
            IArcoServer host = new ArcoSQLLiteServer(@"arcomageDB.db");
            string cardFromServer = host.GetRandomCard();
            List<Card> result = JsonConvert.DeserializeObject<List<Card>>(cardFromServer);

            Assert.AreEqual(result.Count, 102, "������ ���� 102 �����");
            Assert.AreNotEqual(result.First().id, 2, "������ ������ �� ������ ���� ����� 2");
        }
    }
}
