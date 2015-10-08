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


            gameBuilder.AddPlayer(TypePlayer.Human, "Human", humanStat, customCard);
            gameBuilder.AddPlayer(TypePlayer.AI, "AI", aiStat, customCardAi);

            if (maxCard > 0)
                gameBuilder.ChangeMaxCard(maxCard);

          return gameBuilder.StartGame(0);
        }


        public static void MakeMoveAi(GameModel gm, ILog log)
        {
            log.Info("----===== Ход компьютера =====----");
            gm.MakePlayerMove(gm.CurrentPlayer.ChooseCard().id);

            GameAction[] copyAction = new GameAction[gm.CurrentPlayer.gameActions.Count];
            gm.CurrentPlayer.gameActions.CopyTo(copyAction);

            foreach (var gameAction in copyAction)
            {
                switch (gameAction)
                {
                    case GameAction.DropCard: //TODO: зашить, что сначала идет сброс
                        gm.MakePlayerMove(gm.CurrentPlayer.Cards.First().id, true);
                        break;
                    case GameAction.MakeMoveAgain:
                        gm.MakePlayerMove(gm.CurrentPlayer.ChooseCard().id);
                        break;
                }
            }

            gm.NextPlayerTurn();
            log.Info("----===== Ход компьютера закончился =====----");
        }
    }
}
