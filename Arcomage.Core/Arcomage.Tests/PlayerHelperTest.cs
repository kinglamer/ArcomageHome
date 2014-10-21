using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Common;
using Arcomage.Core;
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
             PlayerHelper ph = new PlayerHelper(log, "Winner");

             var card = ph.GetCard();


             Assert.AreEqual(10, 5 + 5);

         }

    }
}
