using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests
{
    class GameControllerTestHelper
    {
        /// <summary>
        /// Последовательность действий, позволяющая игроку получить карты
        /// </summary>
        /// <param name="gmController"></param>
        public static void getCards(GameController gmController)
        {
            //карты можно получить только, при соответствущем ходе
            Assert.AreEqual(gmController.status, CurrentAction.GetPlayerCard, "Сейчас не ход игрока");
            gmController.GetCard();

            //после получения карты, игра должно уведомить о том, что игрок все получил и ждем его хода
            Dictionary<string, object> notify1 = new Dictionary<string, object>();
            notify1.Add("CurrentAction", CurrentAction.WaitHumanMove);
            gmController.SendGameNotification(notify1);

            Assert.AreEqual(gmController.status, CurrentAction.WaitHumanMove, "Должно быть ожидание хода игрока");
        }


        /// <summary>
        /// Последовательность действий позволяющая игроку сбросить свой ход
        /// </summary>
        /// <param name="gameController"></param>
        public static void PassStroke(GameController gameController)
        {
            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.PassStroke);
            notify.Add("ID", 1);
            gameController.SendGameNotification(notify);
            Assert.AreEqual(gameController.status, CurrentAction.PassStroke, "Текущий статус должен быть равным сбросу карты");

            Dictionary<string, object> notify2 = new Dictionary<string, object>();
            notify2.Add("CurrentAction", CurrentAction.AnimateHumanMove);
            gameController.SendGameNotification(notify2);
            Assert.AreEqual(gameController.status, CurrentAction.UpdateStatHuman, "Текущий статус должен быть равным обновлению статистики игрока");

            Dictionary<string, object> notify3 = new Dictionary<string, object>();
            notify3.Add("CurrentAction", CurrentAction.EndHumanMove);
            gameController.SendGameNotification(notify3);
            Assert.AreEqual(gameController.status, CurrentAction.AIUseCardAnimation, "Текущий статус должен быть равным прорисовке хода компьютера");
        }

        public static GameController InitDemoGame(int server = 0)
        {
            LogTest log = new LogTest();
            GameController gm = null;

            if (server == 0)
            {
                gm = new GameController(log, new TestServer());
            }
            else if (server == 2)
            {
                gm = new GameController(log, new TestServer2());
            }

           Assert.AreNotEqual(gm, null, "Геймконтроллер не должен быть пустым");
            gm.AddPlayer(TypePlayer.Human, "Winner");
            gm.AddPlayer(TypePlayer.AI, "Loser");


            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.StartGame);
            notify.Add("currentPlayer", TypePlayer.Human); //делаем подтасовку небольшую, чтобы начал свой ход человек
            gm.SendGameNotification(notify);

            return gm;
        }
    }
}
