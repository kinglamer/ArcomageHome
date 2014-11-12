using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using NUnit.Framework;

namespace Arcomage.Tests
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
                "Строка преобразована неправильно");


        }

    }
}
