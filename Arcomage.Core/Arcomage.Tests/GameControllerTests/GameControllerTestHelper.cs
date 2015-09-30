using System.Collections.Generic;
using Arcomage.Core;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Entity;
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
            gameController.SendGameNotification(new Dictionary<string, object>()
            {
                { "CurrentAction", CurrentAction.PassStroke },
                {"ID", 1}
            });
            Assert.AreEqual(gameController.Status, CurrentAction.PassStroke, "������� ������ ������ ���� ������ ������ �����");
       
            gameController.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.AnimateHumanMove } });
            Assert.AreEqual(gameController.Status, CurrentAction.UpdateStatHuman, "������� ������ ������ ���� ������ ���������� ���������� ������");

            gameController.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.EndHumanMove } });
            Assert.AreEqual(gameController.Status, CurrentAction.AIUseCardAnimation, "������� ������ ������ ���� ������ ���������� ���� ����������");
        }

        public static GameController InitDemoGame(int server = 0)
        {
            LogTest log = new LogTest();
            GameController gm = null;

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
                default:
                    gm = new GameController(log, new TestServer());
                    break;
            }

            gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams());
            gm.AddPlayer(TypePlayer.AI, "AI", new GameStartParams());

            gm.SendGameNotification(new Dictionary<string, object>()
            {
                {"CurrentAction", CurrentAction.StartGame},
                {"currentPlayer", TypePlayer.Human}
            });

            return gm;
        }

        public static void useCard(int id, GameController gameController)
        {
            Assert.AreEqual(gameController.IsCanUseCard(id), true, "�� �������� ������������ �����");
            gameController.SendGameNotification(new Dictionary<string, object>()
            {
                { "CurrentAction", CurrentAction.HumanUseCard },
                {"ID", id}
            });
            Assert.AreEqual(gameController.Status, CurrentAction.HumanUseCard, "������ ���� ������, ��� ����� ����������� �����");

            gameController.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.AnimateHumanMove } });
            Assert.AreEqual(gameController.Status, CurrentAction.UpdateStatHuman, "������ ���� ������, ��� �������� ����� ������ � ������ ����� �������� ����������");
        }
    }
}
