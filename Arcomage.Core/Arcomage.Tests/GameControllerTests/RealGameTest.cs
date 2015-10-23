using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
    [TestFixture]
    class RealGameTest
    {

        [Test]
        public void HumanVersusHuman()
        {
            CardPickerTest cardPicker = new CardPickerTest();
            GameBuilder gameBuilder = new GameBuilder(new LogTest(), new ArcoSQLLiteServer(@"arcomageDB.db"));

            gameBuilder.AddPlayer(TypePlayer.Human, "Human1", cardPicker);
            gameBuilder.AddPlayer(TypePlayer.Human, "Human2", cardPicker);

            GameModel gm = gameBuilder.StartGame(0);



            gm.Update();
        }
    }
}
