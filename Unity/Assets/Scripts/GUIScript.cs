using UnityEngine;
using System.Collections;
using Arcomage.Core;
using Arcomage.Entity;

public class GUIScript : MonoBehaviour
{

//	public GUISkin MainSkin;
		public GUIText PlayerName;
		public GUIText EnemyName;
		public GUIText PlayerTower;
		public GUIText PlayerWall;
		public GUIText EnemyTower;
		public GUIText EnemyWall;
		public GUIText PlayerDiamonds;
		public GUIText PlayerAnimal;
		public GUIText PlayerRock;
		public GUIText EnemyDiamonds;
		public GUIText EnemyAnimal;
		public GUIText EnemyRock;
		public GUIText PlayerMine;
		public GUIText PlayerMagic;
		public GUIText PlayerMenagerie;
		public GUIText EnemyMine;
		public GUIText EnemyMagic;
		public GUIText EnemyMenagerie;

//		public GUISkin mainSkin;


		private string endgametext;

		private static PlayerHelper player { get; set; }

		private static PlayerHelper enemy { get; set; }

		public GUIScript (PlayerHelper ph, PlayerHelper ph2)
		{
				player = ph;
				enemy = ph2;
		}

		void OnGUI ()
		{
				if (player != null && enemy != null) {
						PlayerTower.text = "" + player.GetPlayerStat (Specifications.PlayerTower).ToString ();
						PlayerWall.text = "" + player.GetPlayerStat (Specifications.PlayerWall).ToString ();
		
						PlayerDiamonds.text = player.GetPlayerStat (Specifications.PlayerDiamonds).ToString ();
						PlayerAnimal.text = player.GetPlayerStat (Specifications.PlayerAnimals).ToString ();
						PlayerRock.text = player.GetPlayerStat (Specifications.PlayerRocks).ToString ();
						PlayerMine.text = player.GetPlayerStat (Specifications.PlayerColliery).ToString ();
						PlayerMagic.text = player.GetPlayerStat (Specifications.PlayerDiamondMines).ToString ();
						PlayerMenagerie.text = player.GetPlayerStat (Specifications.PlayerMenagerie).ToString ();

						EnemyTower.text = "" + enemy.GetPlayerStat (Specifications.PlayerTower).ToString ();
						EnemyWall.text = "" + enemy.GetPlayerStat (Specifications.PlayerWall).ToString ();
			
						EnemyDiamonds.text = enemy.GetPlayerStat (Specifications.PlayerDiamonds).ToString ();
						EnemyAnimal.text = enemy.GetPlayerStat (Specifications.PlayerAnimals).ToString ();
						EnemyRock.text = enemy.GetPlayerStat (Specifications.PlayerRocks).ToString ();
						EnemyMine.text = enemy.GetPlayerStat (Specifications.PlayerColliery).ToString ();
						EnemyMagic.text = enemy.GetPlayerStat (Specifications.PlayerDiamondMines).ToString ();
						EnemyMenagerie.text = enemy.GetPlayerStat (Specifications.PlayerMenagerie).ToString ();
				}
				
//				GUI.skin = mainSkin;

				
		}



}
