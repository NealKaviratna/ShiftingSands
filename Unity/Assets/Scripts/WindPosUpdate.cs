﻿using UnityEngine;
using System.Collections;

public class WindPosUpdate : MonoBehaviour {

	public AudioSource windSource;
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x = pos.x + 20;
		windSource.transform.position = pos;
	}
}
