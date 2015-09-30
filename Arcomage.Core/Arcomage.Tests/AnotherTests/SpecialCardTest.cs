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
         private GameController gm;

       
         [Test]
         public void Card5Test()
         {

             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams(), new TestStartParams(1));

             GameControllerTestHelper.useCard(5, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Colliery], 3, "Должно быть 3 шахты");

             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams2(), new TestStartParams2());
             GameControllerTestHelper.useCard(5, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Colliery], 2, "Должно быть 2 шахты");
         }


 
         [Test]
         public void Card8Test()
         {
             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams(), new TestStartParams(1));
             GameControllerTestHelper.useCard(8, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Colliery], 3, "Должно быть 3 шахты");
         }



         [Test]
         public void Card12Test()
         {
             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams(), new TestStartParams());
             GameControllerTestHelper.useCard(12, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Wall], 6, "Стена должна быть 6");

             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams2(), new TestStartParams2());
             GameControllerTestHelper.useCard(12, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Wall], 8, "Стена должна быть 8");
         }

         [Test]
         public void Card31Test()
         {
             gm = new GameController(log, new TestServer6()); 
             gm.ChangeMaxCard(20);
             AddPlayers(new TestStartParams(), new TestStartParams(1));

             GameControllerTestHelper.useCard(31, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Tower], 3, "Башня должна быть 3");
             Assert.AreEqual(result[Attributes.Menagerie], 2, "Зверинец должен быть равен 2");


             gm = new GameController(log, new TestServer6());
             AddPlayers(new TestStartParams2(), new TestStartParams2());
             GameControllerTestHelper.useCard(31, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Tower], 10, "Башня должна быть 10");
             Assert.AreEqual(result[Attributes.Menagerie], 1, "Зверинец должен быть равен 1");

             result = gm.GetPlayerParams(SelectPlayer.Second);
             Assert.AreEqual(result[Attributes.Tower], 10, "Башня должна быть 10");
             Assert.AreEqual(result[Attributes.Menagerie], 1, "Зверинец должен быть равен 1");
         }


         [Test]
         public void Card32Test()
         {
             Dictionary<string, object> notify = new Dictionary<string, object>();
             notify.Add("CurrentAction", CurrentAction.StartGame);
             notify.Add("currentPlayer", TypePlayer.Human);
          

             gm = new GameController(log, new TestServer6()); 
             gm.ChangeMaxCard(20);

             gm.AddPlayer(TypePlayer.Human, "Human", new TestStartParams3());
             gm.AddPlayer(TypePlayer.AI, "AI", new TestStartParams3(1));
             gm.SendGameNotification(notify);

             GameControllerTestHelper.useCard(32, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Animals], 13, "Зверей должно быть 13");
             Assert.AreEqual(result[Attributes.Wall], 7, "Стена должна быть 7");
             Assert.AreEqual(result[Attributes.Menagerie], 2, "Зверинец должен быть равен 2");



             gm = new GameController(log, new TestServer6());
             gm.ChangeMaxCard(20);

             gm.AddPlayer(TypePlayer.Human, "Human", new TestStartParams2());
             gm.AddPlayer(TypePlayer.AI, "AI", new TestStartParams2());
             gm.SendGameNotification(notify);

             GameControllerTestHelper.useCard(32, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Animals], 12, "Зверей должно быть 12");
             Assert.AreEqual(result[Attributes.Wall], 11, "Стена должна быть 11");
             Assert.AreEqual(result[Attributes.Menagerie], 1, "Зверинец должен быть равен 1");
         }


         [Test]
        public void Card34Test()
        {
            gm = new GameController(log, new TestServer6()); 
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams(), new TestStartParams(1));
            GameControllerTestHelper.useCard(34, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Attributes.Wall], 5, "Стена должна быть 5");
        }


  [Test]
        public void Card48Test()
        {
            gm = new GameController(log, new TestServer6()); 
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams(), new TestStartParams(1));
            GameControllerTestHelper.useCard(48, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Attributes.DiamondMines], 4, "Магия должна быть равна 4");
        }

        [Test]
        public void Card64Test()
        {
            gm = new GameController(log, new TestServer6()); 
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams(), new TestStartParams(1));
            GameControllerTestHelper.useCard(64, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Attributes.Tower], 7, "Башня должна быть равна 7");

            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(64, gm);
            result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Attributes.Tower], 11, "Башня должна быть 11");
        }



        [Test]
        public void Card67Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams(), new TestStartParams(1));
            GameControllerTestHelper.useCard(67, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 9, "Башня должна быть равна 9");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(67, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 5, "Стена должна быть равна 5");
            Assert.AreEqual(result[Attributes.Tower], 2, "Башня должна быть равна 2");

        }



        [Test]
        public void Card87Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3(1));
            GameControllerTestHelper.useCard(87, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 2, "Башня должна быть равна 2");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(87, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 9, "Башня должна быть равна 9");

        }


        [Test]
        public void Card89Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3(1));
            GameControllerTestHelper.useCard(89, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 5, "Башня должна быть равна 5");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(89, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 5, "Башня должна быть равна 5");

        }


        [Test]
        public void Card90Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3());
            GameControllerTestHelper.useCard(90, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 0, "Башня должна быть равна 0");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(90, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 7, "Башня должна быть равна 7");

        }


        [Test]
        public void Card91Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3(1));
            GameControllerTestHelper.useCard(91, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 6, "Башня должна быть равна 6");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(91, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 9, "Башня должна быть равна 9");

        }


        [Test]
        public void Card40Test()
        {
            gm = new GameController(log, new TestServer6()); 
            AddPlayers(new TestStartParams3(), new TestStartParams3());
            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.PassStroke);
            notify.Add("ID", 40);
            gm.SendGameNotification(notify);

            Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "Текущий статус должен быть равным ожиданию хода игрока");
            var result = gm.logCard.LastOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Droped);
            Assert.AreEqual(result, null, "Карта не должна быть сброшена");

        }

        [Test]
        public void Card98Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3(1));
            GameControllerTestHelper.useCard(98, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Attributes.Tower], 9, "Башня должна быть равна 9");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(98, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 3, "Стена должна быть равна 3");
            Assert.AreEqual(result[Attributes.Tower], 10, "Башня должна быть равна 10");

        }


         /// <summary>
         /// Цель: проверить правильность использования карты со сбросом
         /// Результат: игра должна пройти определенные статусы,должен произойти правильный прирост ресурсов
         /// </summary>
        [Test]
        public void CardWithDiscardPlayerTest()
        {
            /* 1.  используешь карту №39 и 73, которая позволяет еще раз сходить
 2. получаешь еще одну карту
 3, сбрасываешь карту
  4, добавляются ресурсы
 5,получаешь еще одну карту
 6, юзаешь карту
 7, добавляются ресурсы*/
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            Assert.AreEqual(gm.GetPlayersCard().Count, 18, "Количество карт должно быть равно 5");
            Assert.AreEqual(gm.IsCanUseCard(39), true, "Не возможно использовать карту");

            //перед информацию о том, какую карту использовал игрок
            Dictionary<string, object> notify2 = new Dictionary<string, object>();
            notify2.Add("CurrentAction", CurrentAction.HumanUseCard);
            notify2.Add("ID", 39);
            gm.SendGameNotification(notify2);

            Assert.AreEqual(gm.Status, CurrentAction.HumanUseCard, "Игрок должен сбросить карту");

            Dictionary<string, object> notify1 = new Dictionary<string, object>();
            notify1.Add("CurrentAction", CurrentAction.AnimateHumanMove);
            gm.SendGameNotification(notify1);


            var result = gm.GetPlayerParams();
            Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "Должны вернуться к ожиданию сброса карты");

            Assert.AreEqual(result[Attributes.Rocks], 5, "Не должно быть прироста ресурсов до сброса");
            Assert.AreEqual(result[Attributes.Diamonds], 5, "Не должно быть прироста ресурсов до сброса");
            Assert.AreEqual(result[Attributes.Animals], 5, "Не должно быть прироста ресурсов до сброса");

            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.PassStroke);
            notify.Add("ID", 5);
            gm.SendGameNotification(notify);


            Assert.AreEqual(result[Attributes.Rocks], 6, "должен быть прирост ресурсов");
            Assert.AreEqual(result[Attributes.Diamonds], 6, "должен быть прирост ресурсов");
            Assert.AreEqual(result[Attributes.Animals], 6, "должен быть прирост ресурсов");

            Assert.AreEqual(gm.GetPlayersCard().Count, 20, "Количество карт должно быть равно 20");

        }


         /// <summary>
         /// Цель: проверить, что компьютер правильно использует карту со сбросом
        /// Результат: должны быть использованы и сброшены определенные карты. Должна обновиться статистика после Статус PlayerMustDropCard изменен на GetAICard
         /// </summary>
        [Test]
        public void CardWithDiscardAITest()
        {

            gm = new GameController(log, new TestServer6()); 
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.PassStroke(gm);

            var result = gm.logCard.Where(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used);

            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            Assert.AreEqual(result.Count(x => x.card.id == 73), 1, "AI должен был использовать карту 73");
            Assert.AreEqual(result.Count(x => x.card.id == 31), 1, "AI должен был использовать карту 31");

            var result2 = gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped);
            Assert.AreEqual(result2.card.id, 1, "AI должен был сбросить карту 1");

       
       } 

        private void AddPlayers(IStartParams humanStat, IStartParams AIStat)
        {
            gm.AddPlayer(TypePlayer.Human, "Human", humanStat);
            gm.AddPlayer(TypePlayer.AI, "AI", AIStat);
            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.StartGame);
            notify.Add("currentPlayer", TypePlayer.Human); //делаем подтасовку небольшую, чтобы начал свой ход человек
            gm.SendGameNotification(notify);
        }
    }
}
