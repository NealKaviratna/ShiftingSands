using UnityEngine;
using System.Collections;

public class EndSetup : MonoBehaviour {

	public GameObject enemies;
	public BossSandWorm boss;
	public AudioSource feet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			this.enemies.SetActive(false);
			this.boss.isDeadly = false;
			this.feet.enabled = false;
		}
	}
}
