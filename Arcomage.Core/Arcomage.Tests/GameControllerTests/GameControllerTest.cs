using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
     [TestFixture]
    class GameControllerTest
     {


         [SetUp]
         public void Init()
         {
            
         }

         /// <summary>
         /// ����: ��������, ��� ���� ����������
         /// ���������: ������ ���� ������ ���� ������ "GetPlayerCard" - �� ��������, ��� ����� ������ �������� �����
         /// </summary>
         [Test]
         public void GameIsStarted()
         {
            GameController  gm = GameControllerTestHelper.InitDemoGame();
            // Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "���� �� ����������");
         }

         /// <summary>
         /// ����: ���������, ��� ����� ����� ��������
         /// ���������:  ����� ���������� ������ ���� ����������, � ���������� Winner ������ ��������� ��� ������������ 
         /// </summary>
         [Test]
         public void PlayerMustWin()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(0, null, null, 6, new List<int> { 1 });
             GameControllerTestHelper.UseCard(1, gm);
             gm.NextPlayerTurn();
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0, "����� ����� ������ ���� ����������");
            // Assert.AreEqual(gm.Status, CurrentAction.UpdateStatHuman, "������� ������ ������ ���� ������ ���������� ���������� ������");

            // gm.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.EndHumanMove } });
             Assert.AreEqual(gm.Winner, "Human", "����� �� ����� ���������!");

             
         }

         /// <summary>
         /// ����: ��������, ��� ������� ����� ������������ �����
         /// ���������: ������������� ��������, ��� ����� �������� ������������
         /// </summary>
         [Test]
         public void PlayerCanUserCard()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(0,null,null,6, new List<int>{1});
             Assert.AreEqual(gm.IsCanUseCard(1), true, "�� �������� ������������ �����");
         }
        

         /// <summary>
         /// ����: ���������, ��� ��� ������������� ��������� ������ �� �����
         /// ���������: ������ ���� �������� � ���������� ����� �� ����� 0
         /// </summary>
         [Test]
         public void CheckPlayerInit()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame();
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
             GameController gm = new GameController(new LogTest(), new TestServer());
             gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams());
             gm.AddPlayer(TypePlayer.Human, "AI", new GameStartParams());
             gm.StartGame(0);
             
             GameControllerTestHelper.UseCard(2, gm);
             gm.NextPlayerTurn();
          
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 5 - 4, "�� ��������� �������� �������� PlayerWall");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 10 - 8, "�� ��������� �������� �������� PlayerTower");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.DiamondMines], 1 + 2, "�� ��������� �������� �������� PlayerDiamondMines");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Menagerie], 1 + 3, "�� ��������� �������� �������� PlayerMenagerie");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Colliery], 1 + 4, "�� ��������� �������� �������� PlayerColliery");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Diamonds], 5 + 11 + 3, "�� ��������� �������� �������� PlayerDiamonds");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Animals], 5 + 12 + 4 - 5, "�� ��������� �������� �������� PlayerAnimals");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Rocks], 5 + 13 + 5, "�� ��������� �������� �������� PlayerRocks");

             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 5 - 4, "�� ��������� �������� �������� EnemyWall");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 10 - 8, "�� ��������� �������� �������� EnemyTower");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.DiamondMines], 1 + 2, "�� ��������� �������� �������� EnemyDiamondMines");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Menagerie], 1 + 3, "�� ��������� �������� �������� EnemyMenagerie");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Colliery], 1 + 4, "�� ��������� �������� �������� EnemyColliery");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 5 + 11, "�� ��������� �������� �������� EnemyDiamonds");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Animals], 5 + 12, "�� ��������� �������� �������� EnemyAnimals");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 5 + 13, "�� ��������� �������� �������� EnemyRocks");
             
         }

         /// <summary>
         /// ����: ��������� ���������� ������ ���������
         /// ���������: ����� ������ ���� ����� ���-�� ��������� ��������� + ���������� ��������� � ����� + ������� ����� ��������� 
         /// </summary>
         [Test]
         public void CheckAddDiamonds()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(2);
             GameControllerTestHelper.UseCard(6, gm);
             gm.NextPlayerTurn();
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 5 + 11 + 1, "�� ��������� �������� �������� PlayerDiamonds");
         }

         /// <summary>
         /// ����: ���������, ��� �������������� ������ ���� �� �����
         /// ���������: �������� ������������ �������� ����� � �����
         /// </summary>
         [Test]
         public void CheckApplyEnemyDirectDamage()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.UseCard(3, gm);
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Wall], 0, "�� ��������� �������� �������� EnemyDirectDamage");
             Assert.AreEqual(gm.EnemyPlayer.PlayerParams[Attributes.Tower], 0, "�� ��������� �������� �������� EnemyDirectDamage");
     
         }

         /// <summary>
         /// ����: ���������, ��� �������������� ������ ���� �� ������
         /// ���������: �������� ������������ �������� ����� � �����
         /// </summary>
         [Test]
         public void CheckApplyPlayerDirectDamage()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.UseCard(4, gm);

             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Wall], 0, "�� ��������� �������� �������� PlayerDirectDamage");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Tower], 0, "�� ��������� �������� �������� PlayerDirectDamage");
         }


         /// <summary>
         /// ����: ���������,��� ����� ����� �������� ����� �����, ����� ������������� ����� � ���������� "����� ��� ���� �����"
         /// ���������: ������ ���� ������ ���� ������ "����� ������ �������� �����" 
         /// </summary>
         [Test]
         public void CheckPlayAgain()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame();
             Assert.AreEqual(gm.IsCanUseCard(55), true, "�� �������� ������������ �����");

    
             gm.MakePlayerMove(55);
             gm.NextPlayerTurn();
             Assert.AreEqual(gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Used).card.id, 55, "Human ������ ��� ������������ ����� 55");

        
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Rocks], 5, "�������� ������ �� ������ ����");
             Assert.AreEqual(gm.CurrentPlayer.PlayerParams[Attributes.Diamonds], 6, "�������� ���������� �� ������ ����");
             Assert.AreEqual(gm.CurrentPlayer.gameActions.Contains(GameAction.PlayAgain), true, "������� ����� ������������ ��� ���� �����");
         }

         /// <summary>
         /// ����: ��������, ��� ����� ����� ���������� ���
         /// ���������: ������ ������ ��������, ��� ������� ��� � ���������� (AI)
         /// </summary>
         [Test]
         public void PlayerCanPassMove()
         {
             GameController gm = GameControllerTestHelper.InitDemoGame(5);
             GameControllerTestHelper.PassStroke(gm);
             var result = gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Droped);
             //��������: ��� ������������������ AI ������ ���� ����� ����������, .�.�. ���� ��� �������� ����� �������� ����� ����� ��������
             Assert.AreEqual(result.card.id, 1, "Human ������ �������� ����� 1");

         }

         /// <summary>
         /// ����: ���������, ��� �������� ������� ���������� ����
         /// ���������: � ���� ������ ���� ������������ �����
         /// </summary>
         [Test]
         public void TestCardTricker()
         {
             LogTest log = new LogTest();
             GameController gm = new GameController(log, new ArcoSQLLiteServer(@"arcomageDB.db"));
             gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams(), new List<int> {39, 11, 12, 13, 14, 15});
             gm.AddPlayer(TypePlayer.AI, "AI", new GameStartParams());

      
             gm.StartGame(0);
             Assert.AreEqual(gm.CurrentPlayer.Cards.FirstOrDefault(x => x.id == 39).id, 39,
                 "� ������ ������ ���� ����� � �39");
         }


     }
}
