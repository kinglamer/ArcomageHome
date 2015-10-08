using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
    [TestFixture]
    internal class GameControllerTest
    {


        [SetUp]
        public void Init()
        {

        }

 
        /// <summary>
        /// ����: ���������, ��� ����� ����� ��������
        /// ���������:  ����� ���������� ������ ���� ����������, � ���������� Winner ������ ��������� ��� ������������ 
        /// </summary>
        [Test]
        public void PlayerMustWin()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1});
            gm.MakePlayerMove(1);
            gm.NextPlayerTurn();
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0, "����� ����� ������ ���� ����������");
            Assert.AreEqual(gm.Winner, "Human", "����� �� ����� ���������!");
        }

        /// <summary>
        /// ����: ��������, ��� ������� ����� ������������ �����
        /// ���������: ������������� ��������, ��� ����� �������� ������������
        /// </summary>
        [Test]
        public void PlayerCanUserCard()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1});
            Assert.AreEqual(gm.CanUseCard(1), true, "�� �������� ������������ �����");
        }


        /// <summary>
        /// ����: ���������, ��� ��� ������������� ��������� ������ �� �����
        /// ���������: ������ ���� �������� � ���������� ����� �� ����� 0
        /// </summary>
        [Test]
        public void CheckPlayerInit()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame();
            Assert.IsNotNull(gm.CurrentPlayer.PlayerParams, "��������� ��������� ������ �� ������ ���� �������");
            Assert.IsTrue(gm.CurrentPlayer.Cards.Count > 1, " ������ ���� ���� �� ���� �����");
        }

        /// <summary>
        /// ����: ��������� ��� ����������� ��������� ����� � ���������� �������
        /// ���������: ��� ��������� ��� �������� ���������� ������ � ����������� �����, ������ �������� ���������� ��������
        /// ��� �������� - ����� ������ ���� ����� ���-�� ��������� ��������� + ���������� ��������� � ����� + ������� ����� ��������� . � ����� �� ���������� ��� ���� ����� �������� ��������� �����
        /// ��� ���������� - ������ ��������� �������� + ��������� ����� 
        /// </summary>
        [Test]
        public void CheckApplyCardParams()
        {
            GameBuilder gameBuilder = new GameBuilder(new LogTest(), new TestServerForCustomCard());
            gameBuilder.AddPlayer(TypePlayer.Human, "Human", null, new List<int> { 2 });
            gameBuilder.AddPlayer(TypePlayer.Human, "AI");
            GameModel gm = gameBuilder.StartGame(0);

            gm.MakePlayerMove(2);
            gm.NextPlayerTurn();

            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 5 - 4,
                "�� ��������� �������� �������� PlayerWall");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 10 - 8,
                "�� ��������� �������� �������� PlayerTower");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.DiamondMines], 1 + 2,
                "�� ��������� �������� �������� PlayerDiamondMines");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Menagerie], 1 + 3,
                "�� ��������� �������� �������� PlayerMenagerie");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Colliery], 1 + 4,
                "�� ��������� �������� �������� PlayerColliery");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Diamonds], 5 + 11 + 3,
                "�� ��������� �������� �������� PlayerDiamonds");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Animals], 5 + 12 + 4 - 5,
                "�� ��������� �������� �������� PlayerAnimals");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Rocks], 5 + 13 + 5,
                "�� ��������� �������� �������� PlayerRocks");

            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 5 - 4,
                "�� ��������� �������� �������� EnemyWall");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 10 - 8,
                "�� ��������� �������� �������� EnemyTower");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.DiamondMines], 1 + 2,
                "�� ��������� �������� �������� EnemyDiamondMines");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 1 + 3,
                "�� ��������� �������� �������� EnemyMenagerie");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 1 + 4,
                "�� ��������� �������� �������� EnemyColliery");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 5 + 11,
                "�� ��������� �������� �������� EnemyDiamonds");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Animals], 5 + 12,
                "�� ��������� �������� �������� EnemyAnimals");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 5 + 13,
                "�� ��������� �������� �������� EnemyRocks");

        }

        /// <summary>
        /// ����: ��������� ���������� ������ ���������
        /// ���������: ����� ������ ���� ����� ���-�� ��������� ��������� + ���������� ��������� � ����� + ������� ����� ��������� 
        /// </summary>
        [Test]
        public void CheckAddDiamonds()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {6});
            gm.MakePlayerMove(6);
            gm.NextPlayerTurn();
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Diamonds], 5 + 11 + 1,
                "�� ��������� �������� �������� PlayerDiamonds");
        }

        /// <summary>
        /// ����: ���������, ��� �������������� ������ ���� �� �����
        /// ���������: �������� ������������ �������� ����� � �����
        /// </summary>
        [Test]
        public void CheckApplyEnemyDirectDamage()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {3});
            gm.MakePlayerMove(3);
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0,
                "�� ��������� �������� �������� EnemyDirectDamage");
            Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0,
                "�� ��������� �������� �������� EnemyDirectDamage");

        }

        /// <summary>
        /// ����: ���������, ��� �������������� ������ ���� �� ������
        /// ���������: �������� ������������ �������� ����� � �����
        /// </summary>
        [Test]
        public void CheckApplyPlayerDirectDamage()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {4});
            gm.MakePlayerMove(4);

            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 0,
                "�� ��������� �������� �������� PlayerDirectDamage");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 0,
                "�� ��������� �������� �������� PlayerDirectDamage");
        }


        /// <summary>
        /// ����: ���������,��� ����� ����� �������� ����� �����, ����� ������������� ����� � ���������� "����� ��� ���� �����"
        /// ���������: ������ ���� ������ ���� ������ "����� ������ �������� �����" 
        /// </summary>
        [Test]
        public void CheckPlayAgain()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {55});
            Assert.AreEqual(gm.CanUseCard(55), true, "�� �������� ������������ �����");


            gm.MakePlayerMove(55);
            gm.NextPlayerTurn();
            Assert.AreEqual(
                gm.LogCard.FirstOrDefault(x => x.Player.type == TypePlayer.Human && x.GameAction == GameAction.MakeMove)
                    .Card.id, 55, "Human ������ ��� ������������ ����� 55");


            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 5, "�������� ������ �� ������ ����");
            Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 6, "�������� ���������� �� ������ ����");
            Assert.AreEqual(gm.CurrentPlayer.gameActions.Contains(GameAction.MakeMoveAgain), true,
                "������� ����� ������������ ��� ���� �����");
        }

        /// <summary>
        /// ����: ��������, ��� ����� ����� ���������� ���
        /// ���������: ������ ������ ��������, ��� ������� ��� � ���������� (AI)
        /// </summary>
        [Test]
        public void PlayerCanPassMove()
        {
            GameModel gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> {1});
            gm.MakePlayerMove(1, true);
            var result =
                gm.LogCard.FirstOrDefault(x => x.Player.type == TypePlayer.Human && x.GameAction == GameAction.DropCard);

            Assert.AreEqual(result.Card.id, 1, "Human ������ �������� ����� 1");

        }

        /// <summary>
        /// ����: ���������, ��� �������� ������� ���������� ����
        /// ���������: � ���� ������ ���� ������������ �����
        /// </summary>
        [Test]
        public void TestCardTricker()
        {
            LogTest log = new LogTest();
            GameBuilder gameBuilder = new GameBuilder(log, new ArcoSQLLiteServer(@"arcomageDB.db"));
            gameBuilder.AddPlayer(TypePlayer.Human, "Human", null, new List<int> { 39, 11, 12, 13, 14, 15 });
            gameBuilder.AddPlayer(TypePlayer.AI, "AI");


            GameModel gm = gameBuilder.StartGame(0);
            Assert.AreEqual(gm.CurrentPlayer.Cards.FirstOrDefault(x => x.id == 39).id, 39,
                "� ������ ������ ���� ����� � �39");
        }
    }
}
