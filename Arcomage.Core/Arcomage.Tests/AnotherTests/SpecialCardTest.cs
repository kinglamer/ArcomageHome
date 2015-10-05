using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Entity;
using Arcomage.Entity.Interfaces;
using Arcomage.Tests.GameControllerTests;
using Arcomage.Tests.Moq;
using Arcomage.Tests.MoqStartParams;
using NUnit.Framework;

namespace Arcomage.Tests.AnotherTests
{
    /// <summary>
    /// Тестирование уникальных карт
    /// </summary>
     [TestFixture]
    class SpecialCardTest
    {
         [SetUp]
         public void Init()
         {

         }

         LogTest log = new LogTest();

       
         [Test]
         public void Card5Test_1()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6,new TestStartParams(), new TestStartParams(1));
             GameControllerTestHelper.UseCard(5, gm);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 3, "Должно быть 3 шахты");
         }


         [Test]
         public void Card5Test_2()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6,new TestStartParams2(), new TestStartParams2());
             GameControllerTestHelper.UseCard(5, gm);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 2, "Должно быть 2 шахты");
         }



 
         [Test]
         public void Card8Test()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1),6, new List<int> { 8 });
             GameControllerTestHelper.UseCard(8, gm);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 3, "Должно быть 3 шахты");
         }



         [Test]
         public void Card12Test_1()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(), 6, new List<int> { 12 });
             GameControllerTestHelper.UseCard(12, gm);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 6, "Стена должна быть 6");
         }

         [Test]
         public void Card12Test_2()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 6, new List<int> { 12 });
             GameControllerTestHelper.UseCard(12, gm);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 8, "Стена должна быть 8");
         }


         [Test]
         public void Card31Test_1()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 20);
             GameControllerTestHelper.UseCard(31, gm);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 3, "Башня должна быть 3");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 2, "Зверинец должен быть равен 2");
         }


         [Test]
         public void Card31Test_2()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 6, new List<int> { 31 });
             GameControllerTestHelper.UseCard(31, gm);

             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 10, "Башня должна быть 10");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 1, "Зверинец должен быть равен 1");


             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 10, "Башня должна быть 10");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Menagerie], 1, "Зверинец должен быть равен 1");
         }


         [Test]
         public void Card32Test()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20, new List<int> { 32 });
             GameControllerTestHelper.UseCard(32, gm);
             gm.NextPlayerTurn();

             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Animals], 13, "Зверей должно быть 13");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 7, "Стена должна быть 7");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 2, "Зверинец должен быть равен 2");
         }

         [Test]
         public void Card32Test_2()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20, new List<int> { 32 });
             GameControllerTestHelper.UseCard(32, gm);
             gm.NextPlayerTurn();
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Animals], 12, "Зверей должно быть 12");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 11, "Стена должна быть 11");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 1, "Зверинец должен быть равен 1");
         }


         [Test]
        public void Card34Test()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 20);
            GameControllerTestHelper.UseCard(34, gm);
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 5, "Стена должна быть 5");
        }


        [Test]
        public void Card48Test()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 20);
            GameControllerTestHelper.UseCard(48, gm);
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.DiamondMines], 4, "Магия должна быть равна 4");
        }

        [Test]
        public void Card64Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 20);
            GameControllerTestHelper.UseCard(64, gm);
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 7, "Башня должна быть равна 7");
        }

        [Test]
        public void Card64Test_2()
        {
            GameController gm =  GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            GameControllerTestHelper.UseCard(64, gm);
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 11, "Башня должна быть 11");
        }




        [Test]
        public void Card67Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            GameControllerTestHelper.UseCard(67, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 5, "Стена должна быть равна 5");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 2, "Башня должна быть равна 2");
        }

        [Test]
        public void Card67Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            GameControllerTestHelper.UseCard(67, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 5, "Стена должна быть равна 5");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 2, "Башня должна быть равна 2");
        }



        [Test]
        public void Card87Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20);
            GameControllerTestHelper.UseCard(87, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 2, "Башня должна быть равна 2");
        }

        [Test]
        public void Card87Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            GameControllerTestHelper.UseCard(87, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 9, "Башня должна быть равна 9");
        }



        [Test]
        public void Card89Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20);
            GameControllerTestHelper.UseCard(89, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 5, "Башня должна быть равна 5");
        }

        [Test]
        public void Card89Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            GameControllerTestHelper.UseCard(89, gm);

            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 5, "Башня должна быть равна 5");
        }


        [Test]
        public void Card90Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(), 20);
            GameControllerTestHelper.UseCard(90, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0, "Башня должна быть равна 0");
        }

        [Test]
        public void Card90Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            GameControllerTestHelper.UseCard(90, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 7, "Башня должна быть равна 7");
        }


        [Test]
        public void Card91Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20);
            GameControllerTestHelper.UseCard(91, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 6, "Башня должна быть равна 6");
        }

        [Test]
        public void Card91Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            GameControllerTestHelper.UseCard(91, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 9, "Башня должна быть равна 9");
        }


        [Test]
        public void Card40Test()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(), 6, new List<int> { 40 });
            gm.MakePlayerMove(40, true);
           // Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "Текущий статус должен быть равным ожиданию хода игрока");
            var result = gm.logCard.LastOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Droped);
            Assert.AreEqual(result, null, "Карта не должна быть сброшена");
        }

        [Test]
        public void Card98Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20);
            GameControllerTestHelper.UseCard(98, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 9, "Башня должна быть равна 9");
        }

        [Test]
        public void Card98Tes_2t()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            GameControllerTestHelper.UseCard(98, gm);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 3, "Стена должна быть равна 3");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 10, "Башня должна быть равна 10");

        }


         /// <summary>
         /// Цель: проверить правильность использования карты со сбросом
         /// Результат: игра должна пройти определенные статусы,должен произойти правильный прирост ресурсов
         /// </summary>
        [Test]
        public void CardWithDiscardPlayerTest()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            Assert.AreEqual(gm.CurrentPlayer.Cards.Count, 18, "Количество карт должно быть равно 18");
            Assert.AreEqual(gm.IsCanUseCard(39), true, "Не возможно использовать карту");
 
            gm.MakePlayerMove(39);

           // Assert.AreEqual(gm.Status, CurrentAction.HumanUseCard, "Игрок должен сбросить карту");


          //  gm.SendGameNotification(new Dictionary<string, object>() {{"CurrentAction", CurrentAction.AnimateHumanMove}});


          //  var result = gm.GetPlayerParams();
           // Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "Должны вернуться к ожиданию сброса карты");

            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 5, "Не должно быть прироста ресурсов до сброса");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 5, "Не должно быть прироста ресурсов до сброса");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Animals], 5, "Не должно быть прироста ресурсов до сброса");
     
            gm.MakePlayerMove(5, true);

            gm.NextPlayerTurn();

            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 6, "должен быть прирост ресурсов");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 6, "должен быть прирост ресурсов");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Animals], 6, "должен быть прирост ресурсов");

            Assert.AreEqual(gm.CurrentPlayer.Cards.Count, 20, "Количество карт должно быть равно 20");
        }


         /// <summary>
         /// Цель: проверить, что компьютер правильно использует карту со сбросом
        /// Результат: должны быть использованы и сброшены определенные карты. Должна обновиться статистика после Статус PlayerMustDropCard изменен на GetAICard
         /// </summary>
        [Test]
        public void CardWithDiscardAITest()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20, null, new List<int> { 73,31 });
            GameControllerTestHelper.PassStroke(gm);
            gm.NextPlayerTurn();
            var result = gm.logCard.Where(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used);

            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            Assert.AreEqual(result.Count(x => x.card.id == 73), 1, "AI должен был использовать карту 73");
            Assert.AreEqual(result.Count(x => x.card.id == 31), 1, "AI должен был использовать карту 31");

            var result2 = gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped);
            Assert.AreEqual(result2.card.id, 1, "AI должен был сбросить карту 1");
       }
    }
}
