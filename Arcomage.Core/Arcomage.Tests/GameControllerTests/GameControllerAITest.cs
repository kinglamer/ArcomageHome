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
        private GameController gm;

    [SetUp]
        public void Init()
        {
           
        }


        /// <summary>
        /// ����: ���������, ��� ��������� ����� ������ ���� 
        /// ���������: ��������� ������ ������������ ����� � �������� ��� ��������
        /// </summary>
        [Test]
        public void AICanStartTheGame()
        {
            GameController gm = new GameController(new LogTest(), new TestServer2());

            gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams());
            gm.AddPlayer(TypePlayer.AI, "AI", new GameStartParams());
    
            gm.SendGameNotification(new Dictionary<string, object>()
            {
                { "CurrentAction", CurrentAction.StartGame },
                {"currentPlayer", TypePlayer.AI}
            });

            Assert.AreEqual(gm.Status, CurrentAction.AIUseCardAnimation, "������� ������ ������ ���� ������ ���������� ����� ����������");
        }


        /// <summary>
        /// ����: ���������, ��� ��������� ����������� �����
        /// ���������: ��������� ������ ������������ ����� � id == 1, �.�. �� ������ ������ ��� �������� ��� ��� �������
        /// </summary>
        [Test]
        public void WhichCardUseAI()
        {
            gm = GameControllerTestHelper.InitDemoGame();
            GameControllerTestHelper.PassStroke(gm);
            Assert.AreEqual(gm.GetAIUsedCard().LastOrDefault().id, 2, "��������� ������ ������������ ����� id 2");
        }



    

  
        /// <summary>
        /// ����: ���������, ��� ��������� ����� �������� 
        /// ���������: ����� ������ ������ ���� ����������, � ���������� Winner ������ ��������� ��� ���������� (� ��� �� �������� Loser) 
        /// </summary>
        [Test]
        public void ComputerMustWin()
        {
            gm = GameControllerTestHelper.InitDemoGame(3);
            GameControllerTestHelper.PassStroke(gm);
            
            gm.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.AIMoveIsAnimated } });
            Assert.AreEqual(gm.Status, CurrentAction.UpdateStatAI, "������� ������ ������ ���� ������ ���������� ���������� ����������");
       
            gm.SendGameNotification( new Dictionary<string, object>() {{"CurrentAction", CurrentAction.EndAIMove }});
            Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Attributes.Tower], 0, "����� ����� ������ ���� ����������");
            Assert.AreEqual(gm.Winner, "AI", "��������� �� ����� ���������!");

        }


        /// <summary>
        /// ����: ���������, ��� AI ��������� ��� � ������� �����
        /// ���������: � ���� ������ ���� ������ id ������������ �����
        /// </summary>
        [Test]
        public void AICanPassMove()
        {
            gm = GameControllerTestHelper.InitDemoGame(5);
            GameControllerTestHelper.PassStroke(gm);

            //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
            Assert.AreEqual(gm.logCard.LastOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Droped).card.id, 2, "AI ������ �������� ����� 2");
        }


        /// <summary>
        /// ����: ��������� ��� ��������� ������� ������ �����, ����� ������������� ����������� �����
        /// ���������: ������ ���� ����� � ������������ id 
        /// </summary>
        [Test]
        public void AICanGetAnotherCard()
        {
            //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
            gm = GameControllerTestHelper.InitDemoGame(4);
            GameControllerTestHelper.PassStroke(gm);
            
            Assert.AreEqual(gm.logCard.LastOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).card.id, 6,
                "AI ������ ��� ������������ ����� 6");

            Assert.AreEqual(gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.AI && x.gameEvent == GameEvent.Used).card.id, 55,
                "AI ������ ��� ������������ ����� 55");

            Assert.AreEqual(gm.GetAIUsedCard().Count, 2, "AI ������ ��� ������������ 2 �����");
        }

    }
}
