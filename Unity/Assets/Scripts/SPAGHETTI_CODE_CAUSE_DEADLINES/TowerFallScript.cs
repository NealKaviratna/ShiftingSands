using UnityEngine;
using System.Collections;

public class TowerFallScript : MonoBehaviour {

	public bool isFalling;

	public Transform fallenPosition;

	private Vector3 originalPosition;
	private Quaternion originalQuaternion;

	private float t = 0;

	private AudioSource rumble;
	private bool fadeout;


	// Use this for initialization
	void Start () {
		this.originalPosition = this.transform.position;
		this.originalQuaternion = this.transform.rotation;
		this.rumble = this.GetComponent<AudioSource>();
		this.fadeout = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFalling) {
			t += Time.deltaTime/20;
			this.transform.position = Vector3.Slerp(originalPosition , fallenPosition.position, t);
			this.transform.rotation = Quaternion.Slerp(originalQuaternion , fallenPosition.rotation, t);
			if (t >= 1) {
				this.isFalling = false;
				if (this.rumble && this.rumble.isPlaying) this.fadeout = true;
			}
		}

		if (this.fadeout) {
			this.rumble.volume -= Time.deltaTime;
			if (this.rumble.volume <= 0) this.fadeout = false;
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			this.isFalling = true;
			if (this.rumble && !this.rumble.isPlaying) this.rumble.Play();
		}
	}
}
