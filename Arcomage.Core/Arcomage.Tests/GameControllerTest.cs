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
             gm = GameControllerTestHelper.InitDemoGame();
         }

         /// <summary>
         /// Цель: Проверка, что игра стартовала
         /// Результат: статус игры должен быть равным "GetPlayerCard" - он означает, что игрок должен получить карты
         /// </summary>
         [Test]
         public void GameIsStarted()
         {
             Assert.AreEqual(gm.status, CurrentAction.GetPlayerCard, "Игра не стартовала");
         }

         /// <summary>
         /// Цель: проверить, что игрок может победить
         /// Результат:  башня противника должна быть уничтожена, в переменной Winner должно храниться имя пользователя 
         /// </summary>
         [Test]
         public void PlayerMustWin()
         {
             GameControllerTestHelper.getCards(gm);


             useCard(1);

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");
             Assert.AreEqual(gm.Winner, "Winner", "Игрок не может проиграть!");

             
         }


         /// <summary>
         /// Цель: Проверка, что человек может использовать карту
         /// Результат: геймконтролер сообщает, что карту возможно использовать
         /// </summary>
         [Test]
         public void PlayerCanUserCard()
         {
             gm.GetCard();
             Assert.AreEqual(gm.IsCanUseCard(1), true, "Не возможно использовать карту");

         }

        

         /// <summary>
         /// Цель: проверить, что при инициализации параметры игрока не пусты
         /// Результат: должны быть значения и количество карты не равно 0
         /// </summary>
         [Test]
         public void CheckPlayerInit()
         {
         
             Assert.IsNotNull(gm.GetPlayerParams(), "Стартовые параметры игрока не должны быть пустыми");



             Assert.IsTrue(gm.GetCard().Count > 1, " Должно быть хотя бы одна карта");
         }


         
         /// <summary>
         /// Цель: проверить как применяются параметры карты к параметрам игроков
         /// Результат: при вычитание или сложение параметров игрока с параметрами карты, должны получить конкретное значение
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


             GameControllerTestHelper.getCards(gm);

             useCard(2);


             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerWall], 5 - 4, "Не правильно применен параметр PlayerWall");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerTower], 10 - 8, "Не правильно применен параметр PlayerTower");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerDiamondMines], 1 + 2, "Не правильно применен параметр PlayerDiamondMines");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerMenagerie], 1 + 3, "Не правильно применен параметр PlayerMenagerie");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerColliery], 1 + 4, "Не правильно применен параметр PlayerColliery");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerDiamonds], 5 + 11, "Не правильно применен параметр PlayerDiamonds");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerAnimals], 5 + 12 - 5, "Не правильно применен параметр PlayerAnimals");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerRocks], 5 + 13, "Не правильно применен параметр PlayerRocks");


             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerWall], 5 - 4, "Не правильно применен параметр EnemyWall");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerTower], 10 - 8, "Не правильно применен параметр EnemyTower");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerDiamondMines], 1 + 2, "Не правильно применен параметр EnemyDiamondMines");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerMenagerie], 1 + 3, "Не правильно применен параметр EnemyMenagerie");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerColliery], 1 + 4, "Не правильно применен параметр EnemyColliery");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerDiamonds], 5 + 11, "Не правильно применен параметр EnemyDiamonds");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerAnimals], 5 + 12, "Не правильно применен параметр EnemyAnimals");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerRocks], 5 + 13, "Не правильно применен параметр EnemyRocks");
             
         }

         /// <summary>
         /// Цель: проверить, как распределяется прямой урон по врагу
         /// Результат: получить определенные значения башни и стены
         /// </summary>
         [Test]
         public void CheckApplyEnemyDirectDamage()
         {
             GameControllerTestHelper.getCards(gm);


             useCard(3);


             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerWall], 0, "Не правильно применен параметр EnemyDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerTower], 0, "Не правильно применен параметр EnemyDirectDamage");


     
         }

         private void useCard(int id)
         {
             Assert.AreEqual(gm.IsCanUseCard(id), true, "Не возможно использовать карту");

             //перед информацию о том, какую карту использовал игрок
             Dictionary<string, object> notify = new Dictionary<string, object>();
             notify.Add("CurrentAction", CurrentAction.HumanUseCard);
             notify.Add("ID", id);
             gm.SendGameNotification(notify);
         }

         /// <summary>
         /// Цель: проверить, как распределяется прямой урон по игроку
         /// Результат: получить определенные значения башни и стены
         /// </summary>
         [Test]
         public void CheckApplyPlayerDirectDamage()
         {
             GameControllerTestHelper.getCards(gm);

             useCard(4);


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
         public void CheckApplyGetNewCard()
         {
             GameControllerTestHelper.getCards(gm);

             useCard(5);


            Assert.AreEqual(gm.status, CurrentAction.GetPlayerCard, "Не правильно применен параметр GetNewCard");
         }

         /// <summary>
         /// Цель: Проверка, что игрок может пропустить ход
         /// Результат: Статус должен сообщить, что текущий ход у противника (AI)
         /// </summary>
         [Test]
         public void PlayerCanPassMove()
         {
             GameControllerTestHelper.getCards(gm);

             GameControllerTestHelper.PassStroke(gm);

         }
     }
}
