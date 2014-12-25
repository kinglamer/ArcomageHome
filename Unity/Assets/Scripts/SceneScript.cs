using UnityEngine;
using System.Collections;
using Arcomage.Core;
using Arcomage.Entity;
using System.Collections.Generic;
using System.Linq;
using Arcomage.Common;
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

        public static Dictionary<SoundTypes, AudioClip> AudioClips;
        
        void Awake()
        {
            audio.volume = 30;
            myMusic = Resources.LoadAll("Music", typeof(AudioClip));
            playRandomMusic();

            AudioClips = new Dictionary<SoundTypes, AudioClip>();

            AudioClips.Add(SoundTypes.card, Resources.Load("sounds/card") as AudioClip);
            AudioClips.Add(SoundTypes.bricksdown, Resources.Load("sounds/bricks down") as AudioClip);
            AudioClips.Add(SoundTypes.bricksup, Resources.Load("sounds/bricks up") as AudioClip);
            AudioClips.Add(SoundTypes.damage, Resources.Load("sounds/damage") as AudioClip);
            AudioClips.Add(SoundTypes.damage2, Resources.Load("sounds/damage_ (2)") as AudioClip);
            AudioClips.Add(SoundTypes.harp, Resources.Load("sounds/harp") as AudioClip);
            AudioClips.Add(SoundTypes.loss, Resources.Load("sounds/loss") as AudioClip);
            AudioClips.Add(SoundTypes.resourceloss, Resources.Load("sounds/resourceloss") as AudioClip);
            AudioClips.Add(SoundTypes.towerup, Resources.Load("sounds/tower up") as AudioClip);
            AudioClips.Add(SoundTypes.towerwallgain, Resources.Load("sounds/towerwallgain") as AudioClip);
            AudioClips.Add(SoundTypes.victory, Resources.Load("sounds/victory") as AudioClip);
            AudioClips.Add(SoundTypes.wallup, Resources.Load("sounds/wall up") as AudioClip);
            
        }

        private void playRandomMusic()
        {
            audio.clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
            audio.Play();
        }

		void Start ()
		{
				LoadTextures ();
				StartNewGame ();
		}

    private void LoadTextures()
    {
        TextAsset mydata = Resources.Load("atlas_map") as TextAsset;
        coordinates = new List<TextAtlasCoordinate>();
        string[] lines = mydata.text.Split(new string[] {"\r\n"}, StringSplitOptions.None);


        foreach (string line in lines)
        {
            try
            {
                if (line.Length > 0)
                    coordinates.Add(new TextAtlasCoordinate(line));
            }
            catch (Exception ex)
            {
                Debug.LogError(" Line: " + line + "Ex: " + ex);
            }
        }
    }

    private void StartNewGame()
    {
        GameIsOver = false;
        gm = new GameController(this);

        gm.AddPlayer(TypePlayer.Human, "Human");
        gm.AddPlayer(TypePlayer.AI, "Computer");

        Dictionary<string, object> notify = new Dictionary<string, object>();

        notify.Add("CurrentAction", CurrentAction.StartGame);
        notify.Add("currentPlayer", TypePlayer.Human);

        gm.SendGameNotification(notify);

    }

    private void CreateCard(Card myCard, ref Vector3 spawnPosition, bool isAICard = false)
    {
        Quaternion spawnRotation = new Quaternion();
        spawnRotation = Quaternion.identity;

        GameObject card = (GameObject) Instantiate(cards, spawnPosition, spawnRotation);
        spawnPosition.x += 4.8f;
        card.GetComponent<DoneCardScript>().cardName = myCard.name;


        string Paramscard = myCard.description;

        if (Paramscard == null)
        {
            foreach (var item in myCard.cardParams)
            {
                if (item.key != Specifications.CostAnimals && item.key != Specifications.CostDiamonds &&
                    item.key != Specifications.CostRocks)
                {
                    Paramscard += item.key + " " + item.value + "\n";
                }
            }
        }

        var costCard = myCard.cardParams.FirstOrDefault(x => x.key == Specifications.CostAnimals ||
                                                             x.key == Specifications.CostDiamonds ||
                                                             x.key == Specifications.CostRocks);

        card.GetComponent<DoneCardScript>().cardId = myCard.id;
        card.GetComponent<DoneCardScript>().cardParam = Paramscard;
        card.GetComponent<DoneCardScript>().ListOfParamses =
            myCard.cardParams.Where(x => x.key != Specifications.CostAnimals &&
                                         x.key != Specifications.CostDiamonds && x.key != Specifications.CostRocks)
                .ToList();
        card.GetComponent<DoneCardScript>().cardCost = costCard.value;
        card.GetComponent<DoneCardScript>().CardIsActive = gm.IsCanUseCard(myCard.cardParams);
        card.GetComponent<DoneCardScript>().thisCard = myCard;

        int typeCost = 0;
        switch (costCard.key)
        {
            case Specifications.CostRocks:
                typeCost = 2;
                break;
            case Specifications.CostAnimals:
                typeCost = 1;
                break;
        }

        Texture2D CardPic = GetCardPic(myCard.id);

        card.GetComponent<DoneCardScript>().SetCardGraph(typeCost, CardPic);

        if (isAICard)
        {
            card.tag = "AICard";
            card.GetComponent<AICardMoover>().enabled = true;
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
				if (curr == CurrentAction.WaitHumanMove || curr == CurrentAction.PlayerMustDropCard) {
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
				if (curr == CurrentAction.WaitHumanMove) {
                  
						Dictionary<string, object> notify = new Dictionary<string, object> ();
						notify.Add ("CurrentAction", CurrentAction.HumanUseCard);
						notify.Add ("ID", cardID);
						gm.SendGameNotification (notify);
						cardObject.GetComponent<CardMoover> ().enabled = true;
                    
                      
				}
		}

		public void HumanCardPlayEnd (GameObject cardObject, Vector3 position)
		{
				cardObject.GetComponent<CardMoover> ().enabled = false;
				Destroy (cardObject, 2);
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
						GUILayout.Box (gm.Winner + " is WIN");
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


		}

		void UpdateGameParameters ()
		{
				Dictionary<Specifications,int> humanparam = gm.GetPlayerParams (SelectPlayer.First);
				Dictionary<Specifications,int> enemyparam = gm.GetPlayerParams (SelectPlayer.Second);
		
				gameObject.GetComponent<GUIScript> ().PlayerTower.text = humanparam [Specifications.PlayerTower].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerWall.text = humanparam [Specifications.PlayerWall].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerDiamonds.text = humanparam [Specifications.PlayerDiamonds].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerAnimal.text = humanparam [Specifications.PlayerAnimals].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerRock.text = humanparam [Specifications.PlayerRocks].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerMine.text = humanparam [Specifications.PlayerColliery].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerMagic.text = humanparam [Specifications.PlayerDiamondMines].ToString ();
				gameObject.GetComponent<GUIScript> ().PlayerMenagerie.text = humanparam [Specifications.PlayerMenagerie].ToString ();
				
				gameObject.GetComponent<GUIScript> ().EnemyTower.text = enemyparam [Specifications.PlayerTower].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyWall.text = enemyparam [Specifications.PlayerWall].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyDiamonds.text = enemyparam [Specifications.PlayerDiamonds].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyAnimal.text = enemyparam [Specifications.PlayerAnimals].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyRock.text = enemyparam [Specifications.PlayerRocks].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyMine.text = enemyparam [Specifications.PlayerColliery].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyMagic.text = enemyparam [Specifications.PlayerDiamondMines].ToString ();
				gameObject.GetComponent<GUIScript> ().EnemyMenagerie.text = enemyparam [Specifications.PlayerMenagerie].ToString ();
				
				GameObject[] PlayerCards = GameObject.FindGameObjectsWithTag ("Card");
        

				foreach (GameObject card in PlayerCards) {
						card.GetComponent<DoneCardScript> ().CardIsActive = gm.IsCanUseCard (card.GetComponent<DoneCardScript> ().thisCard.cardParams);
				}

				PlayerTower.GetComponent<BuildingsScript> ().Height = humanparam [Specifications.PlayerTower];
				EnemyTower.GetComponent<BuildingsScript> ().Height = enemyparam [Specifications.PlayerTower];
				PlayerWall.GetComponent<BuildingsScript> ().Height = humanparam [Specifications.PlayerWall];
				EnemyWall.GetComponent<BuildingsScript> ().Height = enemyparam [Specifications.PlayerWall];

		}

	

		public void AICardEndPlay (GameObject cardObject)
		{
				cardObject.GetComponent<AICardMoover> ().enabled = false;
				Destroy (cardObject, 2);
				Dictionary<string, object> notify = new Dictionary<string, object> ();
				notify.Add ("CurrentAction", CurrentAction.AIMoveIsAnimated);
				gm.SendGameNotification (notify);
		}

		// Update is called once per frame
    private void Update()
    {
        UpdateGameParameters();

        if (!audio.isPlaying)
            playRandomMusic();

        CurrentAction prev_action = curr;
        curr = gm.Status;

        switch (curr)
        {
            case CurrentAction.WaitHumanMove:
            {

                if (prev_action != curr)
                {
                    PushCardOnDeck(new Vector3());
                }
                break;
            }
            case CurrentAction.HumanUseCard:
            {
                gameScreenText.guiText.enabled = false;
                break;
            }
            case CurrentAction.PassStroke:
            {

                break;
            }
            case CurrentAction.PlayerMustDropCard:
            {
                gameScreenText.guiText.text = "You need to drop a card";
                gameScreenText.guiText.enabled = true;
                break;
            }
            case CurrentAction.UpdateStatHuman:
            {
                Dictionary<string, object> notify = new Dictionary<string, object>();
                notify.Add("CurrentAction", CurrentAction.EndHumanMove);
                gm.SendGameNotification(notify);
                break;
            }
            case CurrentAction.AIUseCardAnimation:
            {
                if (prev_action != curr)
                {
                    List<Card> AICardsPlayed = gm.GetAIUsedCard();
                    int x = 0;
                    foreach (Card card in AICardsPlayed)
                    {
                        var vect = new Vector3(x, 2f, 0f);
                        CreateCard(card, ref vect, true);
                        x += 5;
                    }
                }
                break;
            }
            case CurrentAction.UpdateStatAI:
            {
                Dictionary<string, object> notify = new Dictionary<string, object>();
                notify.Add("CurrentAction", CurrentAction.EndAIMove);
                gm.SendGameNotification(notify);
                break;
            }
            case CurrentAction.EndGame:
            {
                if (prev_action != curr)
                {
                    EndGame();
                }
                break;
            }
        }

    }

}







