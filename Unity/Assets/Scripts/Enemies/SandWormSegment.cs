using UnityEngine;
using System.Collections;

public class SandWormSegment : MonoBehaviour {
	
	private MeshCollider terrain;
	public Transform leader;

	// Use this for initialization
	void Start () {
		this.terrain = (MeshCollider) GameObject.FindGameObjectWithTag("Terrain").GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion q = Quaternion.LookRotation(leader.transform.position - this.transform.position);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, q, .9f);
		this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, this.transform.eulerAngles.z);

		//Vector3 target = new Vector3(leader.localPosition.x, leader.localPosition.y, leader.localPosition.z-3f);
		this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, leader.localPosition, .03f);


		// Lock segment to desert terrain - TODO: account for other objects in desert
		RaycastHit hit = new RaycastHit();
		Ray r = new Ray(new Vector3(this.transform.position.x,100,this.transform.position.z), Vector3.down);
		terrain.Raycast(r,out hit, 100);
		this.transform.position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);
	}
}
