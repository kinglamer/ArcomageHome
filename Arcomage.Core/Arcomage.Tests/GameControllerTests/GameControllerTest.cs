using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
    [TestFixture]
    internal class GameControllerTest
    {


        [SetUp]
        public void Init()
        {

        }

 
        /// <summary>
        /// Цель: проверить, что игрок может победить
        /// Результат:  башня противника должна быть уничтожена, в переменной Winner должно храниться имя пользователя 
        /// </summary>
        [Test]
        public void PlayerMustWin()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1});
            gm.MakePlayerMove(1);
            gm.NextPlayerTurn();
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0, "Башня врага должна быть уничтожена");
            Assert.AreEqual(gm.Winner, "Human", "Игрок не может проиграть!");
        }

        /// <summary>
        /// Цель: Проверка, что человек может использовать карту
        /// Результат: геймконтролер сообщает, что карту возможно использовать
        /// </summary>
        [Test]
        public void PlayerCanUserCard()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1});
            Assert.AreEqual(gm.CanUseCard(1), true, "Не возможно использовать карту");
        }


        /// <summary>
        /// Цель: проверить, что при инициализации параметры игрока не пусты
        /// Результат: должны быть значения и количество карты не равно 0
        /// </summary>
        [Test]
        public void CheckPlayerInit()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame();
            Assert.IsNotNull(gm.CurrentPlayer.PlayerParams, "Стартовые параметры игрока не должны быть пустыми");
            Assert.IsTrue(gm.CurrentPlayer.Cards.Count > 1, " Должно быть хотя бы одна карта");
        }

        /// <summary>
        /// Цель: проверить как применяются параметры карты к параметрам игроков
        /// Результат: при вычитание или сложение параметров игрока с параметрами карты, должны получить конкретное значение
        /// для человека - сумма должна быть равна кол-во стартовых кристалов + количество кристалов в карте + прирост шахты кристалов . В одном из параметров еще идет вычет значений стоимости карты
        /// для компьютера - только стартовое значение + параметры карты 
        /// </summary>
        [Test]
        public void CheckApplyCardParams()
        {
            GameBuilder gameBuilder = new GameBuilder(new LogTest(), new TestServerForCustomCard());
            gameBuilder.AddPlayer(TypePlayer.Human, "Human", null, new List<int> { 2 });
            gameBuilder.AddPlayer(TypePlayer.Human, "AI");
            GameModel gm = gameBuilder.StartGame(0);

            gm.MakePlayerMove(2);
            gm.NextPlayerTurn();

            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 5 - 4,
                "Не правильно применен параметр PlayerWall");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 10 - 8,
                "Не правильно применен параметр PlayerTower");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.DiamondMines], 1 + 2,
                "Не правильно применен параметр PlayerDiamondMines");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Menagerie], 1 + 3,
                "Не правильно применен параметр PlayerMenagerie");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Colliery], 1 + 4,
                "Не правильно применен параметр PlayerColliery");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Diamonds], 5 + 11 + 3,
                "Не правильно применен параметр PlayerDiamonds");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Animals], 5 + 12 + 4 - 5,
                "Не правильно применен параметр PlayerAnimals");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Rocks], 5 + 13 + 5,
                "Не правильно применен параметр PlayerRocks");

            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 5 - 4,
                "Не правильно применен параметр EnemyWall");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 10 - 8,
                "Не правильно применен параметр EnemyTower");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.DiamondMines], 1 + 2,
                "Не правильно применен параметр EnemyDiamondMines");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 1 + 3,
                "Не правильно применен параметр EnemyMenagerie");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 1 + 4,
                "Не правильно применен параметр EnemyColliery");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 5 + 11,
                "Не правильно применен параметр EnemyDiamonds");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Animals], 5 + 12,
                "Не правильно применен параметр EnemyAnimals");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 5 + 13,
                "Не правильно применен параметр EnemyRocks");

        }

        /// <summary>
        /// цель: проверить применение одного параметра
        /// результат: сумма должна быть равна кол-во стартовых кристалов + количество кристалов в карте + прирост шахты кристалов 
        /// </summary>
        [Test]
        public void CheckAddDiamonds()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {6});
            gm.MakePlayerMove(6);
            gm.NextPlayerTurn();
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Diamonds], 5 + 11 + 1,
                "Не правильно применен параметр PlayerDiamonds");
        }

        /// <summary>
        /// Цель: проверить, как распределяется прямой урон по врагу
        /// Результат: получить определенные значения башни и стены
        /// </summary>
        [Test]
        public void CheckApplyEnemyDirectDamage()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {3});
            gm.MakePlayerMove(3);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0,
                "Не правильно применен параметр EnemyDirectDamage");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0,
                "Не правильно применен параметр EnemyDirectDamage");

        }

        /// <summary>
        /// Цель: проверить, как распределяется прямой урон по игроку
        /// Результат: получить определенные значения башни и стены
        /// </summary>
        [Test]
        public void CheckApplyPlayerDirectDamage()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {4});
            gm.MakePlayerMove(4);

            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 0,
                "Не правильно применен параметр PlayerDirectDamage");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 0,
                "Не правильно применен параметр PlayerDirectDamage");
        }


        /// <summary>
        /// Цель: проверить,что игрок может получить новую карту, после использования карты с параметром "Взять еще одну карту"
        /// Результат: статус игры должен быть равным "Игрок должен получить карту" 
        /// </summary>
        [Test]
        public void CheckPlayAgain()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {55});
            Assert.AreEqual(gm.CanUseCard(55), true, "Не возможно использовать карту");


            gm.MakePlayerMove(55);
            gm.NextPlayerTurn();
            Assert.AreEqual(
                gm.LogCard.FirstOrDefault(x => x.Player.type == TypePlayer.Human && x.GameAction == GameAction.MakeMove)
                    .Card.id, 55, "Human должен был использовать карту 55");


            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 5, "Прироста камней не должно быть");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 6, "Прироста брилиантов не должно быть");
            Assert.AreEqual(gm.CurrentPlayer.gameActions.Contains(GameAction.MakeMoveAgain), true,
                "Человек может использовать еще одну карту");
        }

        /// <summary>
        /// Цель: Проверка, что игрок может пропустить ход
        /// Результат: Статус должен сообщить, что текущий ход у противника (AI)
        /// </summary>
        [Test]
        public void PlayerCanPassMove()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1});
            gm.MakePlayerMove(1, true);
            var result =
                gm.LogCard.FirstOrDefault(x => x.Player.type == TypePlayer.Human && x.GameAction == GameAction.DropCard);

            Assert.AreEqual(result.Card.id, 1, "Human должен сбросить карту 1");

        }

        /// <summary>
        /// Цель: проверить, что возможно сделать подтасовку карт
        /// Результат: в руке должна быть определенная карта
        /// </summary>
        [Test]
        public void TestCardTricker()
        {
            LogTest log = new LogTest();
            GameBuilder gameBuilder = new GameBuilder(log, new ArcoSQLLiteServer(@"arcomageDB.db"));
            gameBuilder.AddPlayer(TypePlayer.Human, "Human", null, new List<int> { 39, 11, 12, 13, 14, 15 });
            gameBuilder.AddPlayer(TypePlayer.AI, "AI");


            GameModel gm = gameBuilder.StartGame(0);
            Assert.AreEqual(gm.CurrentPlayer.Cards.FirstOrDefault(x => x.id == 39).id, 39,
                "У игрока должна быть карта с №39");
        }
    }
}
