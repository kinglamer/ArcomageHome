using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Core.Interfaces;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Entity;
using Arcomage.Entity.Interfaces;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
    class GameControllerTestHelper
    {
 
        /// <summary>
        /// ������������������ �������� ����������� ������ �������� ���� ���
        /// </summary>
        /// <param name="gameController"></param>
        public static void PassStroke(GameController gameController)
        {
      
            gameController.MakePlayerMove(1, true);

   

           // Assert.AreEqual(gameController.Status, CurrentAction.PassStroke, "������� ������ ������ ���� ������ ������ �����");
       
            //gameController.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.AnimateHumanMove } });
            //Assert.AreEqual(gameController.Status, CurrentAction.UpdateStatHuman, "������� ������ ������ ���� ������ ���������� ���������� ������");

            //gameController.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.EndHumanMove } });
           // Assert.AreEqual(gameController.Status, CurrentAction.AIUseCardAnimation, "������� ������ ������ ���� ������ ���������� ���� ����������");
        }

        public static GameController InitDemoGame(int server = 0, IStartParams humanStat = null, IStartParams AIStat = null, int maxCard = 0, List<int> customCard = null, List<int> customCardAi = null)
        {
            LogTest log = new LogTest();
            GameController gm;

            switch (server)
            {
                case 2:
                    gm = new GameController(log, new TestServer2());
                    break;
                case 3:
                    gm = new GameController(log, new TestServer3());
                    break;
                case 4:
                    gm = new GameController(log, new TestServer4());
                    break;
                case 5:
                    gm = new GameController(log, new TestServer5());
                    break;
                case 6:
                    gm = new GameController(log, new TestServer6());
                    break;
                case 7:
                    gm = new GameController(log, new ArcoSQLLiteServer(@"arcomageDB.db"));
                    break;
                default:
                    gm = new GameController(log, new TestServer());
                    break;
            }

                humanStat = humanStat ?? new GameStartParams();
                 AIStat = AIStat ?? new GameStartParams();

            gm.AddPlayer(TypePlayer.Human, "Human", humanStat, customCard);
            gm.AddPlayer(TypePlayer.AI, "AI", AIStat, customCardAi);
       
            if (maxCard > 0)
                gm.ChangeMaxCard(maxCard);

            gm.StartGame(0);
            return gm;
        }

        public static void UseCard(int id, GameController gameController)
        {
            Assert.AreEqual(gameController.CanUseCard(id), true, "�� �������� ������������ �����");
    

            gameController.MakePlayerMove(id);

          //  Assert.AreEqual(gameController.Status, CurrentAction.HumanUseCard, "������ ���� ������, ��� ����� ����������� �����");

           // gameController.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.AnimateHumanMove } });
           // Assert.AreEqual(gameController.Status, CurrentAction.UpdateStatHuman, "������ ���� ������, ��� �������� ����� ������ � ������ ����� �������� ����������");
        }

        public static void MakeMoveAi(GameController gm, ILog log)
        {
            log.Info("----===== ��� ���������� =====----");

            gm.MakePlayerMove(gm.CurrentPlayer.ChooseCard().id);

            GameAction[] copyAction = new GameAction[gm.CurrentPlayer.gameActions.Count];
            gm.CurrentPlayer.gameActions.CopyTo(copyAction);

            foreach (var gameAction in copyAction)
            {
                switch (gameAction)
                {
                    case GameAction.DropCard: //TODO: ������, ��� ������� ���� �����
                        gm.MakePlayerMove(gm.CurrentPlayer.Cards.First().id, true);
                        break;
                    case GameAction.PlayAgain:
                        gm.MakePlayerMove(gm.CurrentPlayer.ChooseCard().id);
                        break;
                }
            }

            gm.NextPlayerTurn();

            log.Info("----===== ��� ���������� ���������� =====----");
        }
    }
}
