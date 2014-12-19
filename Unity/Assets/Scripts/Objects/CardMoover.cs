using UnityEngine;
using System.Collections;

public class CardMoover : MonoBehaviour
{
	
		public float step;
		Vector3 target;
		GameObject gameController;

		// Use this for initialization
		void Start ()
		{
				gameController = GameObject.FindWithTag ("GameController");
				transform.Translate (Vector3.back * 2f);
				target = new Vector3 (-12f, 0f, transform.position.z);
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.position = Vector3.MoveTowards (transform.position, target, step*Time.deltaTime);
				if (Vector3.Distance (transform.position, target)==0f) {
						gameController.GetComponent<SceneScript> ().HumanCardPlayEnd (gameObject);
				}
		}
}
