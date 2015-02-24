using UnityEngine;
using System.Collections;

public class SandShark : Enemy {
	private MeshCollider terrain;
	private GameObject target;

	private float timer;
	private float r;

	// Use this for initialization
	void Start () {
		this.terrain = (MeshCollider) GameObject.FindGameObjectWithTag("Terrain").collider;
		this.target = GameObject.FindGameObjectWithTag("Player");

		this.timer = Random.Range(0, 7.0f);

		r = Random.Range(-3.0f, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		// Spin the sandshark - will probably remove if it doesn't work with Model to be used
		this.transform.Rotate(Time.deltaTime*500,Time.deltaTime*500,Time.deltaTime*500);

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
		// Move towards target
		this.transform.position = Vector3.Lerp(this.transform.position,
		                                       target.transform.position,
		                                       this.speed*Time.deltaTime/50.0f);

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
		this.transform.position = Vector3.Lerp(this.transform.position,
		                                       new Vector3(target.transform.position.x + 12.0f*Mathf.Sin(Time.time), 
		            									   0,
		            									   target.transform.position.z + 12.0f*Mathf.Cos(Time.time)),
		            						   this.speed*Time.deltaTime/20.0f);

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
}
