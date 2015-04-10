using UnityEngine;
using System.Collections;

public class MonsterDeathNoise : MonoBehaviour {

	public AudioSource death;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy() {
		death.Play();
	}
}
