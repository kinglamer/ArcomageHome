using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
    [TestFixture]
    internal class GameControllerAITest
    {

        [SetUp]
        public void Init()
        {

        }

        private LogTest log = new LogTest();

        /// <summary>
        /// Цель: проверить, что компьютер может начать игру 
        /// Результат: компьютер должен использовать карту и передать ход человеку
        /// </summary>
        [Test]
        public void AiCanStartTheGame()
        {
            GameBuilder gameBuilder = new GameBuilder(log, new TestServerForCustomCard());

            gameBuilder.AddPlayer(TypePlayer.Human, "Human");
            gameBuilder.AddPlayer(TypePlayer.AI, "AI", null, new List<int> { 1 });

            GameModel gm = gameBuilder.StartGame(1);


            GameControllerTestHelper.MakeMoveAi(gm, log);
            Assert.AreEqual(
                gm.LogCard.Count(x => x.Player.type == TypePlayer.AI && x.GameAction == GameAction.MakeMove), 1,
                "Компьютер должен использовать карту");

            Assert.AreEqual(gm.CurrentPlayer.type, TypePlayer.AI, "Ход должен остаться за компом");
        }


        /// <summary>
        /// Цель: проверить, что компьютер использовал карту
        /// Результат: компьютер должен использовать карту с id == 1, т.к. на данный момент она подходит под все условия
        /// </summary>
        [Test]
        public void WhichCardUseAi()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1},
                new List<int> {2});
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
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1},
                new List<int> {3});
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
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
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 1, new List<int> {1},
                new List<int> {7});
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            Assert.AreEqual(
                gm.LogCard.LastOrDefault(x => x.Player.type == TypePlayer.AI && x.GameAction == GameAction.DropCard)
                    .Card.id, 7, "AI должен сбросить карту 2");
        }


        /// <summary>
        /// Цель: проверить что компьютер получил другую карту, после использования специальной карты
        /// Результат: должна быть карта с определенным id 
        /// </summary>
        [Test]
        public void AiCanGetAnotherCard()
        {
            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1},
                new List<int> {56, 6});
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);

            Assert.AreEqual(
                gm.LogCard.LastOrDefault(x => x.Player.type == TypePlayer.AI && x.GameAction == GameAction.MakeMove)
                    .Card.id, 6,
                "AI должен был использовать карту 6");

            Assert.AreEqual(
                gm.LogCard.FirstOrDefault(x => x.Player.type == TypePlayer.AI && x.GameAction == GameAction.MakeMove)
                    .Card.id, 56,
                "AI должен был использовать карту 55");

            Assert.AreEqual(gm.GetAiUsedCard().Count, 2, "AI должен был использовать 2 карты");
        }

    }
}
