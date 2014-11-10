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

		private static IPlayer player { get; set; }

		private static IPlayer enemy { get; set; }

		public GUIScript (IPlayer ph, IPlayer ph2)
		{
				player = ph;
				enemy = ph2;
		}

		void OnGUI ()
		{
				if (player != null && enemy != null) 
				{
						PlayerTower.text = "" + player.Statistic [Specifications.PlayerTower].ToString ();
						PlayerWall.text = "" + player.Statistic [Specifications.PlayerWall].ToString ();
		
						PlayerDiamonds.text = player.Statistic [Specifications.PlayerDiamonds].ToString ();
						PlayerAnimal.text = player.Statistic [Specifications.PlayerAnimals].ToString ();
						PlayerRock.text = player.Statistic [Specifications.PlayerRocks].ToString ();
						PlayerMine.text = player.Statistic [Specifications.PlayerColliery].ToString ();
						PlayerMagic.text = player.Statistic [Specifications.PlayerDiamondMines].ToString ();
						PlayerMenagerie.text = player.Statistic [Specifications.PlayerMenagerie].ToString ();

						EnemyTower.text = "" + enemy.Statistic [Specifications.PlayerTower].ToString ();
						EnemyWall.text = "" + enemy.Statistic [Specifications.PlayerWall].ToString ();
			
						EnemyDiamonds.text = enemy.Statistic [Specifications.PlayerDiamonds].ToString ();
						EnemyAnimal.text = enemy.Statistic [Specifications.PlayerAnimals].ToString ();
						EnemyRock.text = enemy.Statistic [Specifications.PlayerRocks].ToString ();
						EnemyMine.text = enemy.Statistic [Specifications.PlayerColliery].ToString ();
						EnemyMagic.text = enemy.Statistic [Specifications.PlayerDiamondMines].ToString ();
						EnemyMenagerie.text = enemy.Statistic [Specifications.PlayerMenagerie].ToString ();
				}
				
//				GUI.skin = mainSkin;

				
		}



}
