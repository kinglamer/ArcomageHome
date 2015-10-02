using System.Collections.Generic;
using System.Linq;
using Arcomage.Core;
using Arcomage.Core.AlternativeServers;
using Arcomage.Core.Interfaces.Impl;
using Arcomage.Entity;
using NUnit.Framework;

namespace Arcomage.Tests.GameControllerTests
{
     [TestFixture]
    class GameControllerTest
     {

         private GameController gm;

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
             gm = GameControllerTestHelper.InitDemoGame();
             Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "���� �� ����������");
         }

         /// <summary>
         /// ����: ���������, ��� ����� ����� ��������
         /// ���������:  ����� ���������� ������ ���� ����������, � ���������� Winner ������ ��������� ��� ������������ 
         /// </summary>
         [Test]
         public void PlayerMustWin()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.useCard(1, gm);

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Attributes.Tower], 0, "����� ����� ������ ���� ����������");
             Assert.AreEqual(gm.Status, CurrentAction.UpdateStatHuman, "������� ������ ������ ���� ������ ���������� ���������� ������");

             gm.SendGameNotification(new Dictionary<string, object>() { { "CurrentAction", CurrentAction.EndHumanMove } });
             Assert.AreEqual(gm.Winner, "Human", "����� �� ����� ���������!");

             
         }

         /// <summary>
         /// ����: ��������, ��� ������� ����� ������������ �����
         /// ���������: ������������� ��������, ��� ����� �������� ������������
         /// </summary>
         [Test]
         public void PlayerCanUserCard()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             Assert.AreEqual(gm.IsCanUseCard(1), true, "�� �������� ������������ �����");
         }
        

         /// <summary>
         /// ����: ���������, ��� ��� ������������� ��������� ������ �� �����
         /// ���������: ������ ���� �������� � ���������� ����� �� ����� 0
         /// </summary>
         [Test]
         public void CheckPlayerInit()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             Assert.IsNotNull(gm.GetPlayerParams(), "��������� ��������� ������ �� ������ ���� �������");
             Assert.IsTrue(gm.GetPlayersCard().Count > 1, " ������ ���� ���� �� ���� �����");
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

             gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.useCard(2, gm);
             var playerParams = gm.GetPlayerParams(SelectPlayer.First);


             Assert.AreEqual(playerParams[Attributes.Wall], 5 - 4, "�� ��������� �������� �������� PlayerWall");
             Assert.AreEqual(playerParams[Attributes.Tower], 10 - 8, "�� ��������� �������� �������� PlayerTower");
             Assert.AreEqual(playerParams[Attributes.DiamondMines], 1 + 2, "�� ��������� �������� �������� PlayerDiamondMines");
             Assert.AreEqual(playerParams[Attributes.Menagerie], 1 + 3, "�� ��������� �������� �������� PlayerMenagerie");
             Assert.AreEqual(playerParams[Attributes.Colliery], 1 + 4, "�� ��������� �������� �������� PlayerColliery");
             Assert.AreEqual(playerParams[Attributes.Diamonds], 5 + 11 + 3, "�� ��������� �������� �������� PlayerDiamonds");
             Assert.AreEqual(playerParams[Attributes.Animals], 5 + 12 + 4 -5, "�� ��������� �������� �������� PlayerAnimals");
             Assert.AreEqual(playerParams[Attributes.Rocks], 5 + 13 + 5, "�� ��������� �������� �������� PlayerRocks");

             playerParams = gm.GetPlayerParams(SelectPlayer.Second);
             Assert.AreEqual(playerParams[Attributes.Wall], 5 - 4, "�� ��������� �������� �������� EnemyWall");
             Assert.AreEqual(playerParams[Attributes.Tower], 10 - 8, "�� ��������� �������� �������� EnemyTower");
             Assert.AreEqual(playerParams[Attributes.DiamondMines], 1 + 2, "�� ��������� �������� �������� EnemyDiamondMines");
             Assert.AreEqual(playerParams[Attributes.Menagerie], 1 + 3, "�� ��������� �������� �������� EnemyMenagerie");
             Assert.AreEqual(playerParams[Attributes.Colliery], 1 + 4, "�� ��������� �������� �������� EnemyColliery");
             Assert.AreEqual(playerParams[Attributes.Diamonds], 5 + 11, "�� ��������� �������� �������� EnemyDiamonds");
             Assert.AreEqual(playerParams[Attributes.Animals], 5 + 12, "�� ��������� �������� �������� EnemyAnimals");
             Assert.AreEqual(playerParams[Attributes.Rocks], 5 + 13, "�� ��������� �������� �������� EnemyRocks");
             
         }

         /// <summary>
         /// ����: ��������� ���������� ������ ���������
         /// ���������: ����� ������ ���� ����� ���-�� ��������� ��������� + ���������� ��������� � ����� + ������� ����� ��������� 
         /// </summary>
         [Test]
         public void CheckAddDiamonds()
         {
             gm = GameControllerTestHelper.InitDemoGame(2);
             GameControllerTestHelper.useCard(6, gm);
             var playerParams = gm.GetPlayerParams(SelectPlayer.First);
             Assert.AreEqual(playerParams[Attributes.Diamonds], 5 + 11 + 1, "�� ��������� �������� �������� PlayerDiamonds");
         }

         /// <summary>
         /// ����: ���������, ��� �������������� ������ ���� �� �����
         /// ���������: �������� ������������ �������� ����� � �����
         /// </summary>
         [Test]
         public void CheckApplyEnemyDirectDamage()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.useCard(3, gm);
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Attributes.Wall], 0, "�� ��������� �������� �������� EnemyDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.Second)[Attributes.Tower], 0, "�� ��������� �������� �������� EnemyDirectDamage");
     
         }

         /// <summary>
         /// ����: ���������, ��� �������������� ������ ���� �� ������
         /// ���������: �������� ������������ �������� ����� � �����
         /// </summary>
         [Test]
         public void CheckApplyPlayerDirectDamage()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             GameControllerTestHelper.useCard(4, gm);

             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Attributes.Wall], 0,"�� ��������� �������� �������� PlayerDirectDamage");
             Assert.AreEqual(gm.GetPlayerParams(SelectPlayer.First)[Attributes.Tower], 0,"�� ��������� �������� �������� PlayerDirectDamage");
         }


         /// <summary>
         /// ����: ���������,��� ����� ����� �������� ����� �����, ����� ������������� ����� � ���������� "����� ��� ���� �����"
         /// ���������: ������ ���� ������ ���� ������ "����� ������ �������� �����" 
         /// </summary>
         [Test]
         public void CheckPlayAgain()
         {
             gm = GameControllerTestHelper.InitDemoGame();
             Assert.AreEqual(gm.IsCanUseCard(55), true, "�� �������� ������������ �����");

             gm.SendGameNotification(new Dictionary<string, object>()
             {
                 {"CurrentAction", CurrentAction.HumanUseCard},
                 {"ID", 55}
             });

             Assert.AreEqual(gm.logCard.FirstOrDefault(x => x.player.type == TypePlayer.Human && x.gameEvent == GameEvent.Used).card.id, 55, "Human ������ ��� ������������ ����� 55");

             var result2 = gm.GetPlayerParams();
             Assert.AreEqual(result2[Attributes.Rocks], 7, "������ ���� ������� ������");
             Assert.AreEqual(result2[Attributes.Diamonds], 7, "������ ���� ������� ����������");
             Assert.AreEqual(gm.Status, CurrentAction.WaitHumanMove, "������� ����� ������������ ��� ���� �����");
         }

         /// <summary>
         /// ����: ��������, ��� ����� ����� ���������� ���
         /// ���������: ������ ������ ��������, ��� ������� ��� � ���������� (AI)
         /// </summary>
         [Test]
         public void PlayerCanPassMove()
         {
             gm = GameControllerTestHelper.InitDemoGame(5);
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
             gm.AddPlayer(TypePlayer.Human, "Human", new GameStartParams());
             gm.AddPlayer(TypePlayer.AI, "AI", new GameStartParams());

             gm.SendGameNotification(new Dictionary<string, object>()
             {
                 {"CurrentAction", CurrentAction.StartGame},
                 {"currentPlayer", TypePlayer.Human},
                 {"CardTricksters", new List<int> {39, 11, 12, 13, 14, 15}}
             });

             Assert.AreEqual(gm.GetPlayersCard().FirstOrDefault(x => x.id == 39).id, 39,
                 "� ������ ������ ���� ����� � �39");
         }


     }
}
