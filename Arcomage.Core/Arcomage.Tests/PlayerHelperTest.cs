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
    class PlayerHelperTest
    {

         LogTest log = new LogTest();

         [Test]
         public void PlayerMustWin()
         {
            
            /* Player ph = new Player(log, "Winner", new TestServer());

             Player en = new Player(log, "Loser");
             ph.SetTheEnemy(en);


             var card = ph.GetCard();

             Assert.AreEqual(ph.UseCard(1), true, "Не возможно использовать карту");

             Assert.AreEqual(ph.IsPlayerWin(),null, "Игрок не может проиграть!");
             Assert.AreEqual(en.IsPlayerWin(), false, "Компьютер должен проиграть!");*/
         }


         [Test]
         public void PlayerCanUserCard()
         {
            /* Player ph = new Player(log, "Winner", new TestServer());
             Player en = new Player(log, "Loser");
             ph.SetTheEnemy(en);

             ph.GetCard();

             Assert.AreEqual(ph.UseCard(1), true, "Не возможно использовать карту");*/
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

           /*  Player ph = new Player(log, "Winner", new TestServer());
             Player en = new Player(log, "Loser");
             ph.SetTheEnemy(en);

             ph.GetCard();
             ph.GetCard();


             Assert.AreEqual(ph.UseCard(2), true, "Не возможно использовать карту");

               

             Assert.AreEqual(ph.GetPlayerStat(Specifications.PlayerWall), 5 - 4, "Не правильно применен параметр PlayerWall");
             Assert.AreEqual(ph.GetPlayerStat(Specifications.PlayerTower),10 -8, "Не правильно применен параметр PlayerTower");
             Assert.AreEqual(ph.GetPlayerStat(Specifications.PlayerDiamondMines), 1 + 2, "Не правильно применен параметр PlayerDiamondMines");
             Assert.AreEqual(ph.GetPlayerStat(Specifications.PlayerMenagerie), 1 + 3, "Не правильно применен параметр PlayerMenagerie");
             Assert.AreEqual(ph.GetPlayerStat(Specifications.PlayerColliery), 1 + 4, "Не правильно применен параметр PlayerColliery");
             Assert.AreEqual(ph.GetPlayerStat(Specifications.PlayerDiamonds), 5 + 11, "Не правильно применен параметр PlayerDiamonds");
             Assert.AreEqual(ph.GetPlayerStat(Specifications.PlayerAnimals), 5 + 12 - 5, "Не правильно применен параметр PlayerAnimals");
             Assert.AreEqual(ph.GetPlayerStat(Specifications.PlayerRocks), 5 + 13, "Не правильно применен параметр PlayerRocks");


             Assert.AreEqual(en.GetPlayerStat(Specifications.PlayerWall), 5 - 4, "Не правильно применен параметр EnemyWall");
             Assert.AreEqual(en.GetPlayerStat(Specifications.PlayerTower), 10 - 8, "Не правильно применен параметр EnemyTower");
             Assert.AreEqual(en.GetPlayerStat(Specifications.PlayerDiamondMines), 1 + 2, "Не правильно применен параметр EnemyDiamondMines");
             Assert.AreEqual(en.GetPlayerStat(Specifications.PlayerMenagerie), 1 + 3, "Не правильно применен параметр EnemyMenagerie");
             Assert.AreEqual(en.GetPlayerStat(Specifications.PlayerColliery), 1 + 4, "Не правильно применен параметр EnemyColliery");
             Assert.AreEqual(en.GetPlayerStat(Specifications.PlayerDiamonds), 5 + 11, "Не правильно применен параметр EnemyDiamonds");
             Assert.AreEqual(en.GetPlayerStat(Specifications.PlayerAnimals), 5 + 12, "Не правильно применен параметр EnemyAnimals");
             Assert.AreEqual(en.GetPlayerStat(Specifications.PlayerRocks), 5 + 13, "Не правильно применен параметр EnemyRocks");*/

         


        
         }

    }
}
