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
       
             var card = gm.GetCard();

             Assert.AreEqual(gm.UseCard(1), true, "Не возможно использовать карту");

             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerTower], 0, "Башня врага должна быть уничтожена");
             Assert.AreEqual(gm.WhoWin(), "Winner", "Игрок не может проиграть!");
             
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
           
             Assert.IsNotNull(gm.players[gm.currentPlayer].Statistic, "Стартовые параметры игрока не должны быть пустыми");

             gm.GetCard();

             Assert.AreEqual(gm.players[gm.currentPlayer].Cards.Count, 1, " Должно быть хотя бы одна карта");
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



             Assert.AreEqual(gm.players[0].Statistic[Specifications.PlayerWall], 5 - 4, "Не правильно применен параметр PlayerWall");
             Assert.AreEqual(gm.players[0].Statistic[Specifications.PlayerTower], 10 - 8, "Не правильно применен параметр PlayerTower");
             Assert.AreEqual(gm.players[0].Statistic[Specifications.PlayerDiamondMines], 1 + 2, "Не правильно применен параметр PlayerDiamondMines");
             Assert.AreEqual(gm.players[0].Statistic[Specifications.PlayerMenagerie], 1 + 3, "Не правильно применен параметр PlayerMenagerie");
             Assert.AreEqual(gm.players[0].Statistic[Specifications.PlayerColliery], 1 + 4, "Не правильно применен параметр PlayerColliery");
             Assert.AreEqual(gm.players[0].Statistic[Specifications.PlayerDiamonds], 5 + 11, "Не правильно применен параметр PlayerDiamonds");
             Assert.AreEqual(gm.players[0].Statistic[Specifications.PlayerAnimals], 5 + 12 - 5, "Не правильно применен параметр PlayerAnimals");
             Assert.AreEqual(gm.players[0].Statistic[Specifications.PlayerRocks], 5 + 13, "Не правильно применен параметр PlayerRocks");


             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerWall], 5 - 4, "Не правильно применен параметр EnemyWall");
             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerTower], 10 - 8, "Не правильно применен параметр EnemyTower");
             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerDiamondMines], 1 + 2, "Не правильно применен параметр EnemyDiamondMines");
             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerMenagerie], 1 + 3, "Не правильно применен параметр EnemyMenagerie");
             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerColliery], 1 + 4, "Не правильно применен параметр EnemyColliery");
             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerDiamonds], 5 + 11, "Не правильно применен параметр EnemyDiamonds");
             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerAnimals], 5 + 12, "Не правильно применен параметр EnemyAnimals");
             Assert.AreEqual(gm.players[1].Statistic[Specifications.PlayerRocks], 5 + 13, "Не правильно применен параметр EnemyRocks");

         


        
         }

    }
}
