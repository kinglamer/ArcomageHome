using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
    [TestFixture]
    class GameControllerAITest
    {
        private GameController gm;

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
            GameController gm = new GameController(new LogTest(), new TestServer2());

            gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams());
            gm.AddPlayer(TypePlayer.AI, "AI", new GameStartParams());
    
            gm.SendGameNotification(new Dictionary<string, object>()
            {
                { "CurrentAction", CurrentAction.StartGame },
                {"currentPlayer", TypePlayer.AI}
            });

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
            GameControllerTestHelper.PassStroke(gm);
            
            gm.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.AIMoveIsAnimated } });
            Assert.AreEqual(gm.Status, CurrentAction.UpdateStatAI, "Текущий статус должен быть равным обновлению статистики компьютера");
       
            gm.SendGameNotification( new Dictionary<string, object>() {{"CurrentAction", CurrentAction.EndAIMove }});
            Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Attributes.Tower], 0, "Башня врага должна быть уничтожена");
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
            GameControllerTestHelper.PassStroke(gm);

            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            Assert.AreEqual(gm.logCard.LastOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped).card.id, 2, "AI должен сбросить карту 2");
        }


        /// <summary>
        /// Цель: проверить что компьютер получил другую карту, после использования специальной карты
        /// Результат: должна быть карта с определенным id 
        /// </summary>
        [Test]
        public void AICanGetAnotherCard()
        {
            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            gm = GameControllerTestHelper.InitDemoGame(4);
            GameControllerTestHelper.PassStroke(gm);
            
            Assert.AreEqual(gm.logCard.LastOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).card.id, 6,
                "AI должен был использовать карту 6");

            Assert.AreEqual(gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).card.id, 55,
                "AI должен был использовать карту 55");

            Assert.AreEqual(gm.GetAIUsedCard().Count, 2, "AI должен был использовать 2 карты");
        }

    }
}
