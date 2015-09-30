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
    /// ������������ ���������� ����
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
             Assert.AreEqual(result[Attributes.Colliery], 3, "������ ���� 3 �����");

             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams2(), new TestStartParams2());
             GameControllerTestHelper.useCard(5, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Colliery], 2, "������ ���� 2 �����");
         }


 
         [Test]
         public void Card8Test()
         {
             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams(), new TestStartParams(1));
             GameControllerTestHelper.useCard(8, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Colliery], 3, "������ ���� 3 �����");
         }



         [Test]
         public void Card12Test()
         {
             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams(), new TestStartParams());
             GameControllerTestHelper.useCard(12, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Wall], 6, "����� ������ ���� 6");

             gm = new GameController(log, new TestServer6()); 
             AddPlayers(new TestStartParams2(), new TestStartParams2());
             GameControllerTestHelper.useCard(12, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Wall], 8, "����� ������ ���� 8");
         }

         [Test]
         public void Card31Test()
         {
             gm = new GameController(log, new TestServer6()); 
             gm.ChangeMaxCard(20);
             AddPlayers(new TestStartParams(), new TestStartParams(1));

             GameControllerTestHelper.useCard(31, gm);
             var result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Tower], 3, "����� ������ ���� 3");
             Assert.AreEqual(result[Attributes.Menagerie], 2, "�������� ������ ���� ����� 2");


             gm = new GameController(log, new TestServer6());
             AddPlayers(new TestStartParams2(), new TestStartParams2());
             GameControllerTestHelper.useCard(31, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Tower], 10, "����� ������ ���� 10");
             Assert.AreEqual(result[Attributes.Menagerie], 1, "�������� ������ ���� ����� 1");

             result = gm.GetPlayerParams(SelectPlayer.Second);
             Assert.AreEqual(result[Attributes.Tower], 10, "����� ������ ���� 10");
             Assert.AreEqual(result[Attributes.Menagerie], 1, "�������� ������ ���� ����� 1");
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
             Assert.AreEqual(result[Attributes.Animals], 13, "������ ������ ���� 13");
             Assert.AreEqual(result[Attributes.Wall], 7, "����� ������ ���� 7");
             Assert.AreEqual(result[Attributes.Menagerie], 2, "�������� ������ ���� ����� 2");



             gm = new GameController(log, new TestServer6());
             gm.ChangeMaxCard(20);

             gm.AddPlayer(TypePlayer.Human, "Human", new TestStartParams2());
             gm.AddPlayer(TypePlayer.AI, "AI", new TestStartParams2());
             gm.SendGameNotification(notify);

             GameControllerTestHelper.useCard(32, gm);
             result = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(result[Attributes.Animals], 12, "������ ������ ���� 12");
             Assert.AreEqual(result[Attributes.Wall], 11, "����� ������ ���� 11");
             Assert.AreEqual(result[Attributes.Menagerie], 1, "�������� ������ ���� ����� 1");
         }


         [Test]
        public void Card34Test()
        {
            gm = new GameController(log, new TestServer6()); 
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams(), new TestStartParams(1));
            GameControllerTestHelper.useCard(34, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Attributes.Wall], 5, "����� ������ ���� 5");
        }


  [Test]
        public void Card48Test()
        {
            gm = new GameController(log, new TestServer6()); 
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams(), new TestStartParams(1));
            GameControllerTestHelper.useCard(48, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Attributes.DiamondMines], 4, "����� ������ ���� ����� 4");
        }

        [Test]
        public void Card64Test()
        {
            gm = new GameController(log, new TestServer6()); 
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams(), new TestStartParams(1));
            GameControllerTestHelper.useCard(64, gm);
            var result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Attributes.Tower], 7, "����� ������ ���� ����� 7");

            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(64, gm);
            result = gm.GetPlayerParams(SelectPlayer.First);
            Assert.AreEqual(result[Attributes.Tower], 11, "����� ������ ���� 11");
        }



        [Test]
        public void Card67Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams(), new TestStartParams(1));
            GameControllerTestHelper.useCard(67, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 9, "����� ������ ���� ����� 9");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(67, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 5, "����� ������ ���� ����� 5");
            Assert.AreEqual(result[Attributes.Tower], 2, "����� ������ ���� ����� 2");

        }



        [Test]
        public void Card87Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3(1));
            GameControllerTestHelper.useCard(87, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 2, "����� ������ ���� ����� 2");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(87, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 9, "����� ������ ���� ����� 9");

        }


        [Test]
        public void Card89Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3(1));
            GameControllerTestHelper.useCard(89, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 5, "����� ������ ���� ����� 5");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(89, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 5, "����� ������ ���� ����� 5");

        }


        [Test]
        public void Card90Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3());
            GameControllerTestHelper.useCard(90, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 0, "����� ������ ���� ����� 0");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(90, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 7, "����� ������ ���� ����� 7");

        }


        [Test]
        public void Card91Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3(1));
            GameControllerTestHelper.useCard(91, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 6, "����� ������ ���� ����� 6");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(91, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 9, "����� ������ ���� ����� 9");

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

            Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "������� ������ ������ ���� ������ �������� ���� ������");
            var result = gm.logCard.LastOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Droped);
            Assert.AreEqual(result, null, "����� �� ������ ���� ��������");

        }

        [Test]
        public void Card98Test()
        {
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams3(), new TestStartParams3(1));
            GameControllerTestHelper.useCard(98, gm);
            var result = gm.GetPlayerParams(SelectPlayer.Second);
            Assert.AreEqual(result[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(result[Attributes.Tower], 9, "����� ������ ���� ����� 9");


            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.useCard(98, gm);
            result = gm.GetPlayerParams(SelectPlayer.Second);

            Assert.AreEqual(result[Attributes.Wall], 3, "����� ������ ���� ����� 3");
            Assert.AreEqual(result[Attributes.Tower], 10, "����� ������ ���� ����� 10");

        }


         /// <summary>
         /// ����: ��������� ������������ ������������� ����� �� �������
         /// ���������: ���� ������ ������ ������������ �������,������ ��������� ���������� ������� ��������
         /// </summary>
        [Test]
        public void CardWithDiscardPlayerTest()
        {
            /* 1.  ����������� ����� �39 � 73, ������� ��������� ��� ��� �������
 2. ��������� ��� ���� �����
 3, ����������� �����
  4, ����������� �������
 5,��������� ��� ���� �����
 6, ������ �����
 7, ����������� �������*/
            gm = new GameController(log, new TestServer6());
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            Assert.AreEqual(gm.GetPlayersCard().Count, 18, "���������� ���� ������ ���� ����� 5");
            Assert.AreEqual(gm.IsCanUseCard(39), true, "�� �������� ������������ �����");

            //����� ���������� � ���, ����� ����� ����������� �����
            Dictionary<string, object> notify2 = new Dictionary<string, object>();
            notify2.Add("CurrentAction", CurrentAction.HumanUseCard);
            notify2.Add("ID", 39);
            gm.SendGameNotification(notify2);

            Assert.AreEqual(gm.Status, CurrentAction.HumanUseCard, "����� ������ �������� �����");

            Dictionary<string, object> notify1 = new Dictionary<string, object>();
            notify1.Add("CurrentAction", CurrentAction.AnimateHumanMove);
            gm.SendGameNotification(notify1);


            var result = gm.GetPlayerParams();
            Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "������ ��������� � �������� ������ �����");

            Assert.AreEqual(result[Attributes.Rocks], 5, "�� ������ ���� �������� �������� �� ������");
            Assert.AreEqual(result[Attributes.Diamonds], 5, "�� ������ ���� �������� �������� �� ������");
            Assert.AreEqual(result[Attributes.Animals], 5, "�� ������ ���� �������� �������� �� ������");

            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.PassStroke);
            notify.Add("ID", 5);
            gm.SendGameNotification(notify);


            Assert.AreEqual(result[Attributes.Rocks], 6, "������ ���� ������� ��������");
            Assert.AreEqual(result[Attributes.Diamonds], 6, "������ ���� ������� ��������");
            Assert.AreEqual(result[Attributes.Animals], 6, "������ ���� ������� ��������");

            Assert.AreEqual(gm.GetPlayersCard().Count, 20, "���������� ���� ������ ���� ����� 20");

        }


         /// <summary>
         /// ����: ���������, ��� ��������� ��������� ���������� ����� �� �������
        /// ���������: ������ ���� ������������ � �������� ������������ �����. ������ ���������� ���������� ����� ������ PlayerMustDropCard ������� �� GetAICard
         /// </summary>
        [Test]
        public void CardWithDiscardAITest()
        {

            gm = new GameController(log, new TestServer6()); 
            gm.ChangeMaxCard(20);
            AddPlayers(new TestStartParams2(), new TestStartParams2());
            GameControllerTestHelper.PassStroke(gm);

            var result = gm.logCard.Where(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used);

            //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
            Assert.AreEqual(result.Count(x => x.card.id == 73), 1, "AI ������ ��� ������������ ����� 73");
            Assert.AreEqual(result.Count(x => x.card.id == 31), 1, "AI ������ ��� ������������ ����� 31");

            var result2 = gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped);
            Assert.AreEqual(result2.card.id, 1, "AI ������ ��� �������� ����� 1");

       
       } 

        private void AddPlayers(IStartParams humanStat, IStartParams AIStat)
        {
            gm.AddPlayer(TypePlayer.Human, "Human", humanStat);
            gm.AddPlayer(TypePlayer.AI, "AI", AIStat);
            Dictionary<string, object> notify = new Dictionary<string, object>();
            notify.Add("CurrentAction", CurrentAction.StartGame);
            notify.Add("currentPlayer", TypePlayer.Human); //������ ���������� ���������, ����� ����� ���� ��� �������
            gm.SendGameNotification(notify);
        }
    }
}
