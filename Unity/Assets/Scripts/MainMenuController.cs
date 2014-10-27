using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour
{

		public GUISkin GUISkin;
		private bool isHelp;
		private bool isAbout;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{

		}

		void OnGUI ()
		{
				GUI.skin = GUISkin;
				GUILayout.BeginArea (new Rect (Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 200));
				if (GUILayout.Button ("Start Game") && !isHelp && !isAbout) {
						Application.LoadLevel (1);
				}
				if (GUILayout.Button ("Help") && !isHelp && !isAbout) {
						isHelp = true;
				}
				if (isHelp) {
						GUILayout.Window (0, new Rect (Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400), DoHelpWindow, "Help");
				}

				if (GUILayout.Button ("About...") && !isHelp && !isAbout) {
						isAbout = true;
				}
				if (isAbout) {
						GUILayout.Window (1, new Rect (Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400), DoHelpWindow, "About");
				}

				if (GUILayout.Button ("Exit") && !isHelp && !isAbout) {
						Application.Quit ();
				}

				GUILayout.EndArea ();
		}

		void DoHelpWindow (int windowID)
		{
				if (windowID == 0) {
						GUILayout.Space (20);
						GUILayout.Label ("Help text", GUILayout.MaxWidth (400));
						if (GUILayout.Button ("Close"))
								isHelp = false;
				}
				if (windowID == 1) {
						GUILayout.Space (20);
						GUILayout.Label ("About text", GUILayout.MaxWidth (400));
						if (GUILayout.Button ("Close"))
								isAbout = false;
				}
			
		}
}
