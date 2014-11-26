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
             gm.AddPlayer(TypePlayer.Human, "Loser");

             gm.StartGame();
         }

         [Test]
         public void PlayerMustWin()
         {
       
             gm.GetCard();

             Assert.AreEqual(gm.UseCard(1), true, "Не возможно использовать карту");

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");
             Assert.AreEqual(gm.WhoWin(), "Winner", "Игрок не может проиграть!");
             
         }

         [Test]
         public void ComputerMustWin()
         {
             gm.EndMove();
             gm.GetCard();

             Assert.AreEqual(gm.UseCard(1), true, "Не возможно использовать карту");

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");
             Assert.AreEqual(gm.WhoWin(), "Loser", "Компьютер не может проиграть!");

         }


         [Test]
         public void PlayerCanUserCard()
         {
           

             var card = gm.GetCard();
             
             Assert.AreEqual(gm.UseCard(1), true, "Не возможно использовать карту");

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
             

             gm.GetCard();
             gm.GetCard();

             Assert.AreEqual(gm.UseCard(2), true, "Не возможно использовать карту");



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



         [Test]
         public void CheckApplyNewCardParams()
         {
             for (int i = 0; i < 5; i++)
             {
                 gm.GetCard();
             }



             Assert.AreEqual(gm.UseCard(3), true, "Не возможно использовать карту");


             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerWall], 0, "Не правильно применен параметр EnemyDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Specifications.PlayerTower], 0, "Не правильно применен параметр EnemyDirectDamage");


             Assert.AreEqual(gm.UseCard(4), true, "Не возможно использовать карту");


             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerWall], 0, "Не правильно применен параметр PlayerDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Specifications.PlayerTower], 0, "Не правильно применен параметр PlayerDirectDamage");


             Assert.AreEqual(gm.UseCard(5), true, "Не возможно использовать карту");


             Assert.AreEqual(gm.EndMove(), CurrentAction.GetPlayerCard, "Не правильно применен параметр GetNewCard");

             gm.GetCard();

             Assert.AreEqual(gm.EndMove(), CurrentAction.None, "Должен быть доступен переход хода");
         }

    }
}
