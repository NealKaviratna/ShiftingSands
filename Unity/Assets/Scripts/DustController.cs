using UnityEngine;
using System.Collections;

public class DustController : MonoBehaviour {

	public Transform PlayerPos;
	public float Yoffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(PlayerPos.position.x + 15.74274f, 
		                                      PlayerPos.position.y + Yoffset, 
		                                      PlayerPos.position.z);
	}
}
