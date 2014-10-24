using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Common;
using Arcomage.Core;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests
{
     [TestFixture]
    class PlayerHelperTest
    {
         [Test]
         public void PlayerMustWin()
         {
             LogTest log = new LogTest();
             PlayerHelper ph = new PlayerHelper(log, "Winner", new TestServer());

             PlayerHelper en = new PlayerHelper(log, "Loser");
             ph.SetTheEnemy(en);


             var card = ph.GetCard();

             Assert.AreEqual(ph.UseCard(1), true, "Не возможно использовать карту");

             Assert.AreEqual(ph.IsPlayerWin(),null, "Игрок не может проиграть!");
             Assert.AreEqual(en.IsPlayerWin(), false, "Компьютер должен проиграть!");
         }


         [Test]
         public void PlayerCanUserCard()
         {
             LogTest log = new LogTest();
             PlayerHelper ph = new PlayerHelper(log, "Winner", new TestServer());
             PlayerHelper en = new PlayerHelper(log, "Loser");
             ph.SetTheEnemy(en);

             ph.GetCard();

             Assert.AreEqual(ph.UseCard(1), true, "Не возможно использовать карту");
         }

         //TODO: дописать тест, что параметры все применились

    }
}
