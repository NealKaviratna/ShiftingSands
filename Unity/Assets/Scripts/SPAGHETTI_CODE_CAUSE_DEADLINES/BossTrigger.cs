﻿using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour {

	public GameObject boss;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			boss.SetActive(true);
		}
	}
}
