using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Arcomage.Core;
using Arcomage.Core.Common;
using Arcomage.Entity;
using Arcomage.Entity.Cards;

public class DoneCardScript : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 startPos;
    private bool cardisactiv;
    AudioSource audios;
    Renderer rend;



    public bool CardIsActive
    {
        get { return cardisactiv; }
        set
        {
            if (value)
                rend.material.SetFloat("_Light", 0.7f);
            else
                rend.material.SetFloat("_Light", 0.2f);
            cardisactiv = value;
        }
    }

    public Card thisCard { get; set; }

    public TextMesh CardName;
    public TextMesh CardParameter;
    public TextMesh CardCost;

    public string cardName { get { return CardName.text; } set { CardName.text = value; } }

    public string cardParam { get { return CardParameter.text; } set { CardParameter.text = value; } }

    public int cardCost { get { return int.Parse(CardCost.text); } set { CardCost.text = "" + value; } }

    public int cardId { get; set; }

    public List<CardParams> ListOfParamses { get; set; }

    GameObject gameController;
    Vector3 curPosition;
    float currentTime = 0;
    float lastClickTime = 0;
    float clickTime = 0.3F;
    void Awake() {
        rend = GetComponent<Renderer>();
    }
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        startPos = transform.position;
        audios = new AudioSource();

    }

    void OnMouseEnter()
    {
        if (CardIsActive)
        {
            rend.material.SetFloat("_Light", 1f);
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

                }
                lastClickTime = currentTime;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            gameController.GetComponent<SceneScript>().PassMove(cardId, startPos, gameObject);
            audios.PlayOneShot(SceneScript.AudioClips[SoundTypes.card]);
        }
    }



    void OnMouseExit()
    {
        if (CardIsActive)
        {
            rend.material.SetFloat("_Light", 0.7f);
        }
    }

    void Update()
    {

    }

    //Устанавливает рубашку и картинку карты CardBack (RGB 2,1,0 соответственно), Pic - Резерв
    public void SetCardGraph(int CardBack, Texture2D Pic)
    {
        rend.material.SetFloat("_CardBackColor", CardBack);
        rend.material.SetTexture("_Picture", (Texture)Pic);
    }

}
