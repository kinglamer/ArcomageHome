using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Arcomage.Tests
{
    [TestFixture]
    public class NUnitExample
    {

        [Test]
        public void SumOfTwoNumbers()
        {

            Assert.AreEqual(10, 5 + 5);

        }



        [Test]

        public void AreTheValuesTheSame()
        {
            string i = "10";
            Assert.AreSame(i, i);

        }

    } 
}
