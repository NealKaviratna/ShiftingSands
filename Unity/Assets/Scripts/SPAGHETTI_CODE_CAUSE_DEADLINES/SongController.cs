using UnityEngine;
using System.Collections;

public class SongController : MonoBehaviour {

	private AudioSource theme;
	private float Timer;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		theme = this.GetComponent<AudioSource>();
		this.Timer = 5.0f;

		if (GameObject.FindGameObjectsWithTag("Music").Length == 2) {
			DestroyImmediate(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.theme.isPlaying && Application.loadedLevel == 0 || Application.loadedLevel == 5) {
			this.Timer -= Time.deltaTime;
			if (this.Timer <= 0) {
				this.Timer = 5.0f;
				this.theme.Play();
			}
		}
	}
}
