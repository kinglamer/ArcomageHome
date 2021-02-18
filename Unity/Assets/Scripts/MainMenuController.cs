using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

		public GUISkin GUISkin;
	public TextAsset HelpText;
	public TextAsset AboutText;

		private bool isHelp;
		private bool isAbout;
		

	Vector2 scrollPosition;

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
					SceneManager.LoadScene(1);
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
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400), GUILayout.Height(400));
			GUILayout.Label (HelpText.text, "SimpleLable", GUILayout.MaxWidth (400));
			GUILayout.EndScrollView();
						if (GUILayout.Button ("Close"))
								isHelp = false;
				}
				if (windowID == 1) {
						GUILayout.Space (20);
						GUILayout.Label (AboutText.text, GUILayout.MaxWidth (400));
						if (GUILayout.Button ("Close"))
								isAbout = false;
				}
			
		}
}
