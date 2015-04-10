using UnityEngine;
using System.Collections;

public class BossChanger : MonoBehaviour {

	public BossSandWorm boss;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			boss.curWaypoint = 0;
			boss.target = boss.waypoints[0];
			boss.length = 2;
		}
	}
}
