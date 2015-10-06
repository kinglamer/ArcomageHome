using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
    [TestFixture]
    class GameControllerAITest
    {

    [SetUp]
        public void Init()
        {
           
        }
    LogTest log = new LogTest();

        /// <summary>
        /// ����: ���������, ��� ��������� ����� ������ ���� 
        /// ���������: ��������� ������ ������������ ����� � �������� ��� ��������
        /// </summary>
        [Test]
        public void AiCanStartTheGame()
        {
            GameController gm = new GameController(new LogTest(), new TestServer2());

            gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams());
            gm.AddPlayer(TypePlayer.AI, "AI", new GameStartParams());
            
            gm.StartGame(1);
            GameControllerTestHelper.MakeMoveAi(gm, log);
            Assert.AreEqual(gm.LogCard.Count(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used), 1,
                "��������� ������ ������������ �����");

            Assert.AreEqual(gm.CurrentPlayer.type, TypePlayer.Human, "��� ������ ��������� � ��������");
        }


        /// <summary>
        /// ����: ���������, ��� ��������� ����������� �����
        /// ���������: ��������� ������ ������������ ����� � id == 1, �.�. �� ������ ������ ��� �������� ��� ��� �������
        /// </summary>
        [Test]
        public void WhichCardUseAi()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> { 1 }, new List<int> { 2 });
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
            Assert.AreEqual(gm.GetAiUsedCard().LastOrDefault().id, 2, "��������� ������ ������������ ����� id 2");
        }
        
  
        /// <summary>
        /// ����: ���������, ��� ��������� ����� �������� 
        /// ���������: ����� ������ ������ ���� ����������, � ���������� Winner ������ ��������� ��� ���������� (� ��� �� �������� Loser) 
        /// </summary>
        [Test]
        public void ComputerMustWin()
        {
            GameController gm = GameControllerTestHelper.InitDemoGame(3);
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
           // gm.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.AIMoveIsAnimated } });
           // Assert.AreEqual(gm.Status, CurrentAction.UpdateStatAI, "������� ������ ������ ���� ������ ���������� ���������� ����������");
       
           // gm.SendGameNotification( new Dictionary<string, object>() {{"CurrentAction", CurrentAction.EndAIMove }});
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
            GameController gm = GameControllerTestHelper.InitDemoGame(5, null, null, 6, null, new List<int> { 2 });
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);
            //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
            Assert.AreEqual(gm.LogCard.LastOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped).card.id, 2, "AI ������ �������� ����� 2");
        }


        /// <summary>
        /// ����: ��������� ��� ��������� ������� ������ �����, ����� ������������� ����������� �����
        /// ���������: ������ ���� ����� � ������������ id 
        /// </summary>
        [Test]
        public void AiCanGetAnotherCard()
        {
            //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
            GameController gm = GameControllerTestHelper.InitDemoGame(4, null, null, 6, null, new List<int> { 55, 6 });
            gm.MakePlayerMove(1, true);
            gm.NextPlayerTurn();
            GameControllerTestHelper.MakeMoveAi(gm, log);

            Assert.AreEqual(gm.LogCard.LastOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).card.id, 6,
                "AI ������ ��� ������������ ����� 6");

            Assert.AreEqual(gm.LogCard.FirstOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).card.id, 55,
                "AI ������ ��� ������������ ����� 55");

            Assert.AreEqual(gm.GetAiUsedCard().Count, 2, "AI ������ ��� ������������ 2 �����");
        }

    }
}
