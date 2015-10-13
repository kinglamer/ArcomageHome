using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Core.Interfaces;
using Arcomage.Entity;
using Arcomage.Tests.Moq;

namespace Arcomage.Tests.GameControllerTests
{
    internal class GameControllerTestHelper
    {
        public static CardPickerTest CardPicker = new CardPickerTest();
        public static GameModel InitDemoGame(int server = 0,
            Dictionary<Attributes, int> humanStat = null, Dictionary<Attributes, int> aiStat = null,
            int maxCard = 0, List<int> customCard = null, List<int> customCardAi = null)
        {
            GameBuilder gameBuilder;
            switch (server)
            {
                case 6:
                    gameBuilder = new GameBuilder(new LogTest(), new TestServerForSpecialCard());
                    break;
                default:
                    gameBuilder = new GameBuilder(new LogTest(), new TestServerForCustomCard());
                    break;
            }


            gameBuilder.AddPlayer(TypePlayer.Human, "Human", CardPicker, humanStat, customCard);
            gameBuilder.AddPlayer(TypePlayer.AI, "AI", null, aiStat, customCardAi);

            if (maxCard > 0)
                gameBuilder.ChangeMaxCard(maxCard);

          return gameBuilder.StartGame(0);
        }
    }
}
