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

        public static GameController InitDemoGame(int server = 0,
            Dictionary<Attributes, int> humanStat = null, Dictionary<Attributes, int> aiStat = null,
            int maxCard = 0, List<int> customCard = null, List<int> customCardAi = null)
        {
            GameController gm;

            switch (server)
            {
                case 6:
                    gm = new GameController(new LogTest(), new TestServerForSpecialCard());
                    break;
                default:
                    gm = new GameController(new LogTest(), new TestServerForCustomCard());
                    break;
            }


            gm.AddPlayer(TypePlayer.Human, "Human", humanStat, customCard);
            gm.AddPlayer(TypePlayer.AI, "AI", aiStat, customCardAi);

            if (maxCard > 0)
                gm.ChangeMaxCard(maxCard);

            gm.StartGame(0);
            return gm;
        }


        public static void MakeMoveAi(GameController gm, ILog log)
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
