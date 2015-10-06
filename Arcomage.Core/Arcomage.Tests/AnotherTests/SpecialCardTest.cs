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

       
         [Test]
         public void Card5Test_1()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 6, new List<int> { 5 });
             gm.MakePlayerMove(5);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 3, "������ ���� 3 �����");
         }


         [Test]
         public void Card5Test_2()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6,new TestStartParams2(), new TestStartParams2(), 6, new List<int> {5});
             gm.MakePlayerMove(5);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 2, "������ ���� 2 �����");
         }



 
         [Test]
         public void Card8Test()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1),6, new List<int> { 8 });
             gm.MakePlayerMove(8);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 3, "������ ���� 3 �����");
         }



         [Test]
         public void Card12Test_1()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(), 6, new List<int> { 12 });
             gm.MakePlayerMove(12);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 6, "����� ������ ���� 6");
         }

         [Test]
         public void Card12Test_2()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 6, new List<int> { 12 });
             gm.MakePlayerMove(12);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 8, "����� ������ ���� 8");
         }


         [Test]
         public void Card31Test_1()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 20);
             gm.MakePlayerMove(31);
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 3, "����� ������ ���� 3");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 2, "�������� ������ ���� ����� 2");
         }


         [Test]
         public void Card31Test_2()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 6, new List<int> { 31 });
             gm.MakePlayerMove(31);

             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 10, "����� ������ ���� 10");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 1, "�������� ������ ���� ����� 1");


             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 10, "����� ������ ���� 10");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Menagerie], 1, "�������� ������ ���� ����� 1");
         }


         [Test]
         public void Card32Test_1()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20, new List<int> { 32 }, new List<int> { 5 });
             gm.MakePlayerMove(32);
             gm.NextPlayerTurn();
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Animals], 13, "������ ������ ���� 13");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 7, "����� ������ ���� 7");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Menagerie], 2, "�������� ������ ���� ����� 2");
         }

         [Test]
         public void Card32Test_2()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20, new List<int> { 32 }, new List<int> { 5 });
             gm.MakePlayerMove(32);
             gm.NextPlayerTurn();
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Animals], 12, "������ ������ ���� 12");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 11, "����� ������ ���� 11");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Menagerie], 1, "�������� ������ ���� ����� 1");
         }


         [Test]
        public void Card34Test()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 20);
            gm.MakePlayerMove(34);
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 5, "����� ������ ���� 5");
        }


        [Test]
        public void Card48Test()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 20);
            gm.MakePlayerMove(48);
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.DiamondMines], 4, "����� ������ ���� ����� 4");
        }

        [Test]
        public void Card64Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams(), new TestStartParams(1), 20);
            gm.MakePlayerMove(64);
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 7, "����� ������ ���� ����� 7");
        }

        [Test]
        public void Card64Test_2()
        {
            GameController gm =  GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            gm.MakePlayerMove(64);
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 11, "����� ������ ���� 11");
        }




        [Test]
        public void Card67Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            gm.MakePlayerMove(67);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 5, "����� ������ ���� ����� 5");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 2, "����� ������ ���� ����� 2");
        }

        [Test]
        public void Card67Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            gm.MakePlayerMove(67);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 5, "����� ������ ���� ����� 5");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 2, "����� ������ ���� ����� 2");
        }



        [Test]
        public void Card87Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20);
            gm.MakePlayerMove(87);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 2, "����� ������ ���� ����� 2");
        }

        [Test]
        public void Card87Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            gm.MakePlayerMove(87);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 9, "����� ������ ���� ����� 9");
        }



        [Test]
        public void Card89Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20);
            gm.MakePlayerMove(89);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 5, "����� ������ ���� ����� 5");
        }

        [Test]
        public void Card89Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            gm.MakePlayerMove(89);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 5, "����� ������ ���� ����� 5");
        }


        [Test]
        public void Card90Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(), 20);
            gm.MakePlayerMove(90);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0, "����� ������ ���� ����� 0");
        }

        [Test]
        public void Card90Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            gm.MakePlayerMove(90);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 7, "����� ������ ���� ����� 7");
        }


        [Test]
        public void Card91Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20);
            gm.MakePlayerMove(91);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 6, "����� ������ ���� ����� 6");
        }

        [Test]
        public void Card91Test_2()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            gm.MakePlayerMove(91);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 9, "����� ������ ���� ����� 9");
        }


        [Test]
        public void Card40Test()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(), 6, new List<int> { 40 });
            gm.MakePlayerMove(40, true);
           // Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "������� ������ ������ ���� ������ �������� ���� ������");
            var result = gm.LogCard.LastOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Droped);
            Assert.AreEqual(result, null, "����� �� ������ ���� ��������");
        }

        [Test]
        public void Card98Test_1()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams3(), new TestStartParams3(1), 20);
            gm.MakePlayerMove(98);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "����� ������ ���� ����� 0");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 9, "����� ������ ���� ����� 9");
        }

        [Test]
        public void Card98Tes_2t()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            gm.MakePlayerMove(98);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 3, "����� ������ ���� ����� 3");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 10, "����� ������ ���� ����� 10");

        }


         /// <summary>
         /// ����: ��������� ������������ ������������� ����� �� �������
         /// ���������: ���� ������ ������ ������������ �������,������ ��������� ���������� ������� ��������
         /// </summary>
        [Test]
        public void CardWithDiscardPlayerTest()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20);
            Assert.AreEqual(gm.CurrentPlayer.Cards.Count, 18, "���������� ���� ������ ���� ����� 18");
            Assert.AreEqual(gm.CanUseCard(39), true, "�� �������� ������������ �����");
 
            gm.MakePlayerMove(39);

            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 5, "�� ������ ���� �������� �������� �� ������");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 5, "�� ������ ���� �������� �������� �� ������");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Animals], 5, "�� ������ ���� �������� �������� �� ������");
     
            gm.MakePlayerMove(5, true);

            gm.NextPlayerTurn();

            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Rocks], 6, "������ ���� ������� ��������");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Diamonds], 6, "������ ���� ������� ��������");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Animals], 6, "������ ���� ������� ��������");

            Assert.AreEqual(gm.EnemyPlayer.Cards.Count, 20, "���������� ���� ������ ���� ����� 20");
        }


         /// <summary>
         /// ����: ���������, ��� ��������� ��������� ���������� ����� �� �������
        /// ���������: ������ ���� ������������ � �������� ������������ �����. ������ ���������� ���������� ����� ������ PlayerMustDropCard ������� �� GetAICard
         /// </summary>
        [Test]
        public void CardWithDiscardAiTest()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(6, new TestStartParams2(), new TestStartParams2(), 20, new List<int> { 1 }, new List<int> { 73, 1,31 });
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
          
            //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
            Assert.AreEqual(gm.LogCard.Count(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used && x.card.id == 73), 1, "AI ������ ��� ������������ ����� 73");
            Assert.AreEqual(gm.LogCard.Count(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used && x.card.id == 31), 1, "AI ������ ��� ������������ ����� 31");

            var result2 = gm.LogCard.FirstOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped);
            Assert.AreEqual(result2.card.id, 1, "AI ������ ��� �������� ����� 1");
       }
    }
}
