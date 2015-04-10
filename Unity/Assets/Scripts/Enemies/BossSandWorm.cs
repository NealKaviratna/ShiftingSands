using UnityEngine;
using System.Collections;

public class BossSandWorm : Enemy {
	private MeshCollider terrain;
	public GameObject target;
	public GameObject[] waypoints;
	public int curWaypoint;
	public int length;
	
	private float timer;
	private float r;
	
	public AudioSource playerdeath;
	public bool isDeadly;
	
	// Use this for initialization
	void Start () {
		this.terrain = (MeshCollider) GameObject.FindGameObjectWithTag("Terrain").GetComponent<Collider>();
		
		this.timer = Random.Range(0, 7.0f);
		
		r = Random.Range(-3.0f, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		// Waypointing
		if (Vector3.Distance(this.target.transform.position, this.transform.position) < 20.0f) {
			if (curWaypoint == this.length) curWaypoint = -1;
			target = waypoints[++curWaypoint];
		}
		this.findPlayer();
	}
	
	State findPlayer() {
		// Turn towards target
		Quaternion q = Quaternion.LookRotation(target.transform.position - this.transform.position);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, q, .015f);
		this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
		
		// Move forward
		transform.Translate(Vector3.forward * 45 * Time.deltaTime);
		
		// Lock Sandworm to desert terrain - TODO: account for other objects in desert
		RaycastHit hit = new RaycastHit();
		Ray r = new Ray(new Vector3(this.transform.position.x,100,this.transform.position.z), Vector3.down);
		terrain.Raycast(r,out hit, 100);
		this.transform.position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);
		
		// State transitions
		if (Vector3.Distance(this.transform.position,this.target.transform.position) < 15.0f)
			return State.searching;
		else 
			return State.searching;
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.collider.tag == "Player" && this.isDeadly) {
			this.playerdeath.Play();
			FindObjectOfType<SceneTransitionController>().fadeOut = true;
		}
	}
}
