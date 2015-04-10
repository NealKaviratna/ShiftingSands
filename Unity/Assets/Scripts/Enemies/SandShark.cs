using UnityEngine;
using System.Collections;

public class SandShark : Enemy {
	private MeshCollider terrain;
	private GameObject target;

	private float timer;
	private float r;

	public GameObject body;

	public AudioSource playerdeath;

	// Use this for initialization
	void Start () {
		this.terrain = (MeshCollider) GameObject.FindGameObjectWithTag("Terrain").GetComponent<Collider>();
		this.target = GameObject.FindGameObjectWithTag("Player");

		this.timer = Random.Range(0, 7.0f);

		r = Random.Range(-3.0f, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		// Spin the sandshark - will probably remove if it doesn't work with Model to be used
		body.transform.Rotate(Time.deltaTime*500,Time.deltaTime*500,Time.deltaTime*500);

		// State-Machine
		switch (this.state) {
		case State.searching:
			this.state = this.findPlayer();
			break;
		case State.waiting:
			this.state = this.circlePlayer();
			break;
		case State.attacking:
			this.state = this.combat();
			break;
		case State.frozen:
			break;
		default:
			this.state = State.searching;
			break;
		}	
	}
	
	State findPlayer() {
		// Turn towards target
		Quaternion q = Quaternion.LookRotation(target.transform.position - this.transform.position);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, q, .015f);
		this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
		
		// Move forward
		transform.Translate(Vector3.forward * this.speed * Time.deltaTime);

		// Lock Sandshark to desert terrain - TODO: account for other objects in desert
		RaycastHit hit = new RaycastHit();
		Ray r = new Ray(new Vector3(this.transform.position.x,100,this.transform.position.z), Vector3.down);
		terrain.Raycast(r,out hit, 100);
		this.transform.position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);

		// State transitions
		if (Vector3.Distance(this.transform.position,this.target.transform.position) < 15.0f)
			return State.waiting;
		else 
			return State.searching;
	}

	// This whole function is jankily rewritten TODO: Fix circlePlayer()
	State circlePlayer() {
		Vector3 displacement =  this.transform.position-target.transform.position;
		Vector3 newPos = Vector3.Slerp (displacement, Vector3.Cross(displacement,Vector3.up), this.speed*Time.deltaTime/20.0f);
		this.transform.position = newPos + target.transform.position;

		// Lock Sandshark to desert terrain - TODO: account for other objects in desert
		RaycastHit hit = new RaycastHit();
		Ray r = new Ray(new Vector3(this.transform.position.x,100,this.transform.position.z), Vector3.down);
		terrain.Raycast(r,out hit, 100);
		this.transform.position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);

		// State transitions
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer = 3.0f;
			return State.attacking;
		}
		else 
			return State.waiting;
	}
	
	State combat() {
		this.transform.position = Vector3.Lerp(this.transform.position,target.transform.position,this.speed*Time.deltaTime/10.0f);

		// Keep sandshark 2 world units above Sand
		RaycastHit hit = new RaycastHit();
		Ray r = new Ray(new Vector3(this.transform.position.x,100,this.transform.position.z), Vector3.down);
		terrain.Raycast(r,out hit, 100);
		Vector3 dest = new Vector3(this.transform.position.x, hit.point.y + 2.0f, this.transform.position.z);
		this.transform.position = Vector3.Lerp(this.transform.position,dest,Time.deltaTime);
		
		return State.attacking;
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			this.playerdeath.Play();
			FindObjectOfType<SceneTransitionController>().fadeOut = true;
		}
	}
}
