using UnityEngine;
using System.Collections;

public class CardPassMoover : MonoBehaviour
{

		public float step;
		Vector3 target;
		GameObject gameController;
		GameObject passText;
	
		// Use this for initialization
		void Start ()
		{
				gameController = GameObject.FindWithTag ("GameController");
				passText = GameObject.Find ("PassText");
				transform.Translate (Vector3.back * 2f);
				target = new Vector3 (transform.position.x, -25f, transform.position.z);
				passText.guiText.enabled = true;
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.position = Vector3.MoveTowards (transform.position, target, step * Time.deltaTime);
				if (Vector3.Distance (transform.position, target) == 0f) {
						gameController.GetComponent<SceneScript> ().HumanCardPassEnd (gameObject);
				}
		}

		void OnDisable ()
		{
				passText.guiText.enabled = false;
		}

}
