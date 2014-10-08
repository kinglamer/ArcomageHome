using UnityEngine;
using System.Collections;

public class QuadCardController : MonoBehaviour {

	[System.Serializable]
	public class Boundary
	{
		public float xMin, xMax, yMin, yMax;
	}
	
	private Vector3 screenPoint;
	private Vector3 offset;
	//		private CardParametrs parametrs ;
	
	public Boundary boundary;
	
	public Transform namePlace;
	public Transform paramPlace;
	public Transform costPlace;
	
	public GUISkin Name_GUISkin;
	public GUISkin Parameters_GUISkin;
	public GUISkin Cost_GUISkin;
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
		transform.Translate (Vector3.back);
		
	}
	
	void OnMouseOver ()
	{
		this.GetComponentInChildren<Light> ().enabled = true;
		
	}
	
	void OnMouseExit ()
	{	
		this.GetComponentInChildren<Light> ().enabled = false;
		transform.Translate (Vector3.forward);
	}
	
	void OnGUI(){
		Rect nameRec = new Rect ();
		nameRec = Rect.MinMaxRect 
			(Camera.main.WorldToScreenPoint(namePlace.collider2D.bounds.min).x,
			 Camera.main.pixelHeight-Camera.main.WorldToScreenPoint(namePlace.collider2D.bounds.max).y,
			 Camera.main.WorldToScreenPoint(namePlace.collider2D.bounds.max).x,
			 Camera.main.pixelHeight-Camera.main.WorldToScreenPoint(namePlace.collider2D.bounds.min).y
			 );
		
		GUILayout.BeginArea (nameRec);
		GUI.skin = Name_GUISkin;
		GUILayout.Label(cardName);
		GUILayout.EndArea ();
		
		Rect paramRec = new Rect ();
		paramRec = Rect.MinMaxRect 
			(Camera.main.WorldToScreenPoint(paramPlace.collider2D.bounds.min).x,
			 Camera.main.pixelHeight-Camera.main.WorldToScreenPoint(paramPlace.collider2D.bounds.max).y,
			 Camera.main.WorldToScreenPoint(paramPlace.collider2D.bounds.max).x,
			 Camera.main.pixelHeight-Camera.main.WorldToScreenPoint(paramPlace.collider2D.bounds.min).y
			 );
		
		GUILayout.BeginArea (paramRec);
		GUI.skin = Parameters_GUISkin;
		GUILayout.Label(cardParam+"\n"+cardParam+"\n"+cardParam);
		GUILayout.EndArea ();
		
		Rect costRec = new Rect ();
		costRec = Rect.MinMaxRect 
			(Camera.main.WorldToScreenPoint(costPlace.collider2D.bounds.min).x,
			 Camera.main.pixelHeight-Camera.main.WorldToScreenPoint(costPlace.collider2D.bounds.max).y,
			 Camera.main.WorldToScreenPoint(costPlace.collider2D.bounds.max).x,
			 Camera.main.pixelHeight-Camera.main.WorldToScreenPoint(costPlace.collider2D.bounds.min).y
			 );
		
		GUILayout.BeginArea (costRec);
		GUI.skin = Cost_GUISkin;
		GUILayout.Label(""+cardCost);
		GUILayout.EndArea ();
		
	}
}
