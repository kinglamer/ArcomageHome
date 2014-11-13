using UnityEngine;
using System.Collections;

public class DoneCardScript : MonoBehaviour
{

		[System.Serializable]
		public class Boundary
		{
				public float xMin, xMax, yMin, yMax;
		}
	
		private Vector3 screenPoint;
		private Vector3 offset;
		private Vector3 startPos;
		//		private CardParametrs parametrs ;
	
		public Boundary boundary;
		public TextMesh CardName;
		public TextMesh CardParameter;
		public TextMesh CardCost;
	
		public string cardName{ get; set; }

		public string cardParam{ get; set; }

		public int cardCost{ get; set; }

		public int cardId { get; set; }

		GameObject gameController;
		Vector3 curPosition;
		float currentTime = 0;
		float lastClickTime = 0;
		float clickTime = 0.3F;
	
		void Start ()
		{
				//parametrs = new CardParametrs ();
				gameController = GameObject.FindWithTag ("GameController");
				startPos = transform.position;
		
		}
	
		void OnMouseDown ()
		{
		
				screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		
				offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		
		}
	
		void OnMouseDrag ()
		{
				Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
		
				curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
		
				transform.position = new Vector3
			(
				Mathf.Clamp (curPosition.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp (curPosition.y, boundary.yMin, boundary.yMax),
				transform.position.z
				);
		
		}
	
		void OnMouseEnter ()
		{
				transform.Translate (Vector3.back * 5f);
				this.renderer.material.SetFloat ("_Light", 1f);
		
		}
	
		void OnMouseOver ()
		{
				if (Input.GetMouseButtonDown (0)) {
						currentTime = Time.time;
						if ((currentTime - lastClickTime) < clickTime) {
								gameController.GetComponent<SceneScript> ().CardPlayed (cardId, startPos);
								Destroy (gameObject);
						}
						lastClickTime = currentTime;
				}
		if(Input.GetMouseButtonDown (1)){
			gameController.GetComponent<SceneScript>().EnemyMove();
			Destroy (gameObject);
			Debug.Log ("Pass!");
		}
		}
	
		void OnMouseExit ()
		{	
				transform.Translate (Vector3.forward * 5f);
				this.renderer.material.SetFloat ("_Light", 0.7f);
		}

		void OnMouseUp ()
		{
				if (curPosition.y > -5.5f) {
						gameController.GetComponent<SceneScript> ().CardPlayed (cardId, startPos);
						Destroy (gameObject);
				}
		}

		void Update ()
		{
				CardName.text = cardName;
				CardParameter.text = cardParam;
				CardCost.text = "" + cardCost;
				
		}
		//Устанавливает рубашку и картинку карты CardBack (RGB 2,1,0 соответственно), Pic - Резерв
		public void SetCardGraph (int CardBack, Vector2 Pic)
		{
				renderer.material.SetFloat ("_CardBackColor", CardBack);
		}

}
