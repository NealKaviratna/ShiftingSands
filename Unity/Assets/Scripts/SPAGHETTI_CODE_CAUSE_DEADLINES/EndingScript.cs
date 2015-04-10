using UnityEngine;
using System.Collections;

public class EndingScript : MonoBehaviour {

	private bool activated;
	public Transform player;

	private Texture whiteTexture;
	public AudioSource endSpeech;
	public AudioSource monsterScream;
	public GUIText thanks;
	private bool over;

	private float timer;

	private bool first;
	private bool second;

	// Use this for initialization
	void Start () {
		first = false;
		second = false;

		activated = false;
		over = false;
		whiteTexture = Resources.Load("White_Texture") as Texture;
		timer = 0.0f;
	}

	// Update is called once per frame
	void Update () {
		if (this.over) {
			this.timer += Time.deltaTime;

			if (this.timer > 0.5f && !this.first) {
				this.monsterScream.Play();
				this.first = true;
			}
			if (this.timer > 4.0f && !this.second) {
				this.endSpeech.Play();
				this.second = true;
			}
			if (this.timer > 10.0f) {
				Application.LoadLevel(5);
			}
		}
	}

	void OnGUI() {
		if (this.over){
			GUI.color = new Color(1, 1, 1, 1);
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), whiteTexture );
		}
		else if (this.activated) {
			GUI.color = new Color(1, 1, 1, Mathf.Clamp01((220 - player.position.y)/160));
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), whiteTexture );

			if (Mathf.Clamp01((220 - player.position.y)/160) >= .835) {
				this.over = true;
			}
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			this.activated = true;
			coll.GetComponent<Footsteps>().enabled = false;
		}
	}
}
