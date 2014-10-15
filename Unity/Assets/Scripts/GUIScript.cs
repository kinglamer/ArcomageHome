using UnityEngine;
using System.Collections;
using Arcomage.Core;
using Arcomage.Entity;

public class GUIScript : MonoBehaviour {

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

	private static PlayerHelper player { get; set; } 
	private static PlayerHelper enemy { get; set; } 


	public GUIScript(PlayerHelper ph, PlayerHelper ph2)
	{
		player = ph;
		enemy = ph2;
	}





	void OnGUI()
	{
		if (player != null) {
						PlayerTower.text = "Tower: " + player.GetPlayerStat (Specifications.PlayerTower).ToString ();
						PlayerWall.text = "Wall: " + player.GetPlayerStat (Specifications.PlayerWall).ToString ();
		
						PlayerDiamonds.text = player.GetPlayerStat (Specifications.PlayerDiamonds).ToString () + " / +" + player.GetPlayerStat (Specifications.PlayerDiamondMines).ToString ();
						PlayerAnimal.text = player.GetPlayerStat (Specifications.PlayerAnimals).ToString () + " / +" + player.GetPlayerStat (Specifications.PlayerMenagerie).ToString ();
						PlayerRock.text = player.GetPlayerStat (Specifications.PlayerRocks).ToString () + " / +" + player.GetPlayerStat (Specifications.PlayerColliery).ToString ();


			EnemyTower.text = "Tower: " + player.GetPlayerStat (Specifications.PlayerTower).ToString ();
			EnemyWall.text = "Wall: " + player.GetPlayerStat (Specifications.PlayerWall).ToString ();
			
			EnemyDiamonds.text = player.GetPlayerStat (Specifications.PlayerDiamonds).ToString () + " / +" + player.GetPlayerStat (Specifications.PlayerDiamondMines).ToString ();
			EnemyAnimal.text = player.GetPlayerStat (Specifications.PlayerAnimals).ToString () + " / +" + player.GetPlayerStat (Specifications.PlayerMenagerie).ToString ();
			EnemyRock.text = player.GetPlayerStat (Specifications.PlayerRocks).ToString () + " / +" + player.GetPlayerStat (Specifications.PlayerColliery).ToString ();

				}
		
	}
}
