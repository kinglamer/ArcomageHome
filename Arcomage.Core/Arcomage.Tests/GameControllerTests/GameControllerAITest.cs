using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
    [TestFixture]
    internal class GameControllerAITest
    {

        [SetUp]
        public void Init()
        {

        }

        private LogTest log = new LogTest();

        /// <summary>
        /// ����: ���������, ��� ��������� ����� ������ ���� 
        /// ���������: ��������� ������ ������������ ����� � �������� ��� ��������
        /// </summary>
        [Test]
        public void AiCanStartTheGame()
        {
            GameBuilder gameBuilder = new GameBuilder(log, new TestServerForCustomCard());

            gameBuilder.AddPlayer(TypePlayer.Human, "Human");
            gameBuilder.AddPlayer(TypePlayer.AI, "AI", null, new List<int> {1});

            GameModel gm = gameBuilder.StartGame(1);


            GameControllerTestHelper.MakeMoveAi(gm, log);
            Assert.AreEqual(gm.GetUsedCard(TypePlayer.AI, GameAction.MakeMove).Count, 1,
                "��������� ������ ������������ �����");

            Assert.AreEqual(gm.CurrentPlayer.type, TypePlayer.AI, "��� ������ �������� �� ������");
        }


        /// <summary>
        /// ����: ���������, ��� ��������� ����������� �����
        /// ���������: ��������� ������ ������������ ����� � id == 1, �.�. �� ������ ������ ��� �������� ��� ��� �������
        /// </summary>
        [Test]
        public void WhichCardUseAi()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1},
                new List<int> {2});
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
            Assert.AreEqual(gm.GetUsedCard(TypePlayer.AI, GameAction.MakeMove).LastOrDefault().id, 2, "��������� ������ ������������ ����� id 2");
        }


        /// <summary>
        /// ����: ���������, ��� ��������� ����� �������� 
        /// ���������: ����� ������ ������ ���� ����������, � ���������� Winner ������ ��������� ��� ���������� (� ��� �� �������� Loser) 
        /// </summary>
        [Test]
        public void ComputerMustWin()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1},
                new List<int> {3});
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0, "����� ����� ������ ���� ����������");
            Assert.AreEqual(gm.Winner, "AI", "��������� �� ����� ���������!");

        }


        /// <summary>
        /// ����: ���������, ��� AI ��������� ��� � ������� �����
        /// ���������: � ���� ������ ���� ������ id ������������ �����
        /// </summary>
        [Test]
        public void AiCanPassMove()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 1, new List<int> {1},
                new List<int> {7});
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
            //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
            Assert.AreEqual(gm.GetUsedCard(TypePlayer.AI, GameAction.DropCard).LastOrDefault().id, 7, "AI ������ �������� ����� 2");
        }


        /// <summary>
        /// ����: ��������� ��� ��������� ������� ������ �����, ����� ������������� ����������� �����
        /// ���������: ������ ���� ����� � ������������ id 
        /// </summary>
        [Test]
        public void AiCanGetAnotherCard()
        {
            //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1},
                new List<int> {56, 6});
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);

            Assert.AreEqual(gm.GetUsedCard(TypePlayer.AI, GameAction.MakeMove).LastOrDefault().id, 6,"AI ������ ��� ������������ ����� 6");

            Assert.AreEqual(gm.GetUsedCard(TypePlayer.AI, GameAction.MakeMove).FirstOrDefault().id, 56,
                "AI ������ ��� ������������ ����� 55");

            Assert.AreEqual(gm.GetUsedCard(TypePlayer.AI, GameAction.MakeMove).Count, 2, "AI ������ ��� ������������ 2 �����");
        }

    }
}
