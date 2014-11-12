using UnityEngine;
using System.Collections;
using Arcomage.Core;
using Arcomage.Entity;
using System.Collections.Generic;
using System.Linq;
using Arcomage.Common;

public class SceneScript : MonoBehaviour, ILog
{


	#region ILog implementation

	public void Error (string text)
	{
		Debug.LogError (text);
	}


	public void Info (string text)
	{
		Debug.Log (text);
	}
	#endregion


		public Transform respawnCard ;
		public GameObject cards ;
		
		public GUISkin mainSkin;
		public static GameController gm;



		// Use this for initialization
		void Start ()
		{
			StartNewGame ();
		}

		void StartNewGame ()
		{
			gm = new GameController (this);
			gm.AddPlayer(TypePlayer.Human, "Human");
			gm.AddPlayer(TypePlayer.AI, "Comp");
			gm.StartGame ();
			/*enemyInfo = new Player ("Comp");
			ps = new Player ("Human");
			//ps.SetTheEnemy (enemyInfo);
			//enemyInfo.SetTheEnemy (ps);*/
			//GUIScript guiS = new GUIScript (, SecondPlayer);
			PushCardOnDeck (new Vector3 ());
			
	}

		private Vector3 GetSpawn ()
		{
				Vector3 spawnPosition = new Vector3 (respawnCard.position.x - 10, respawnCard.position.y, respawnCard.position.z);
//
				return spawnPosition;
		}

		private void CreateCard (Card myCard,ref Vector3 spawnPosition)
		{
				Quaternion spawnRotation = new Quaternion ();
				spawnRotation = Quaternion.identity;

				GameObject card = (GameObject)Instantiate (cards, spawnPosition, spawnRotation);
				spawnPosition.x += 5f;
				spawnPosition.z += 0.5f;
				card.GetComponent<DoneCardScript> ().cardName = myCard.name;
				

				string Paramscard = myCard.description;

				if (Paramscard == null) 
				{
						foreach (var item in myCard.cardParams) {
								if (item.key != Specifications.CostAnimals && item.key != Specifications.CostDiamonds && item.key != Specifications.CostRocks) {
										Paramscard += item.key.ToString () + " " + item.value.ToString () + "\n";
								}
						}
				}

				var costCard = myCard.cardParams.FirstOrDefault (x => x.key == Specifications.CostAnimals ||
		                                                 x.key == Specifications.CostDiamonds || x.key == Specifications.CostRocks).value;

				card.GetComponent<DoneCardScript> ().cardId = myCard.id;
				card.GetComponent<DoneCardScript> ().cardParam = Paramscard;
				card.GetComponent<DoneCardScript> ().cardCost = costCard;
		}

		private void PushCardOnDeck (Vector3 cardPos)
		{
						
				Vector3 spawnPosition;
				if (cardPos.x != 0 || cardPos.y != 0 || cardPos.z != 0) {
						spawnPosition = cardPos;
				} else {
						spawnPosition = GetSpawn ();
				}

			
			
					while (gm.GetCountCard() < gm.MaxCard) {
			
					var myCard = gm.GetCard ();
			
					CreateCard (myCard,ref spawnPosition);
				}

			
		}
		

	private void EnemyMove ()
	{
		//Todo: анимацию для хода противника
		//AIHelper.MakeMove (enemyInfo);
	}

		//метод для отыгрывания карты
		public void CardPlayed (int cardID, Vector3 cardPos)
		{
				if (gm.UseCard (cardID)) 
				{			 	

			var endMov = gm.EndMove();
					
					if (endMov == EndMoveStatus.GetCard)
					{

						PushCardOnDeck (cardPos);
					}
					else
					{
					
						PushCardOnDeck (cardPos);
						EnemyMove ();
					}
				
				}
				else 
				{
						var returnCard = gm.ReturnCard(cardID);
						CreateCard (returnCard,ref cardPos);
				}
				//Debug.Log ("Card been destroyed " + cardID + " at position " + cardPos);//тест
		}





		// Update is called once per frame
		void Update ()
		{
		
		}

	//Метод для вызова экрана конца игры
		public void EndGame(string endgametext)
		{

			var hinges = GameObject.Find ("Done_Card(Clone)");
			if (hinges != null) {
						Destroy (hinges.gameObject);
				}
					
			
			
			GUILayout.BeginArea(new Rect(Screen.width / 2-150, Screen.height / 2-50, 300, 100));
			GUILayout.Box(endgametext);
			GUILayout.BeginHorizontal();
		
			if(GUILayout.Button("Replay"))
			{
				StartNewGame();
				Debug.Log("Replay");
			}
				
			if (GUILayout.Button("Exit"))
			{
				Application.Quit();
			}


			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		
		}


	void OnGUI()
	{
		GUI.skin = mainSkin;
		if (GUI.Button (new Rect (120, 200, 60, 25), "Pass")) {
			//Тут действия на пас
			EnemyMove();
			Debug.Log ("Pass!");
		}

		if (gm.WhoWin ().Length > 0) 
		{
			EndGame (gm.WhoWin () + " WIN!");		

		} 

	}

}







