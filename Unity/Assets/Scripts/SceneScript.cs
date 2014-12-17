using UnityEngine;
using System.Collections;
using Arcomage.Core;
using Arcomage.Entity;
using System.Collections.Generic;
using System.Linq;
using Arcomage.Common;
using AssemblyCSharp;
using System;

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
		public Texture2D PicAtlas;
		List<TextAtlasCoordinate> coordinates ;


		// Use this for initialization
		void Start ()
		{
				LoadTextures ();
				StartNewGame ();
		}

		private void LoadTextures ()
		{		 
				TextAsset mydata = Resources.Load ("atlas_map") as TextAsset;
				coordinates = new List<TextAtlasCoordinate> ();
				string[] lines = mydata.text.Split (new string[] { "\r\n" }, StringSplitOptions.None);


				foreach (string line in lines) {
						try {
								if (line.Length > 0)
										coordinates.Add (new TextAtlasCoordinate (line));
						} catch (Exception ex) {
								Debug.LogError (" Line: " + line + "Ex: " + ex);
						}
				}
		}

		private void StartNewGame ()
		{
				ILog log = new ILog ();
				gm = new GameController (log);

				gm.AddPlayer (TypePlayer.Human, "Human");
				gm.AddPlayer (TypePlayer.AI, "Computer");
				
				Dictionary<string, object> notify = new Dictionary<string, object> ();

				notify.Add ("CurrentAction", CurrentAction.StartGame);

				gm.SendGameNotification (notify);

				PushCardOnDeck (new Vector3 ());
			
		}

		private Vector3 GetSpawn ()
		{
				Vector3 spawnPosition = new Vector3 (respawnCard.position.x - 10, respawnCard.position.y, respawnCard.position.z);
//
				return spawnPosition;
		}

		private void CreateCard (Card myCard, ref Vector3 spawnPosition)
		{
				Quaternion spawnRotation = new Quaternion ();
				spawnRotation = Quaternion.identity;

				GameObject card = (GameObject)Instantiate (cards, spawnPosition, spawnRotation);
				spawnPosition.x += 4f;
				spawnPosition.z += 0.5f;
				card.GetComponent<DoneCardScript> ().cardName = myCard.name;
				

				string Paramscard = myCard.description;

				if (Paramscard == null) {
						foreach (var item in myCard.cardParams) {
								if (item.key != Specifications.CostAnimals && item.key != Specifications.CostDiamonds && item.key != Specifications.CostRocks) {
										Paramscard += item.key.ToString () + " " + item.value.ToString () + "\n";
								}
						}
				}

				var costCard = myCard.cardParams.FirstOrDefault (x => x.key == Specifications.CostAnimals ||
						x.key == Specifications.CostDiamonds || x.key == Specifications.CostRocks);
		
				card.GetComponent<DoneCardScript> ().cardId = myCard.id;
				card.GetComponent<DoneCardScript> ().cardParam = Paramscard;
				card.GetComponent<DoneCardScript> ().cardCost = costCard.value;
		
				int typeCost = 0;
				switch (costCard.key) {
				case Specifications.CostRocks:
						typeCost = 2;
						break;
				case Specifications.CostAnimals:
						typeCost = 1;
						break;
				}
				
				Texture2D CardPic = GetCardPic (myCard.id);

				card.GetComponent<DoneCardScript> ().SetCardGraph (typeCost, CardPic);
		}
		//метод для выборки из атласа картинки карты
		private Texture2D GetCardPic (int cardID)
		{
				TextAtlasCoordinate item = coordinates.FirstOrDefault (el => el.id == cardID);
				if (item == null) {
						item = coordinates.FirstOrDefault (el => el.id == 1);
				}
				int x = item.x;
				int y = PicAtlas.width - item.y - item.height;
				int w = item.width;
				int h = item.height;
				Color[] pic = PicAtlas.GetPixels (x, y, w, h);
				Texture2D PicTexture = new Texture2D (w, h);
				PicTexture.SetPixels (pic);
				PicTexture.Apply ();
				return PicTexture;
			
		}

		private void PushCardOnDeck (Vector3 cardPos)
		{
						
				Vector3 spawnPosition;
				if (cardPos.x != 0 || cardPos.y != 0 || cardPos.z != 0) {
						spawnPosition = cardPos;
				} else {
						spawnPosition = GetSpawn ();
				}

				List <Card> cardList = gm.GetCard ();
			
				//while (gm.GetCountCard() < gm.MaxCard) {
				foreach (Card card in cardList) {
			
						CreateCard (card, ref spawnPosition);
				}

			
		}

		public void PassMove (int cardID, Vector3 cardPos)
		{

				if (gm.PassMove (cardID)) {
						PushCardOnDeck (cardPos);
				}

				gm.EndMove ();
				EnemuMove ();
		}

		private void EnemuMove ()
		{
				//Todo: анимацию для хода противника
		}

		//метод для отыгрывания карты
		public void CardPlayed (int cardID, Vector3 cardPos)
		{
				if (gm.UseCard (cardID)) {			 	

						var endMov = gm.EndMove ();
					
						if (endMov == EndMoveStatus.GetCard) {

								PushCardOnDeck (cardPos);
						} else {					
								PushCardOnDeck (cardPos);
								EnemuMove ();
						}
				
				} else {
						var returnCard = gm.ReturnCard (cardID);
						CreateCard (returnCard, ref cardPos);
				}
				//Debug.Log ("Card been destroyed " + cardID + " at position " + cardPos);//тест
		}





		// Update is called once per frame
		void Update ()
		{
		
		}

		//Метод для вызова экрана конца игры
		public void EndGame (string endgametext)
		{

				var hinges = GameObject.Find ("Card(Clone)");
				if (hinges != null) {
						Destroy (hinges.gameObject);
				}
					
			
			
				GUILayout.BeginArea (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100));
				GUILayout.Box (endgametext);
				GUILayout.BeginHorizontal ();
		
				if (GUILayout.Button ("Replay")) {
						StartNewGame ();
						Debug.Log ("Replay");
				}
				
				if (GUILayout.Button ("Exit")) {
						Application.Quit ();
				}


				GUILayout.EndHorizontal ();
				GUILayout.EndArea ();
		
		}

		void OnGUI ()
		{
				GUI.skin = mainSkin;
//		if (GUI.Button (new Rect (120, 200, 60, 25), "Pass")) {
//			//Тут действия на пас
//			EnemyMove();
//			Debug.Log ("Pass!");
//		}

				if (gm.WhoWin ().Length > 0) {
						EndGame (gm.WhoWin () + " WIN!");		

				} 

		}

}







