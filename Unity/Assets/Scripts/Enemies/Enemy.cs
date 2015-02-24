using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health;
	public float speed;

	public enum State {searching, waiting, attacking, frozen};

	public State state;

	// Use this for initialization
	void Start () {
		this.health = 100.0f;
		this.speed = 10.0f;

		this.state = State.searching;
	}
	
	// Update is called once per frame
	void Update () {
		switch (this.state) {
		case State.searching:
			this.state = this.findPlayer();
			break;
		case State.attacking:
			this.state = this.combat();
			break;
		default:
			this.state = State.searching;
			break;
		}
	}

	State findPlayer() {
		return 0;
	}

	State combat() {
		return 0;
	}

	public void freeze() {
		this.state = State.frozen;
	}
}
