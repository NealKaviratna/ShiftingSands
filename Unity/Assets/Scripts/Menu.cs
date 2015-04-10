using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	
	Texture2D button;

	// Use this for initialization
	void Start () {
		button = Resources.Load("button") as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
//		GUI.skin.button.normal.background = null;
		GUI.skin.button.hover.background = button;
//		GUI.skin.button.active.background = null;
//		GUI.skin.button.font = f;
		GUI.skin.button.fontSize = 30;
		//if(!showOptions){
			//GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), title);
			
			if(GUI.Button(new Rect(408, 362, button.width, button.height), "Play")) {
				FindObjectOfType<SceneTransitionController>().levelToLoad = 1;
				FindObjectOfType<SceneTransitionController>().fadeOut = true;
			}
			if(GUI.Button(new Rect(408, 423, button.width, button.height), "Credits")) {
				FindObjectOfType<SceneTransitionController>().levelToLoad = 4;
				FindObjectOfType<SceneTransitionController>().fadeOut = true;
			}
			
			//GUI.BeginGroup(new Rect(0f, 0f, Screen.width, Screen.height));
			//GUIUtility.RotateAroundPivot(rotateVal, new Vector2(Screen.width/2, Screen.height/2));
			//GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), ring);
			//GUIUtility.RotateAroundPivot(-rotateVal, new Vector2(Screen.width/2, Screen.height/2));
			//GUI.EndGroup();
		//}
		//else{
		//	GUI.skin.button.normal.background = (Texture2D)buttonBack;
		//	GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), options);
		//	if(GUI.Button(new Rect(408, 500, button.width, button.height), "Back"))
		//		showOptions = false;
		//}
	}
}
