﻿using System.IO;
using Arcomage.Core;
using Arcomage.Core.Common;
using Arcomage.Core.Interfaces;
using Arcomage.Core.AlternativeServers;
using Arcomage.Core.ArcomageService;
using UnityEngine;
using System.Collections;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
using Arcomage.Entity.Interfaces;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp;
using System;
using Random = UnityEngine.Random;

public enum SoundTypes
{
		card,
		bricksdown,
		bricksup,
		damage,
		damage2,
		harp,
		loss,
		resourceloss,
		towerup,
		towerwallgain,
		victory,
		wallup,
		None
}

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
		private CurrentAction curr;
		public GameObject gameScreenText;
		private bool GameIsOver = false;
		public GameObject PlayerTower;
		public GameObject EnemyTower;
		public GameObject PlayerWall;
		public GameObject EnemyWall;
		private object[] myMusic; // declare this as Object array
		public bool Mute = false;
		public static Dictionary<SoundTypes, AudioClip> AudioClips;
        AudioSource audios;
        
		void Awake ()
		{
//				if (Mute)
//						audio.volume = 0;
//				else {
//						audio.volume = 30;
//				}
          
				myMusic = Resources.LoadAll ("Music", typeof(AudioClip));
                audios = GetComponent<AudioSource>();
				playRandomMusic ();

				AudioClips = new Dictionary<SoundTypes, AudioClip> ();

				AudioClips.Add (SoundTypes.card, Resources.Load ("sounds/card") as AudioClip);
				AudioClips.Add (SoundTypes.bricksdown, Resources.Load ("sounds/bricks down") as AudioClip);
				AudioClips.Add (SoundTypes.bricksup, Resources.Load ("sounds/bricks up") as AudioClip);
				AudioClips.Add (SoundTypes.damage, Resources.Load ("sounds/damage") as AudioClip);
				AudioClips.Add (SoundTypes.damage2, Resources.Load ("sounds/damage_ (2)") as AudioClip);
				AudioClips.Add (SoundTypes.harp, Resources.Load ("sounds/harp") as AudioClip);
				AudioClips.Add (SoundTypes.loss, Resources.Load ("sounds/loss") as AudioClip);
				AudioClips.Add (SoundTypes.resourceloss, Resources.Load ("sounds/resourceloss") as AudioClip);
				AudioClips.Add (SoundTypes.towerup, Resources.Load ("sounds/tower up") as AudioClip);
				AudioClips.Add (SoundTypes.towerwallgain, Resources.Load ("sounds/towerwallgain") as AudioClip);
				AudioClips.Add (SoundTypes.victory, Resources.Load ("sounds/victory") as AudioClip);
				AudioClips.Add (SoundTypes.wallup, Resources.Load ("sounds/wall up") as AudioClip);

		}

		private void playRandomMusic ()
		{
				audios.clip = myMusic [Random.Range (0, myMusic.Length)] as AudioClip;
				audios.Play ();
		}

		void Start ()
		{
				LoadTextures ();
				StartNewGame ();
		}

		private void LoadTextures ()
		{
				TextAsset mydata = Resources.Load ("atlas_map") as TextAsset;
				coordinates = new List<TextAtlasCoordinate> ();
				string[] lines = mydata.text.Split (new string[] {"\r\n"}, StringSplitOptions.None);


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
				GameIsOver = false;

                string filePath = GetStreamingAssetsPath("arcomageDB.db");
                var newServe = new ArcoSQLLiteServer(filePath);
                Debug.LogWarning("newServe.Path: " + newServe.connectionPath);
                gm = new GameController(this, newServe);

				gm.AddPlayer (TypePlayer.Human, "Human");
				gm.AddPlayer (TypePlayer.AI, "Computer");

				Dictionary<string, object> notify = new Dictionary<string, object> ();

				notify.Add ("CurrentAction", CurrentAction.StartGame);
				notify.Add ("currentPlayer", TypePlayer.Human);

//				notify.Add ("CardTricksters", new List<int> { 39, 11, 12, 13, 14, 15 });


				gm.SendGameNotification (notify);

		}

    private string GetStreamingAssetsPath(string fileName)
    {

        string path;
#if UNITY_EDITOR
    path = Path.Combine(Application.streamingAssetsPath, fileName);
#elif UNITY_ANDROID
     path = "//"+ Application.dataPath + "!/assets/";
#elif UNITY_IOS
     path = Application.dataPath + "/Raw";
#elif UNITY_STANDALONE_OSX
     path = Application.streamingAssetsPath + @"/" + fileName;
#else
    path = Path.Combine(Application.streamingAssetsPath, fileName);
    
#endif

        return path;
    }

        private void CreateCard (Card myCard, ref Vector3 spawnPosition, bool isAICard = false)
		{
				Quaternion spawnRotation = new Quaternion ();
				spawnRotation = Quaternion.identity;

				GameObject card = (GameObject)Instantiate (cards, spawnPosition, spawnRotation);
				spawnPosition.x += 4.8f;
				card.GetComponent<DoneCardScript> ().cardName = myCard.name;


				string Paramscard = myCard.description;

				if (Paramscard == null) {
						foreach (var item in myCard.cardParams) {
								if (item.key != Specifications.CostAnimals && item.key != Specifications.CostDiamonds &&
										item.key != Specifications.CostRocks) {
										Paramscard += item.key + " " + item.value + "\n";
								}
						}
				}

				var costCard = myCard.cardParams.FirstOrDefault (x => x.key == Specifications.CostAnimals ||
						x.key == Specifications.CostDiamonds ||
						x.key == Specifications.CostRocks);

				card.GetComponent<DoneCardScript> ().cardId = myCard.id;
				card.GetComponent<DoneCardScript> ().cardParam = Paramscard;
				card.GetComponent<DoneCardScript> ().ListOfParamses =
            myCard.cardParams.Where (x => x.key != Specifications.CostAnimals &&
						x.key != Specifications.CostDiamonds && x.key != Specifications.CostRocks)
                .ToList ();
				card.GetComponent<DoneCardScript> ().cardCost = costCard.value;

				if (!isAICard) {
						card.GetComponent<DoneCardScript> ().CardIsActive = gm.IsCanUseCard (myCard.price);
				}
				card.GetComponent<DoneCardScript> ().thisCard = myCard;

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

				if (isAICard) {
						card.tag = "AICard";
						card.GetComponent<AICardMoover> ().enabled = true;
				}
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
				GameObject[] hinges = GameObject.FindGameObjectsWithTag ("Card");
				if (hinges != null) {
						foreach (GameObject card in hinges)
								Destroy (card);
				}
				List <Card> cardList = gm.GetPlayersCard (SelectPlayer.First);
			
				//while (gm.GetCountCard() < gm.MaxCard) {
				foreach (Card card in cardList) {
			
						CreateCard (card, ref spawnPosition);
				}

			
		}

		private Vector3 GetSpawn ()
		{
				Vector3 spawnPosition = new Vector3 (respawnCard.position.x - 12, respawnCard.position.y, respawnCard.position.z);
				return spawnPosition;
		}

		public void PassMove (int cardID, Vector3 cardPos, GameObject cardObject)
		{
				//if (curr == CurrentAction.WaitHumanMove || curr == CurrentAction.PlayerMustDropCard) {
				if (curr == CurrentAction.WaitHumanMove) {
						Dictionary<string, object> notify = new Dictionary<string, object> ();
						notify.Add ("CurrentAction", CurrentAction.PassStroke);
						notify.Add ("ID", cardID);
						gm.SendGameNotification (notify);
						cardObject.GetComponent<CardPassMoover> ().enabled = true;
				}
				
		}

		//метод для отыгрывания карты
		public void CardPlayed (int cardID, Vector3 cardPos, GameObject cardObject)
		{
				if (curr == CurrentAction.WaitHumanMove && gm.additionaStatus != CurrentAction.PlayerMustDropCard) {

						var soundParam = gm.GetPlayersCard ().First (x => x.id == cardID).cardParams.LastOrDefault ();
               

						Dictionary<string, object> notify = new Dictionary<string, object> ();
						notify.Add ("CurrentAction", CurrentAction.HumanUseCard);
						notify.Add ("ID", cardID);
						gm.SendGameNotification (notify);

						audios.PlayOneShot (AudioClips [SoundTypes.card]);
						PlaySecondSound (soundParam);

						cardObject.GetComponent<CardMoover> ().enabled = true;
						UpdateGameParameters ();


                    
				}
		}

		public void HumanCardPlayEnd (GameObject cardObject, Vector3 position)
		{
				cardObject.GetComponent<CardMoover> ().enabled = false;
				cardObject.tag = "Untagged";
				Destroy (cardObject, 2f);
				PushCardOnDeck (new Vector3 ());
				Dictionary<string, object> notify = new Dictionary<string, object> ();
				notify.Add ("CurrentAction", CurrentAction.AnimateHumanMove);
				gm.SendGameNotification (notify);
		}

		public void HumanCardPassEnd (GameObject cardObject, Vector3 position)
		{
				cardObject.GetComponent<CardPassMoover> ().enabled = false;
				Destroy (cardObject);
				PushCardOnDeck (new Vector3 ());
				Dictionary<string, object> notify = new Dictionary<string, object> ();
				notify.Add ("CurrentAction", CurrentAction.AnimateHumanMove);
				gm.SendGameNotification (notify);
		}

		//Метод для вызова экрана конца игры
		public void EndGame ()
		{

				GameObject[] hinges = GameObject.FindGameObjectsWithTag ("Card");
				if (hinges != null) {
						foreach (GameObject card in hinges)
								Destroy (card);
				}
					
			
				GameIsOver = true;
		
		}

		void OnGUI ()
		{
				GUI.skin = mainSkin;
				if (GameIsOver) {
						GUILayout.BeginArea (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100));
						GUILayout.Box (gm.Winner + " WINS");
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


				GUILayout.BeginArea (new Rect (Screen.width / 2 - 100, Screen.height / 20, 200, 50));
				Mute = GUILayout.Toggle (Mute, "Mute Sound");
				AudioListener.pause = Mute;
				GUILayout.EndArea ();

		}

		void UpdateGameParameters ()
		{
                

				Dictionary<Attributes,int> humanparam = gm.GetPlayerParams (SelectPlayer.First);
				Dictionary<Attributes,int> enemyparam = gm.GetPlayerParams (SelectPlayer.Second);
                gameObject.GetComponent<GUIScript>().PlayerTower.text = humanparam[Attributes.Tower].ToString();
				gameObject.GetComponent<GUIScript> ().PlayerWall.text = humanparam [Attributes.Wall].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerDiamonds.text = humanparam [Attributes.Diamonds].ToString ();
                gameObject.GetComponent<GUIScript>().PlayerAnimal.text = humanparam[Attributes.Animals].ToString();
                gameObject.GetComponent<GUIScript>().PlayerRock.text = humanparam[Attributes.Rocks].ToString();
				gameObject.GetComponent<GUIScript> ().PlayerMine.text = humanparam [Attributes.Colliery].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerMagic.text = humanparam [Attributes.DiamondMines].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerMenagerie.text = humanparam [Attributes.Menagerie].ToString ();
				
				gameObject.GetComponent<GUIScript> ().EnemyTower.text = enemyparam [Attributes.Tower].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyWall.text = enemyparam [Attributes.Wall].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyDiamonds.text = enemyparam [Attributes.Diamonds].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyAnimal.text = enemyparam [Attributes.Animals].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyRock.text = enemyparam [Attributes.Rocks].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyMine.text = enemyparam [Attributes.Colliery].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyMagic.text = enemyparam [Attributes.DiamondMines].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyMenagerie.text = enemyparam [Attributes.Menagerie].ToString ();
				
				GameObject[] PlayerCards = GameObject.FindGameObjectsWithTag ("Card");
        

				foreach (GameObject card in PlayerCards) {
						card.GetComponent<DoneCardScript> ().CardIsActive = gm.IsCanUseCard (card.GetComponent<DoneCardScript> ().thisCard.price);
				}

                PlayerTower.GetComponent<BuildingsScript>().Height = humanparam[Attributes.Tower];
                EnemyTower.GetComponent<BuildingsScript>().Height = enemyparam[Attributes.Tower];
                PlayerWall.GetComponent<BuildingsScript>().Height = humanparam[Attributes.Wall];
                EnemyWall.GetComponent<BuildingsScript>().Height = enemyparam[Attributes.Wall];

		}

		public void AICardEndPlay (GameObject cardObject)
		{
				cardObject.GetComponent<AICardMoover> ().enabled = false;
				Destroy (cardObject);
				Dictionary<string, object> notify = new Dictionary<string, object> ();
				notify.Add ("CurrentAction", CurrentAction.AIMoveIsAnimated);
				gm.SendGameNotification (notify);
		}

		// Update is called once per frame
		private void Update ()
		{
        

				if (!audios.isPlaying)
						playRandomMusic ();

				CurrentAction prev_action = curr;
				curr = gm.Status;


				switch (curr) {
				case CurrentAction.WaitHumanMove:
						{
								//gameScreenText.guiText.enabled = false;
								if (prev_action != curr) {
										UpdateGameParameters ();
										PushCardOnDeck (new Vector3 ());
										if (gm.additionaStatus == CurrentAction.PlayerMustDropCard) {
												gameScreenText.GetComponent<GUIText>().text = "You need to drop a card";
												gameScreenText.GetComponent<GUIText>().enabled = true;
										}
								}
								break;
						}
				case CurrentAction.HumanUseCard:
						{
								gameScreenText.GetComponent<GUIText>().enabled = false;
								break;
						}
				case CurrentAction.PassStroke:
						{
								gameScreenText.GetComponent<GUIText>().enabled = false;
								break;
						}
				case CurrentAction.PlayerMustDropCard:
						{
								PushCardOnDeck (new Vector3 ());
								gameScreenText.GetComponent<GUIText>().text = "You need to drop a card";
								gameScreenText.GetComponent<GUIText>().enabled = true;
								break;
						}
				case CurrentAction.UpdateStatHuman:
						{
								Dictionary<string, object> notify = new Dictionary<string, object> ();
								notify.Add ("CurrentAction", CurrentAction.EndHumanMove);
								gm.SendGameNotification (notify);
								break;
						}
				case CurrentAction.AIUseCardAnimation:
						{
								if (prev_action != curr) {
										List<Card> AICardsPlayed = gm.GetAIUsedCard ();
										int x = 0;
										foreach (Card card in AICardsPlayed) {
												var vect = new Vector3 (x, 2f, 0f);
												CreateCard (card, ref vect, true);
												var soundParam = card.cardParams.LastOrDefault ();

												audios.PlayOneShot (AudioClips [SoundTypes.card]);
												PlaySecondSound (soundParam);

												x += 5;
										}
								}
								break;
						}
				case CurrentAction.UpdateStatAI:
						{
								Dictionary<string, object> notify = new Dictionary<string, object> ();
								notify.Add ("CurrentAction", CurrentAction.EndAIMove);
								gm.SendGameNotification (notify);
								break;
						}
				case CurrentAction.EndGame:
						{
								if (prev_action != curr) {
										EndGame ();
								}
								break;
						}
				}

		}

		private void PlaySecondSound (CardParams item)
		{

				SoundTypes typeS = SoundTypes.None;

				switch (item.key) {
				case Specifications.PlayerTower:
				case Specifications.EnemyTower:
						if (item.value > 0)
								typeS = SoundTypes.towerup;
						else {
								typeS = SoundTypes.damage2;
						}
						break;
				case Specifications.PlayerWall:
				case Specifications.EnemyWall:
						if (item.value > 0)
								typeS = SoundTypes.wallup;
						else {
								typeS = SoundTypes.damage;
						}
						break;

				case Specifications.PlayerColliery:
				case Specifications.EnemyColliery:
						if (item.value < 0)
								typeS = SoundTypes.bricksdown;
						else {
								typeS = SoundTypes.bricksup;
						}
						break;
				case Specifications.PlayerDiamonds:
				case Specifications.PlayerAnimals:
				case Specifications.PlayerRocks:
				case Specifications.EnemyDiamonds:
				case Specifications.EnemyAnimals:
				case Specifications.EnemyRocks:
						if (item.value < 0)
								typeS = SoundTypes.resourceloss;
						else {
								typeS = SoundTypes.harp;
						}
						break;
				case Specifications.EnemyDiamondMines:
				case Specifications.PlayerDiamondMines:
				case Specifications.PlayerMenagerie:
				case Specifications.EnemyMenagerie:
						if (item.value < 0)
								typeS = SoundTypes.resourceloss;
						else {
								typeS = SoundTypes.towerwallgain;
						}
						break;
				case Specifications.EnemyDirectDamage:
				case Specifications.PlayerDirectDamage:
						typeS = SoundTypes.damage;
						break;

				}

				if (typeS != SoundTypes.None) {
						audios.PlayOneShot (SceneScript.AudioClips [typeS], 0.7f);
				}
		}

}







