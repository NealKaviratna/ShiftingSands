using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {

	public AudioSource foot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<CharacterController>().velocity != Vector3.zero && !foot.isPlaying)
			foot.Play();
		else if (this.GetComponent<CharacterController>().velocity == Vector3.zero && foot.isPlaying
		         || Random.Range(0,100) > 99)
			foot.Stop();
	}
}
