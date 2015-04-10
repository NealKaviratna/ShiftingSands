using UnityEngine;
using System.Collections;

public class TowerHintSpeech : MonoBehaviour {

	public AudioSource speech;

	private float timer;

	// Use this for initialization
	void Start () {
		this.timer = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		this.timer -= Time.deltaTime;
		if (this.timer <= 0) {
			this.speech.Play();
			this.timer = 100000000000000.0f;
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			this.speech.Play();
		}
	}
}
