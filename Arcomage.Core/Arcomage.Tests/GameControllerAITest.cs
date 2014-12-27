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

        //TODO: нужен еще тест, что комп может пропустить ход, если нет подходящей карты

        /// <summary>
        /// Инициализация начала игры
        /// </summary>
        [SetUp]
        public void Init()
        {
           
        }


        /// <summary>
        /// Цель: проверить, что компьютер может начать игру 
        /// Результат: компьютер должен использовать карту и передать ход человеку
        /// </summary>
        [Test]
        public void AICanStartTheGame()
        {
            LogTest log = new LogTest();
            GameController gm = new GameController(log, new TestServer2());
   


            Assert.AreNotEqual(gm, null, "Геймконтроллер не должен быть пустым");
            gm.AddPlayer(TypePlayer.Human, "Human");
            gm.AddPlayer(TypePlayer.AI, "AI");


            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.StartGame);
            notify.Add("currentPlayer", TypePlayer.AI); //делаем подтасовку небольшую, чтобы начал свой ход компьютер
            gm.SendGameNotification(notify);


            Assert.AreEqual(gm.Status, CurrentAction.AIUseCardAnimation, "Текущий статус должен быть равным прорисовке карты компьютера");
        }


        /// <summary>
        /// Цель: проверить, что компьютер использовал карту
        /// Результат: компьютер должен использовать карту с id == 1, т.к. на данный момент она подходит под все условия
        /// </summary>
        [Test]
        public void WhichCardUseAI()
        {
            gm = GameControllerTestHelper.InitDemoGame();
            //GameControllerTestHelper.getCards(gm);

            GameControllerTestHelper.PassStroke(gm);

           
            Assert.AreEqual(gm.GetAIUsedCard().LastOrDefault().id, 2, "Компьютер должен использовать карту id 2");
        }



    

  
        /// <summary>
        /// Цель: проверить, что компьютер может выйграть 
        /// Результат: башня игрока должна быть уничтожена, в переменной Winner должно храниться имя компьютера (у нас он является Loser) 
        /// </summary>
        [Test]
        public void ComputerMustWin()
        {
            gm = GameControllerTestHelper.InitDemoGame(3);
           // GameControllerTestHelper.getCards(gm);

            GameControllerTestHelper.PassStroke(gm);
            

            Dictionary<string, object> notify4 = new Dictionary<string, object>();
            notify4.Add("CurrentAction", CurrentAction.AIMoveIsAnimated);
            gm.SendGameNotification(notify4);
            Assert.AreEqual(gm.Status, CurrentAction.UpdateStatAI, "Текущий статус должен быть равным обновлению статистики компьютера");

            Dictionary<string, object> notify5 = new Dictionary<string, object>();
            notify5.Add("CurrentAction", CurrentAction.EndAIMove);
            gm.SendGameNotification(notify5);


            Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");
            Assert.AreEqual(gm.Winner, "AI", "Компьютер не может проиграть!");

        }


        /// <summary>
        /// Цель: проверить, что AI пропустил ход и сбросил карту
        /// Результат: в логе должен быть найден id определенной карты
        /// </summary>
        [Test]
        public void AICanPassMove()
        {
            gm = GameControllerTestHelper.InitDemoGame(5);
           // GameControllerTestHelper.getCards(gm);

            GameControllerTestHelper.PassStroke(gm);

            var result = gm.logCard.Where(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped).LastOrDefault();

            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            Assert.AreEqual(result.card.id, 2, "AI должен сбросить карту 2");
        }


        /// <summary>
        /// Цель: проверить что компьютер получил другую карту, после использования специальной карты
        /// Результат: должна быть карта с определенным id 
        /// </summary>
        [Test]
        public void AICanGetAnotherCard()
        {
            gm = GameControllerTestHelper.InitDemoGame(4);
          //  GameControllerTestHelper.getCards(gm);

            GameControllerTestHelper.PassStroke(gm);

            var result = gm.logCard.Where(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).LastOrDefault();

            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            Assert.AreEqual(result.card.id, 6, "AI должен был использовать карту 6");

            result = gm.logCard.Where(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).FirstOrDefault();
            Assert.AreEqual(result.card.id, 55, "AI должен был использовать карту 55");


            var info = gm.GetAIUsedCard();
            Assert.AreEqual(info.Count, 2, "AI должен был использовать 2 карты");
        }

    }
}
