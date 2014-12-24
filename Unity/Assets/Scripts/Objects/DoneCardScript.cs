using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Arcomage.Core;
using Arcomage.Entity;

public class DoneCardScript : MonoBehaviour
{

		private Vector3 screenPoint;
		private Vector3 offset;
		private Vector3 startPos;
		private bool cardisactiv;
    

        
    public bool CardIsActive
    {
        get { return cardisactiv; }
        set
        {
            if (value)
                this.renderer.material.SetFloat("_Light", 0.7f);
            else
                this.renderer.material.SetFloat("_Light", 0.2f);
            cardisactiv = value;
        }
    }

    public Card thisCard{ get; set; }

		public TextMesh CardName;
		public TextMesh CardParameter;
		public TextMesh CardCost;
	
		public string cardName { get { return CardName.text; } set { CardName.text = value; } }

		public string cardParam { get { return CardParameter.text; } set { CardParameter.text = value; } }

		public int cardCost { get { return int.Parse (CardCost.text); } set { CardCost.text = "" + value; } }

		public int cardId { get; set; }
        
         public List<CardParams> ListOfParamses { get; set; }

		GameObject gameController;
		Vector3 curPosition;
		float currentTime = 0;
		float lastClickTime = 0;
		float clickTime = 0.3F;
	
		void Start ()
		{
				gameController = GameObject.FindWithTag ("GameController");
				startPos = transform.position;
		}

		void OnMouseEnter ()
		{
				if (CardIsActive) {
						this.renderer.material.SetFloat ("_Light", 1f);
				}
		
		}

    private void OnMouseOver()
    {
        if (CardIsActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentTime = Time.time;
                if ((currentTime - lastClickTime) < clickTime)
                {
                    gameController.GetComponent<SceneScript>().CardPlayed(cardId, startPos, gameObject);
                    audio.PlayOneShot(SceneScript.AudioClips[SoundTypes.card]);
                    PlaySecondSound();
                }
                lastClickTime = currentTime;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            gameController.GetComponent<SceneScript>().PassMove(cardId, startPos, gameObject);
            audio.PlayOneShot(SceneScript.AudioClips[SoundTypes.card]);
        }
    }

    private void PlaySecondSound()
    {
        var item = ListOfParamses.LastOrDefault();

        SoundTypes typeS = SoundTypes.None;

        switch (item.key)
        {
            case Specifications.PlayerTower:
            case Specifications.EnemyTower:
                if(item.value > 0)
                    typeS = SoundTypes.towerup;
                else
                {
                    typeS = SoundTypes.damage2;
                }
                break;
            case Specifications.PlayerWall:
            case Specifications.EnemyWall:
                if (item.value > 0)
                    typeS = SoundTypes.wallup;
                else
                {
                    typeS = SoundTypes.damage;
                }
                break;
 
            case Specifications.PlayerColliery:
            case Specifications.EnemyColliery:
                if (item.value < 0)
                    typeS = SoundTypes.bricksdown;
                else
                {
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
                else
                {
                    typeS = SoundTypes.harp;
                }
                break;
            case Specifications.EnemyDiamondMines:
            case Specifications.PlayerDiamondMines:
            case Specifications.PlayerMenagerie:
            case Specifications.EnemyMenagerie:
                if (item.value < 0)
                    typeS = SoundTypes.resourceloss;
                else
                {
                    typeS = SoundTypes.towerwallgain;
                }
                break;
            case Specifications.EnemyDirectDamage:
            case Specifications.PlayerDirectDamage:
                typeS = SoundTypes.damage;
                break;

        }

        if (typeS != SoundTypes.None)
        {
            audio.PlayOneShot(SceneScript.AudioClips[typeS], 0.7f);
        }
    }

    void OnMouseExit ()
		{	
				if (CardIsActive) {
						this.renderer.material.SetFloat ("_Light", 0.7f);
				}
		}

		void Update ()
		{
				
		}

		//Устанавливает рубашку и картинку карты CardBack (RGB 2,1,0 соответственно), Pic - Резерв
		public void SetCardGraph (int CardBack, Texture2D Pic)
		{
				renderer.material.SetFloat ("_CardBackColor", CardBack);
				renderer.material.SetTexture ("_Picture", (Texture)Pic);
		}

}
