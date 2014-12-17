using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using Arcomage.Tests.MoqStartParams;
using NUnit.Framework;

namespace Arcomage.Tests
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

         /// <summary>
         /// Цель: проверить, что правильно отработала специальная карта
         /// Результат: проверка измененных атрибутов, в двух различных ситуациях
         /// </summary>
         [Test]
         public void Card5Test()
         {
             gm =  new GameController(log, new TestServer6(), new TestStartParams());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(5, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerColliery], 3, "Должно быть 3 шахты");

             gm = new GameController(log, new TestServer6(), new TestStartParams2());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(5, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerColliery], 2, "Должно быть 2 шахты");
         }


         /// <summary>
         /// Цель: проверить, что правильно отработала специальная карта
         /// Результат: проверка измененных атрибутов
         /// </summary>
         [Test]
         public void Card8Test()
         {
             gm = new GameController(log, new TestServer6(), new TestStartParams());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(8, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerColliery], 3, "Должно быть 3 шахты");
         }


         /// <summary>
         /// Цель: проверить, что правильно отработала специальная карта
         /// Результат: проверка измененных атрибутов
         /// </summary>
         [Test]
         public void Card12Test()
         {
             gm = new GameController(log, new TestServer6(), new TestStartParams());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(12, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerWall], 6, "Стена должна быть 6");

             gm = new GameController(log, new TestServer6(), new TestStartParams2());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(12, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerWall], 8, "Стена должна быть 8");
         }

         /// <summary>
         /// Цель: проверить, что правильно отработала специальная карта
         /// Результат: проверка измененных атрибутов
         /// </summary>
         [Test]
         public void Card31Test()
         {
             gm = new GameController(log, new TestServer6(), new TestStartParams());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(31, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerTower], 3, "Башня должна быть 3");
             Assert.AreEqual(result[Specifications.PlayerMenagerie], 2, "Зверинец должен быть равен 2");


             gm = new GameController(log, new TestServer6(), new TestStartParams2());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(31, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerTower], 10, "Башня должна быть 10");
             Assert.AreEqual(result[Specifications.PlayerMenagerie], 1, "Зверинец должен быть равен 1");

             result = gm.GetPlayerParams(SelectPlayer.Second);
             Assert.AreEqual(result[Specifications.PlayerTower], 10, "Башня должна быть 10");
             Assert.AreEqual(result[Specifications.PlayerMenagerie], 1, "Зверинец должен быть равен 1");
         }


         /// <summary>
         /// Цель: проверить, что правильно отработала специальная карта
         /// Результат: проверка измененных атрибутов
         /// </summary>
         [Test]
         public void Card32Test()
         {
             gm = new GameController(log, new TestServer6(), new TestStartParams3());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(32, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerAnimals], 13, "Зверей должно быть 13");
             Assert.AreEqual(result[Specifications.PlayerWall], 7, "Стена должна быть 7");
             Assert.AreEqual(result[Specifications.PlayerMenagerie], 2, "Зверинец должен быть равен 2");


             gm = new GameController(log, new TestServer6(), new TestStartParams2());
             AddPlayers();
             GameControllerTestHelper.getCards(gm);
             GameControllerTestHelper.useCard(32, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Specifications.PlayerAnimals], 12, "Зверей должно быть 11");
             Assert.AreEqual(result[Specifications.PlayerWall], 11, "Стена должна быть 6");
             Assert.AreEqual(result[Specifications.PlayerMenagerie], 1, "Зверинец должен быть равен 1");
         }


        /// <summary>
        /// Цель: проверить, что правильно отработала специальная карта
        /// Результат: проверка измененных атрибутов
        /// </summary>
        [Test]
        public void Card34Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(34, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Specifications.PlayerWall], 5, "Башня должна быть 5");
        }


        /// <summary>
        /// Цель: проверить, что правильно отработала специальная карта
        /// Результат: проверка измененных атрибутов
        /// </summary>
        [Test]
        public void Card48Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(48, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Specifications.PlayerDiamondMines], 4, "Магия должна быть равна 4");
        }

        [Test]
        public void Card64Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(64, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Specifications.PlayerTower], 7, "Башня должна быть равна 7");

            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(64, gm);
            result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Specifications.PlayerTower], 11, "Башня должна быть 11");
        }



        [Test]
        public void Card67Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(67, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 9, "Башня должна быть равна 9");


            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(67, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Specifications.PlayerWall], 5, "Стена должна быть равна 5");
            Assert.AreEqual(result[Specifications.PlayerTower], 2, "Башня должна быть равна 2");

        }



        [Test]
        public void Card87Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams3());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(87, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 2, "Башня должна быть равна 2");


            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(87, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 9, "Башня должна быть равна 9");

        }


        [Test]
        public void Card89Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams3());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(89, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 5, "Башня должна быть равна 5");


            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(89, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 5, "Башня должна быть равна 5");

        }


        [Test]
        public void Card90Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams3());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(90, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 0, "Башня должна быть равна 0");


            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(90, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 7, "Башня должна быть равна 7");

        }


        [Test]
        public void Card91Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams3());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(91, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 6, "Башня должна быть равна 6");


            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(91, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 9, "Башня должна быть равна 9");

        }


        [Test]
        public void Card40Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams3());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.PassStroke);
            notify.Add("ID", 40);
            gm.SendGameNotification(notify);

            Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "Текущий статус должен быть равным ожиданию хода игрока");

            var result = gm.logCard.Where(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Droped).LastOrDefault();

            Assert.AreEqual(result, null, "Карта не должна быть сброшена");

        }

        [Test]
        public void Card98Test()
        {
            gm = new GameController(log, new TestServer6(), new TestStartParams3());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(98, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Specifications.PlayerWall], 0, "Стена должна быть равна 0");
            Assert.AreEqual(result[Specifications.PlayerTower], 9, "Башня должна быть равна 9");


            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(98, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Specifications.PlayerWall], 3, "Стена должна быть равна 3");
            Assert.AreEqual(result[Specifications.PlayerTower], 10, "Башня должна быть равна 10");

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
            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.useCard(39, gm);

            Assert.AreEqual(gm.Status, CurrentAction.PlayerMustDropCard, "Игрок должен сбросить карту");

            gm.GetCard();

            var result = gm.GetPlayerParams();


            Assert.AreEqual(result[Specifications.PlayerRocks], 5, "Не должно быть прироста ресурсов до сброса");
               Assert.AreEqual(result[Specifications.PlayerDiamonds], 5, "Не должно быть прироста ресурсов до сброса");
               Assert.AreEqual(result[Specifications.PlayerAnimals], 5, "Не должно быть прироста ресурсов до сброса");

            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.PassStroke);
            notify.Add("ID", 5);
            gm.SendGameNotification(notify);


            Assert.AreEqual(result[Specifications.PlayerRocks], 6, "должен быть прирост ресурсов");
            Assert.AreEqual(result[Specifications.PlayerDiamonds], 6, "должен быть прирост ресурсов");
            Assert.AreEqual(result[Specifications.PlayerAnimals], 6, "должен быть прирост ресурсов");

            Assert.AreEqual(gm.Status, CurrentAction.GetPlayerCard, "Игрок должен взять еще одну карту");

            GameControllerTestHelper.getCards(gm);
        }


         /// <summary>
         /// Цель: проверить, что компьютер правильно использует карту со сбросом
        /// Результат: должны быть использованы и сброшены определенные карты. Должна обновиться статистика после Статус PlayerMustDropCard изменен на GetAICard
         /// </summary>
        [Test]
        public void CardWithDiscardAITest()
        {

            gm = new GameController(log, new TestServer6(), new TestStartParams2());
            AddPlayers();
            GameControllerTestHelper.getCards(gm);
            GameControllerTestHelper.PassStroke(gm);

            var result = gm.logCard.Where(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used);

            //Внимание: при усовершенствование AI данный тест может измениться, .т.к. комп уже осознано будет выбирать какую карту сбросить
            Assert.AreEqual(result.Where(x=>x.card.id == 73).Count(), 1, "AI должен был использовать карту 73");
            Assert.AreEqual(result.Where(x => x.card.id == 8).Count(), 1, "AI должен был использовать карту 73");

            var result2 = gm.logCard.Where(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped).FirstOrDefault();
            Assert.AreEqual(result2.card.id, 5, "AI должен был сбросить карту 5");

       
        }

        private void AddPlayers()
        {
            gm.AddPlayer(TypePlayer.Human, "Human");
            gm.AddPlayer(TypePlayer.AI, "AI");
            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.StartGame);
            notify.Add("currentPlayer", TypePlayer.Human); //делаем подтасовку небольшую, чтобы начал свой ход человек
            gm.SendGameNotification(notify);
        }
    }
}
