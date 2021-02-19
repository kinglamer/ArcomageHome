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
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;
    

    public float SWIPE_THRESHOLD = 20f;



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

    void OnSwipeUp()
    {
       // Debug.Log("Swipe UP");
    }

    void OnSwipeLeft()
    {
        //Debug.Log("Swipe Left");
    }

    void OnSwipeRight()
    {
        //Debug.Log("Swipe Right");
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
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
        audios = gameObject.AddComponent<AudioSource>();

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
            var sound = SceneScript.AudioClips[SoundTypes.card];
            audios.PlayOneShot(sound);
        }
    }

    void OnSwipeDown()
    {
        //Debug.Log("Swipe work");
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
        


        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }
    }

    //Устанавливает рубашку и картинку карты CardBack (RGB 2,1,0 соответственно), Pic - Резерв
    public void SetCardGraph(int CardBack, Texture2D Pic)
    {
        rend.material.SetFloat("_CardBackColor", CardBack);
        rend.material.SetTexture("_Picture", (Texture)Pic);
    }

}
