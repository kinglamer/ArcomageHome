using UnityEngine;
using System.Collections;
using Arcomage.Core;
using Arcomage.Entity;
using System.Collections.Generic;
using System.Linq;
using Arcomage.Common;

public class GameController : MonoBehaviour, ILog
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
		private PlayerHelper ps;
		private PlayerHelper enemyInfo ;
		


		// Use this for initialization
		void Start ()
		{
				

				enemyInfo = new PlayerHelper (this, "Comp");
				
				ps = new PlayerHelper (this, "Human");

				ps.SetTheEnemy (enemyInfo);
				enemyInfo.SetTheEnemy (ps);
			
				GUIScript guiS = new GUIScript (ps, enemyInfo);



				PushCardOnDeck (new Vector3());






		}

		private Vector3 GetSpawn ()
		{
				Vector3 spawnPosition = new Vector3 (respawnCard.position.x - 10, respawnCard.position.y, respawnCard.position.z);
//
				return spawnPosition;
		}

		private void PushCardOnDeck (Vector3 cardPos)
		{
				Quaternion spawnRotation = new Quaternion ();
				spawnRotation = Quaternion.identity;
				

				Vector3 spawnPosition;
				if (cardPos.x != 0 && cardPos.y != 0  && cardPos.z != 0)
				{
						spawnPosition = cardPos;
				} else {
					spawnPosition =GetSpawn();
				}

			
			
				while (ps.CountCard < ps.MaxCard) {
			
						var myCard = ps.GetCard ();
			
						GameObject card = (GameObject)Instantiate (cards, spawnPosition, spawnRotation);
						spawnPosition.x += 5f;
						spawnPosition.z += 0.5f;
						card.GetComponent<DoneCardScript> ().cardName = myCard.name;


						string Paramscard = string.Empty;
						foreach (var item in myCard.cardParams) {
								if (item.key != Specifications.CostAnimals &&
										item.key != Specifications.CostDiamonds &&
										item.key != Specifications.CostRocks) {
										Paramscard += item.key.ToString () + " " + item.value.ToString () + "\n";
								}
						}
			
						var costCard = myCard.cardParams.FirstOrDefault (x => x.key == Specifications.CostAnimals || 
								x.key == Specifications.CostDiamonds ||
								x.key == Specifications.CostRocks).value;

						card.GetComponent<DoneCardScript> ().cardId = myCard.id;
			
						card.GetComponent<DoneCardScript> ().cardParam = Paramscard;
						card.GetComponent<DoneCardScript> ().cardCost = costCard;
			

			
				}
		}


		//метод для отыгрывания карты
		public void CardPlayed (int cardID, Vector3 cardPos)
		{
				ps.UseCard (cardID);
				PushCardOnDeck (cardPos);
				Debug.Log ("Card been destroyed " + cardID + " at position " + cardPos);//тест
		}


		// Update is called once per frame
		void Update ()
		{
		
		}
}





