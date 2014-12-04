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
         public void GameIsStarted()
         {
             Assert.AreEqual(gm.status, CurrentAction.GetPlayerCard, "Игра не стартовала");
         }

         [Test]
         public void PlayerMustWin()
         {
             getCards();


             useCard(1);

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");
             Assert.AreEqual(gm.WhoWin(), "Winner", "Игрок не может проиграть!");
             
         }



         [Test]
         public void ComputerMustWin()
         {
            // gm.EndMove();
             gm.GetCard();

           //  Assert.AreEqual(gm.UseCard(1), true, "Не возможно использовать карту");

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");
             Assert.AreEqual(gm.WhoWin(), "Loser", "Компьютер не может проиграть!");

         }


         [Test]
         public void PlayerCanUserCard()
         {
             gm.GetCard();
             Assert.AreEqual(gm.IsCanUseCard(1), true, "Не возможно использовать карту");

         }

        

         [Test]
         public void CheckPlayerInit()
         {
         
             Assert.IsNotNull(gm.GetPlayerParams(), "Стартовые параметры игрока не должны быть пустыми");



             Assert.IsTrue(gm.GetCard().Count > 1, " Должно быть хотя бы одна карта");
         }


         

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


             getCards();

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

         private void getCards()
         {
//карты можно получить только, при соответствущем ходе
             Assert.AreEqual(gm.status, CurrentAction.GetPlayerCard, "Сейчас не ход игрока");
             gm.GetCard();

             //после получения карты, игра должно уведомить о том, что игрок все получил и ждем его хода
             Dictionary<string, object> notify1 = new Dictionary<string, object>();
             notify1.Add("CurrentAction", CurrentAction.WaitHumanMove);
             gm.SendGameNotification(notify1);

             Assert.AreEqual(gm.status, CurrentAction.WaitHumanMove, "Должно быть ожидание хода игрока");
         }


         [Test]
         public void CheckApplyEnemyDirectDamage()
         {
             getCards();


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

         [Test]
         public void CheckApplyPlayerDirectDamage()
         {
             getCards();

             useCard(4);


             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerWall], 0,
                 "Не правильно применен параметр PlayerDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerTower], 0,
                 "Не правильно применен параметр PlayerDirectDamage");

         }


         [Test]
         public void CheckApplyGetNewCard()
         {
             getCards();

             useCard(5);
           


            Assert.AreEqual(gm.status, CurrentAction.GetPlayerCard, "Не правильно применен параметр GetNewCard");
               

             //   Assert.AreEqual(gm.EndMove(), CurrentAction.None, "Должен быть доступен переход хода");
         }


         [Test]
         public void PlayerCanPassMove()
         {
             getCards();

             
             Dictionary<string, object> notify = new Dictionary<string, object>();
             notify.Add("CurrentAction", CurrentAction.PassStroke);
             notify.Add("ID", 1);

             gm.SendGameNotification(notify);
             Assert.AreEqual(gm.status, CurrentAction.PassStroke, "Текущий статус должен быть равным сбросу карты");


             Dictionary<string, object> notify2 = new Dictionary<string, object>();
             notify2.Add("CurrentAction", CurrentAction.AnimateHumanMove);
            

             gm.SendGameNotification(notify2);

             Assert.AreEqual(gm.status, CurrentAction.UpdateStatHuman, "Текущий статус должен быть равным обновлению статистики игрока");

             Dictionary<string, object> notify3 = new Dictionary<string, object>();
             notify3.Add("CurrentAction", CurrentAction.EndHumanMove);
             gm.SendGameNotification(notify3);

             Assert.AreEqual(gm.status, CurrentAction.AIUseCardAnimation, "Текущий статус должен быть равным прорисовке хода компьютера");

             Dictionary<string, object> notify4 = new Dictionary<string, object>();
             notify4.Add("CurrentAction", CurrentAction.AIMoveIsAnimated);
             gm.SendGameNotification(notify4);
             
             Assert.AreEqual(gm.status, CurrentAction.UpdateStatAI, "Текущий статус должен быть равным обновлению статистики компьютера");

             Dictionary<string, object> notify5 = new Dictionary<string, object>();
             notify5.Add("CurrentAction", CurrentAction.EndAIMove);
             gm.SendGameNotification(notify5);

             Assert.AreEqual(gm.status, CurrentAction.WaitHumanMove, "Текущий статус должен быть равным ожиданию хода игрока");
         }

     }
}
