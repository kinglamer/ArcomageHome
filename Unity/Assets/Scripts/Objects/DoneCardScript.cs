using UnityEngine;
using System.Collections;

public class DoneCardScript : MonoBehaviour {

	[System.Serializable]
	public class Boundary
	{
		public float xMin, xMax, yMin, yMax;
	}
	
	private Vector3 screenPoint;
	private Vector3 offset;
	//		private CardParametrs parametrs ;
	
	public Boundary boundary;

	public TextMesh CardName;
	public TextMesh CardParameter;
	public TextMesh CardCost;
	
	public string cardName{ get; set; }
	public string cardParam{ get; set; }
	public int cardCost{ get; set; }
	
	
	void Start ()
	{
		//parametrs = new CardParametrs ();
		
	}
	
	void OnMouseDown ()
	{
		
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		
	}
	
	void OnMouseDrag ()
	{
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
		
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint)+offset;
		
		transform.position = new Vector3
			(
				Mathf.Clamp (curPosition.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp (curPosition.y, boundary.yMin, boundary.yMax),
				transform.position.z
				);
		
	}
	
	void OnMouseEnter ()
	{
		transform.Translate (Vector3.back*5f);
		
	}
	
	void OnMouseOver ()
	{
		this.renderer.material.SetFloat ("_Light", 1f);
	}
	
	void OnMouseExit ()
	{	
		transform.Translate (Vector3.forward*5f);
		this.renderer.material.SetFloat ("_Light", 0.7f);
	}

	void Update()
	{
		CardName.text = cardName;
		CardParameter.text = cardParam;
		CardCost.text = "" + cardCost;
		}
}
