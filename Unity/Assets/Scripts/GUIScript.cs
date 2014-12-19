﻿using UnityEngine;
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

	public static string playername { set{ PlayerName.text = value; }}
	public static string enemyname { set{ EnemyName.text = value; }}
	public static string playertower { set{ PlayerTower.text = value; }}
	public static string playerwall { set{ PlayerWall.text = value; }}
	public static string enemytower { set{ EnemyTower.text = value; }}
	public static string enemywall { set{ EnemyWall.text = value; }}
	public static string playerdiamonds { set{ PlayerDiamonds.text = value; }}
	public static string playeranimal { set{ PlayerAnimal.text = value; }}
	public static string playerrock { set{ PlayerRock.text = value; }}
	public static string enemydiamonds { set{ EnemyDiamonds.text = value; }}
	public static string enemyanimal { set{ EnemyAnimal.text = value; }}
	public static string enemyrock { set{ EnemyRock.text = value; }}
	public static string playermine { set{ PlayerMine.text = value; }}
	public static string playermagic { set{ PlayerMagic.text = value; }}
	public static string playermenagerie { set{ PlayerMenagerie.text = value; }}
	public static string enemymine { set{ EnemyMine.text = value; }}
	public static string enemymagic { set{ EnemyMagic.text = value; }}
	public static string enemymenagerie { set{ EnemyMenagerie.text = value; }}

//		public GUISkin mainSkin;


//		private string endgametext;

		void OnGUI ()
		{
				
//				PlayerTower.text = "" + SceneScript.gm.GetPlayerParams (SelectPlayer.First) [Specifications.PlayerTower].ToString ();
//				PlayerWall.text = "" + SceneScript.gm.GetPlayerParams (SelectPlayer.First) [Specifications.PlayerWall].ToString ();
//		
//				PlayerDiamonds.text = SceneScript.gm.GetPlayerParams (SelectPlayer.First) [Specifications.PlayerDiamonds].ToString ();
//				PlayerAnimal.text = SceneScript.gm.GetPlayerParams (SelectPlayer.First) [Specifications.PlayerAnimals].ToString ();
//				PlayerRock.text = SceneScript.gm.GetPlayerParams (SelectPlayer.First) [Specifications.PlayerRocks].ToString ();
//				PlayerMine.text = SceneScript.gm.GetPlayerParams (SelectPlayer.First) [Specifications.PlayerColliery].ToString ();
//				PlayerMagic.text = SceneScript.gm.GetPlayerParams (SelectPlayer.First) [Specifications.PlayerDiamondMines].ToString ();
//				PlayerMenagerie.text = SceneScript.gm.GetPlayerParams (SelectPlayer.First) [Specifications.PlayerMenagerie].ToString ();
//
//				EnemyTower.text = "" + SceneScript.gm.GetPlayerParams (SelectPlayer.Second) [Specifications.PlayerTower].ToString ();
//				EnemyWall.text = "" + SceneScript.gm.GetPlayerParams (SelectPlayer.Second) [Specifications.PlayerWall].ToString ();
//			
//				EnemyDiamonds.text = SceneScript.gm.GetPlayerParams (SelectPlayer.Second) [Specifications.PlayerDiamonds].ToString ();
//				EnemyAnimal.text = SceneScript.gm.GetPlayerParams (SelectPlayer.Second) [Specifications.PlayerAnimals].ToString ();
//				EnemyRock.text = SceneScript.gm.GetPlayerParams (SelectPlayer.Second) [Specifications.PlayerRocks].ToString ();
//				EnemyMine.text = SceneScript.gm.GetPlayerParams (SelectPlayer.Second) [Specifications.PlayerColliery].ToString ();
//				EnemyMagic.text = SceneScript.gm.GetPlayerParams (SelectPlayer.Second) [Specifications.PlayerDiamondMines].ToString ();
//				EnemyMenagerie.text = SceneScript.gm.GetPlayerParams (SelectPlayer.Second) [Specifications.PlayerMenagerie].ToString ();
				
				
//				GUI.skin = mainSkin;

				
		}



}
