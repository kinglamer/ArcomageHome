using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Entity;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
     [TestFixture]
    class GameControllerTest
     {

         private GameController gm;

         [SetUp]
         public void Init()
         {
            
         }

         /// <summary>
         /// Цель: Проверка, что игра стартовала
         /// Результат: статус игры должен быть равным "GetPlayerCard" - он означает, что игрок должен получить карты
         /// </summary>
         [Test]
         public void GameIsStarted()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "Игра не стартовала");
         }

         /// <summary>
         /// Цель: проверить, что игрок может победить
         /// Результат:  башня противника должна быть уничтожена, в переменной Winner должно храниться имя пользователя 
         /// </summary>
         [Test]
         public void PlayerMustWin()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.useCard(1, gm);

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Attributes.Tower], 0, "Башня врага должна быть уничтожена");
             Assert.AreEqual(gm.Status, CurrentAction.UpdateStatHuman, "Текущий статус должен быть равным обновлению статистики игрока");

             gm.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.EndHumanMove } });
             Assert.AreEqual(gm.Winner, "Human", "Игрок не может проиграть!");

             
         }

         /// <summary>
         /// Цель: Проверка, что человек может использовать карту
         /// Результат: геймконтролер сообщает, что карту возможно использовать
         /// </summary>
         [Test]
         public void PlayerCanUserCard()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             Assert.AreEqual(gm.IsCanUseCard(1), true, "Не возможно использовать карту");
         }
        

         /// <summary>
         /// Цель: проверить, что при инициализации параметры игрока не пусты
         /// Результат: должны быть значения и количество карты не равно 0
         /// </summary>
         [Test]
         public void CheckPlayerInit()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             Assert.IsNotNull(gm.GetPlayerParams(), "Стартовые параметры игрока не должны быть пустыми");
             Assert.IsTrue(gm.GetPlayersCard().Count > 1, " Должно быть хотя бы одна карта");
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

             gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.useCard(2, gm);
             var playerParams = gm.GetPlayerParams(SelectPlayer.First);


             Assert.AreEqual(playerParams[Attributes.Wall], 5 - 4, "Не правильно применен параметр PlayerWall");
             Assert.AreEqual(playerParams[Attributes.Tower], 10 - 8, "Не правильно применен параметр PlayerTower");
             Assert.AreEqual(playerParams[Attributes.DiamondMines], 1 + 2, "Не правильно применен параметр PlayerDiamondMines");
             Assert.AreEqual(playerParams[Attributes.Menagerie], 1 + 3, "Не правильно применен параметр PlayerMenagerie");
             Assert.AreEqual(playerParams[Attributes.Colliery], 1 + 4, "Не правильно применен параметр PlayerColliery");
             Assert.AreEqual(playerParams[Attributes.Diamonds], 5 + 11 + 3, "Не правильно применен параметр PlayerDiamonds");
             Assert.AreEqual(playerParams[Attributes.Animals], 5 + 12 + 4 -5, "Не правильно применен параметр PlayerAnimals");
             Assert.AreEqual(playerParams[Attributes.Rocks], 5 + 13 + 5, "Не правильно применен параметр PlayerRocks");

             playerParams = gm.GetPlayerParams(SelectPlayer.Second);
             Assert.AreEqual(playerParams[Attributes.Wall], 5 - 4, "Не правильно применен параметр EnemyWall");
             Assert.AreEqual(playerParams[Attributes.Tower], 10 - 8, "Не правильно применен параметр EnemyTower");
             Assert.AreEqual(playerParams[Attributes.DiamondMines], 1 + 2, "Не правильно применен параметр EnemyDiamondMines");
             Assert.AreEqual(playerParams[Attributes.Menagerie], 1 + 3, "Не правильно применен параметр EnemyMenagerie");
             Assert.AreEqual(playerParams[Attributes.Colliery], 1 + 4, "Не правильно применен параметр EnemyColliery");
             Assert.AreEqual(playerParams[Attributes.Diamonds], 5 + 11, "Не правильно применен параметр EnemyDiamonds");
             Assert.AreEqual(playerParams[Attributes.Animals], 5 + 12, "Не правильно применен параметр EnemyAnimals");
             Assert.AreEqual(playerParams[Attributes.Rocks], 5 + 13, "Не правильно применен параметр EnemyRocks");
             
         }

         /// <summary>
         /// цель: проверить применение одного параметра
         /// результат: сумма должна быть равна кол-во стартовых кристалов + количество кристалов в карте + прирост шахты кристалов 
         /// </summary>
         [Test]
         public void CheckAddDiamonds()
         {
             gm = GameControllerTestHelper.InitDemoGame(2);
             GameControllerTestHelper.useCard(6, gm);
             var playerParams = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(playerParams[Attributes.Diamonds], 5 + 11 + 1, "Не правильно применен параметр PlayerDiamonds");
         }

         /// <summary>
         /// Цель: проверить, как распределяется прямой урон по врагу
         /// Результат: получить определенные значения башни и стены
         /// </summary>
         [Test]
         public void CheckApplyEnemyDirectDamage()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.useCard(3, gm);
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Attributes.Wall], 0, "Не правильно применен параметр EnemyDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Attributes.Tower], 0, "Не правильно применен параметр EnemyDirectDamage");
     
         }

         /// <summary>
         /// Цель: проверить, как распределяется прямой урон по игроку
         /// Результат: получить определенные значения башни и стены
         /// </summary>
         [Test]
         public void CheckApplyPlayerDirectDamage()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.useCard(4, gm);

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Attributes.Wall], 0,"Не правильно применен параметр PlayerDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Attributes.Tower], 0,"Не правильно применен параметр PlayerDirectDamage");
         }


         /// <summary>
         /// Цель: проверить,что игрок может получить новую карту, после использования карты с параметром "Взять еще одну карту"
         /// Результат: статус игры должен быть равным "Игрок должен получить карту" 
         /// </summary>
         [Test]
         public void CheckPlayAgain()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             Assert.AreEqual(gm.IsCanUseCard(55), true, "Не возможно использовать карту");

             gm.SendGameNotification(new Dictionary<string, object>()
             {
                 {"CurrentAction", CurrentAction.HumanUseCard},
                 {"ID", 55}
             });

             Assert.AreEqual(gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Used).card.id, 55, "Human должен был использовать карту 55");

             var result2 = gm.GetPlayerParams();
             Assert.AreEqual(result2[Attributes.Rocks], 7, "Должен быть прирост камней");
             Assert.AreEqual(result2[Attributes.Diamonds], 7, "Должен быть прирост брилиантов");
             Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "Человек может использовать еще одну карту");
         }

         /// <summary>
         /// Цель: Проверка, что игрок может пропустить ход
         /// Результат: Статус должен сообщить, что текущий ход у противника (AI)
         /// </summary>
         [Test]
         public void PlayerCanPassMove()
         {
             gm = GameControllerTestHelper.InitDemoGame(5);
             GameControllerTestHelper.PassStroke(gm);
             var result = gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Droped);
             //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
             Assert.AreEqual(result.card.id, 1, "Human должен сбросить карту 1");

         }

         /// <summary>
         /// Цель: проверить, что возможно сделать подтасовку карт
         /// Результат: в руке должна быть определенная карта
         /// </summary>
         [Test]
         public void TestCardTricker()
         {
             LogTest log = new LogTest();
             GameController gm = new GameController(log, new ArcoSQLLiteServer(@"arcomageDB.db"));
             gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams());
             gm.AddPlayer(TypePlayer.AI, "AI", new GameStartParams());

             gm.SendGameNotification(new Dictionary<string, object>()
             {
                 {"CurrentAction", CurrentAction.StartGame},
                 {"currentPlayer", TypePlayer.Human},
                 {"CardTricksters", new List<int> {39, 11, 12, 13, 14, 15}}
             });

             Assert.AreEqual(gm.GetPlayersCard().FirstOrDefault(x => x.id == 39).id, 39,
                 "У игрока должна быть карта с №39");
         }


     }
}
