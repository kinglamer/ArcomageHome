using UnityEngine;
using System.Collections;

public class AICardMoover : MonoBehaviour
{
	
		public float step;
		Vector3 target;
		GameObject gameController;
		float starttime;

		// Use this for initialization
		void Start ()
		{
				gameController = GameObject.FindWithTag ("GameController");
				transform.Translate (Vector3.back * 2f);
				target = new Vector3 (12f, 0f, transform.position.z);
				starttime = Time.time;
		}
	
		// Update is called once per frame
		void Update ()
		{
				float lifetime = Time.time - starttime;
				if (lifetime > 2f) {
						transform.position = Vector3.MoveTowards (transform.position, target, step * Time.deltaTime);
						if (Vector3.Distance (transform.position, target) == 0f) {
								gameController.GetComponent<SceneScript> ().AICardEndPlay (gameObject);
						}
				}
		}
}
