using UnityEngine;
using System.Collections;

public class speechScript : MonoBehaviour {

	public AudioSource speech;
	public float Timer;

	// Use this for initialization
	void Start () {
		Timer = 6.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Timer != 10.0f) Timer -= Time.deltaTime;
		if (Timer <= 0) {
			this.speech.Play();
			Timer = 10.0f;
		}
		if (!this.speech.isPlaying && Timer == 10.0f) {
			FindObjectOfType<SceneTransitionController>().levelToLoad = 2;
			FindObjectOfType<SceneTransitionController>().fadeOut = true;
		}
		Debug.Log(this.speech.isPlaying);
	}
}
