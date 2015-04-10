using UnityEngine;
using System.Collections;

public class TutorialSandShark : Enemy {
	private MeshCollider terrain;
	public GameObject target;

	private float timer;
	private float r;

	public GameObject body;
	public SceneTransitionController stc;

	// Use this for initialization
	void Start () {
		//this.terrain = (MeshCollider) GameObject.FindGameObjectWithTag("Terrain").GetComponent<Collider>();
		this.target = GameObject.FindGameObjectWithTag("Player");

		this.timer = Random.Range(0, 7.0f);

		r = Random.Range(-3.0f, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		// Spin the sandshark - will probably remove if it doesn't work with Model to be used
		body.transform.Rotate(Time.deltaTime*500,Time.deltaTime*500,Time.deltaTime*500);
	}

	void OnDestroy() {
		stc.fadeOut = true;
	}
}
