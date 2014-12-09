using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests
{
    [TestFixture]
    class GameControllerAITest
    {
        private GameController gm;

        [SetUp]
        public void Init()
        {
            LogTest log = new LogTest();
            gm = new GameController(log, new TestServer());
            gm.AddPlayer(TypePlayer.Human, "Winner");
            gm.AddPlayer(TypePlayer.AI, "Loser");


            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.StartGame);
            notify.Add("currentPlayer", TypePlayer.Human); //делаем подтасовку небольшую, чтобы начал свой ход человек
            gm.SendGameNotification(notify);
        }

        [Test]
        public void WhichCardUseAI()
        {
            GameControllerTestHelper.getCards(gm);

            GameControllerTestHelper.PassStroke(gm);

            Dictionary<string, object> notify2 = new Dictionary<string, object>();
            notify2.Add("CurrentAction", CurrentAction.AnimateHumanMove);
            gm.SendGameNotification(notify2);
            Assert.AreEqual(gm.status, CurrentAction.UpdateStatHuman, "Текущий статус должен быть равным обновлению статистики игрока");

            Dictionary<string, object> notify3 = new Dictionary<string, object>();
            notify3.Add("CurrentAction", CurrentAction.EndHumanMove);
            gm.SendGameNotification(notify3);
            Assert.AreEqual(gm.status, CurrentAction.AIUseCardAnimation, "Текущий статус должен быть равным прорисовке хода компьютера");

            Assert.AreEqual(gm.GetAIUsedCard().First().id, 1, "Компьютер должен использовать первую карту");
        }

        //TODO: нужен еще тест, что комп может пропустить ход, если нет подходящей карты

        //TODO: продумать когда определяется, что игра закончилась и что происходит со статусами
        //TODO: нужно написать тест, что игра перешла в состояние выйграша игрока

        [Test]
        public void ComputerMustWin()
        {
            GameControllerTestHelper.getCards(gm);

            GameControllerTestHelper.PassStroke(gm);
            

            Dictionary<string, object> notify4 = new Dictionary<string, object>();
            notify4.Add("CurrentAction", CurrentAction.AIMoveIsAnimated);
            gm.SendGameNotification(notify4);
            Assert.AreEqual(gm.status, CurrentAction.UpdateStatAI, "Текущий статус должен быть равным обновлению статистики компьютера");

            Dictionary<string, object> notify5 = new Dictionary<string, object>();
            notify5.Add("CurrentAction", CurrentAction.EndAIMove);
            gm.SendGameNotification(notify5);


            Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");
            Assert.AreEqual(gm.Winner, "Loser", "Компьютер не может проиграть!");

        }

    }
}
