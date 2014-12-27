using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Common;
using Arcomage.Core;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests
{
     [TestFixture]
    class GameControllerTest
     {

         //TODO: сделать тест, что нельзя использовать карту

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
             //GameControllerTestHelper.getCards(gm);


             GameControllerTestHelper.useCard(1, gm);

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");

             Assert.AreEqual(gm.Status, CurrentAction.UpdateStatHuman, "Текущий статус должен быть равным обновлению статистики игрока");

             Dictionary<string, object> notify3 = new Dictionary<string, object>();
             notify3.Add("CurrentAction", CurrentAction.EndHumanMove);
             gm.SendGameNotification(notify3);

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
            // gm.GetCard();
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
             /* Стартовые значения игрока 
              * 
              * returnVal.Add(Specifications.PlayerWall, 5);
             returnVal.Add(Specifications.PlayerTower, 10);

             returnVal.Add(Specifications.PlayerMenagerie, 1);
             returnVal.Add(Specifications.PlayerColliery, 1);
             returnVal.Add(Specifications.PlayerDiamondMines, 1);

             returnVal.Add(Specifications.PlayerRocks, 5);
             returnVal.Add(Specifications.PlayerDiamonds, 5);
             returnVal.Add(Specifications.PlayerAnimals, 5);*/

             gm = GameControllerTestHelper.InitDemoGame();
             //GameControllerTestHelper.getCards(gm);

             GameControllerTestHelper.useCard(2, gm);



             var playerParams = gm.GetPlayerParams(SelectPlayer.First);

            
             Assert.AreEqual(playerParams[Specifications.PlayerWall], 5 - 4, "Не правильно применен параметр PlayerWall");
             Assert.AreEqual(playerParams[Specifications.PlayerTower], 10 - 8, "Не правильно применен параметр PlayerTower");
             Assert.AreEqual(playerParams[Specifications.PlayerDiamondMines], 1 + 2, "Не правильно применен параметр PlayerDiamondMines");
             Assert.AreEqual(playerParams[Specifications.PlayerMenagerie], 1 + 3, "Не правильно применен параметр PlayerMenagerie");
             Assert.AreEqual(playerParams[Specifications.PlayerColliery], 1 + 4, "Не правильно применен параметр PlayerColliery");
             Assert.AreEqual(playerParams[Specifications.PlayerDiamonds], 5 + 11 + 3, "Не правильно применен параметр PlayerDiamonds");
             Assert.AreEqual(playerParams[Specifications.PlayerAnimals], 5 + 12 - 5 + 4, "Не правильно применен параметр PlayerAnimals");
             Assert.AreEqual(playerParams[Specifications.PlayerRocks], 5 + 13 + 5, "Не правильно применен параметр PlayerRocks");

             playerParams = gm.GetPlayerParams(SelectPlayer.Second);
             Assert.AreEqual(playerParams[Specifications.PlayerWall], 5 - 4, "Не правильно применен параметр EnemyWall");
             Assert.AreEqual(playerParams[Specifications.PlayerTower], 10 - 8, "Не правильно применен параметр EnemyTower");
             Assert.AreEqual(playerParams[Specifications.PlayerDiamondMines], 1 + 2, "Не правильно применен параметр EnemyDiamondMines");
             Assert.AreEqual(playerParams[Specifications.PlayerMenagerie], 1 + 3, "Не правильно применен параметр EnemyMenagerie");
             Assert.AreEqual(playerParams[Specifications.PlayerColliery], 1 + 4, "Не правильно применен параметр EnemyColliery");
             Assert.AreEqual(playerParams[Specifications.PlayerDiamonds], 5 + 11, "Не правильно применен параметр EnemyDiamonds");
             Assert.AreEqual(playerParams[Specifications.PlayerAnimals], 5 + 12, "Не правильно применен параметр EnemyAnimals");
             Assert.AreEqual(playerParams[Specifications.PlayerRocks], 5 + 13, "Не правильно применен параметр EnemyRocks");
             
         }

         /// <summary>
         /// цель: проверить применение одного параметра
         /// результат: сумма должна быть равна кол-во стартовых кристалов + количество кристалов в карте + прирост шахты кристалов 
         /// </summary>
         [Test]
         public void CheckAddDiamonds()
         {

             gm = GameControllerTestHelper.InitDemoGame(2);

             //GameControllerTestHelper.getCards(gm);

             GameControllerTestHelper.useCard(6, gm);

             var playerParams = gm.GetPlayerParams(SelectPlayer.First);

             Assert.AreEqual(playerParams[Specifications.PlayerDiamonds], 5 + 11 + 1, "Не правильно применен параметр PlayerDiamonds");
         }

         /// <summary>
         /// Цель: проверить, как распределяется прямой урон по врагу
         /// Результат: получить определенные значения башни и стены
         /// </summary>
         [Test]
         public void CheckApplyEnemyDirectDamage()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             //GameControllerTestHelper.getCards(gm);


             GameControllerTestHelper.useCard(3, gm);


             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerWall], 0, "Не правильно применен параметр EnemyDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerTower], 0, "Не правильно применен параметр EnemyDirectDamage");


     
         }

         /// <summary>
         /// Цель: проверить, как распределяется прямой урон по игроку
         /// Результат: получить определенные значения башни и стены
         /// </summary>
         [Test]
         public void CheckApplyPlayerDirectDamage()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             //GameControllerTestHelper.getCards(gm);

             GameControllerTestHelper.useCard(4, gm);


             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerWall], 0,
                 "Не правильно применен параметр PlayerDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerTower], 0,
                 "Не правильно применен параметр PlayerDirectDamage");

         }


         /// <summary>
         /// Цель: проверить,что игрок может получить новую карту, после использования карты с параметром "Взять еще одну карту"
         /// Результат: статус игры должен быть равным "Игрок должен получить карту" 
         /// </summary>
         [Test]
         public void CheckPlayAgain()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             //GameControllerTestHelper.getCards(gm);

             Assert.AreEqual(gm.IsCanUseCard(55), true, "Не возможно использовать карту");

             //перед информацию о том, какую карту использовал игрок
             Dictionary<string, object> notify = new Dictionary<string, object>();
             notify.Add("CurrentAction", CurrentAction.HumanUseCard);
             notify.Add("ID", 55);
             gm.SendGameNotification(notify);


             var result = gm.logCard.Where(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Used).FirstOrDefault();
             Assert.AreEqual(result.card.id, 55, "Human должен был использовать карту 55");

             var result2 = gm.GetPlayerParams();


             Assert.AreEqual(result2[Specifications.PlayerRocks], 7, "Должен быть прирост камней");
             Assert.AreEqual(result2[Specifications.PlayerDiamonds], 7, "Должен быть прирост брилиантов");



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
             //GameControllerTestHelper.getCards(gm);

             GameControllerTestHelper.PassStroke(gm);

             var result = gm.logCard.Where(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Droped).FirstOrDefault();

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
             GameController gm = new GameController(log);
             

             gm.AddPlayer(TypePlayer.Human, "Human");
             gm.AddPlayer(TypePlayer.AI, "AI");


             Dictionary<string, object> notify = new Dictionary<string, object>();
             notify.Add("CurrentAction", CurrentAction.StartGame);
             notify.Add("currentPlayer", TypePlayer.Human); //делаем подтасовку небольшую, чтобы начал свой ход компьютер
             notify.Add("CardTricksters", new List<int> { 39, 11, 12, 13, 14, 15 });
             gm.SendGameNotification(notify);


             Assert.AreEqual(gm.GetPlayersCard().FirstOrDefault(x=>x.id == 39).id, 39, "У игрока должна быть карта с №39");
         }

        
     }
}
