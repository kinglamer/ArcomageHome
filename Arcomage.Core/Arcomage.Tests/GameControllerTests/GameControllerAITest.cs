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

    [SetUp]
        public void Init()
        {
           
        }
    LogTest log = new LogTest();

        /// <summary>
        /// Цель: проверить, что компьютер может начать игру 
        /// Результат: компьютер должен использовать карту и передать ход человеку
        /// </summary>
        [Test]
        public void AiCanStartTheGame()
        {
            GameController gm = new GameController(new LogTest(), new TestServer2());

            gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams());
            gm.AddPlayer(TypePlayer.AI, "AI", new GameStartParams());
            
            gm.StartGame(1);
            GameControllerTestHelper.MakeMoveAi(gm, log);
            Assert.AreEqual(gm.LogCard.Count(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used), 1,
                "Компьютер должен использовать карту");

            Assert.AreEqual(gm.CurrentPlayer.type, TypePlayer.Human, "Ход должен вернуться к человеку");
        }


        /// <summary>
        /// Цель: проверить, что компьютер использовал карту
        /// Результат: компьютер должен использовать карту с id == 1, т.к. на данный момент она подходит под все условия
        /// </summary>
        [Test]
        public void WhichCardUseAi()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> { 1 }, new List<int> { 2 });
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
            Assert.AreEqual(gm.GetAiUsedCard().LastOrDefault().id, 2, "Компьютер должен использовать карту id 2");
        }
        
  
        /// <summary>
        /// Цель: проверить, что компьютер может выйграть 
        /// Результат: башня игрока должна быть уничтожена, в переменной Winner должно храниться имя компьютера (у нас он является Loser) 
        /// </summary>
        [Test]
        public void ComputerMustWin()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(3);
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
           // gm.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.AIMoveIsAnimated } });
           // Assert.AreEqual(gm.Status, CurrentAction.UpdateStatAI, "Текущий статус должен быть равным обновлению статистики компьютера");
       
           // gm.SendGameNotification( new Dictionary<string, object>() {{"CurrentAction", CurrentAction.EndAIMove }});
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0, "Башня врага должна быть уничтожена");
            Assert.AreEqual(gm.Winner, "AI", "Компьютер не может проиграть!");

        }


        /// <summary>
        /// Цель: проверить, что AI пропустил ход и сбросил карту
        /// Результат: в логе должен быть найден id определенной карты
        /// </summary>
        [Test]
        public void AiCanPassMove()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(5, null, null, 6, null, new List<int> { 2 });
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            Assert.AreEqual(gm.LogCard.LastOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped).card.id, 2, "AI должен сбросить карту 2");
        }


        /// <summary>
        /// Цель: проверить что компьютер получил другую карту, после использования специальной карты
        /// Результат: должна быть карта с определенным id 
        /// </summary>
        [Test]
        public void AiCanGetAnotherCard()
        {
            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            GameController gm = GameControllerTestHelper.InitDemoGame(4, null, null, 6, null, new List<int> { 55, 6 });
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);

            Assert.AreEqual(gm.LogCard.LastOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).card.id, 6,
                "AI должен был использовать карту 6");

            Assert.AreEqual(gm.LogCard.FirstOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).card.id, 55,
                "AI должен был использовать карту 55");

            Assert.AreEqual(gm.GetAiUsedCard().Count, 2, "AI должен был использовать 2 карты");
        }

    }
}
