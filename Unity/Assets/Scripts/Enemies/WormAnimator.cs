using UnityEngine;
using System.Collections;

public class WormAnimator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(Vector3.forward,3.0f);
	}
}
