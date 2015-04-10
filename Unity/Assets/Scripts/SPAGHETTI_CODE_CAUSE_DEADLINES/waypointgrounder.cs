using UnityEngine;
using System.Collections;

public class waypointgrounder : MonoBehaviour {
	private MeshCollider terrain;

	// Use this for initialization
	void Start () {
		this.terrain = (MeshCollider) GameObject.FindGameObjectWithTag("Terrain").GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit = new RaycastHit();
		Ray r = new Ray(new Vector3(this.transform.position.x,100,this.transform.position.z), Vector3.down);
		terrain.Raycast(r,out hit, 100);
		this.transform.position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);
	}
}
