using UnityEngine;
using System.Collections;

public class SpearController : MonoBehaviour {

	public WiiMoteInputController WiiInput;
	public Transform shaft;
	public Transform spearHead;

	public float yawMultiplier = 90.0f;

	// Used for smoothing with WiiMote input
	private float lastPitch;
	private float lastYaw;
	private float lastRoll;

	public bool isAttacking;
	public bool isReturning;

	private bool firstAttack;
	private float attackTime;
	public Transform currTarget;

	// Used for returning spear
	public Vector3 originalPosition;
	public Vector3 originalRotation;

	public Renderer shaftRenderer;

	// Use this for initialization
	void Start () {
		lastPitch = 90.0f + WiiInput.GetAxis("Pitch");
		lastYaw  = WiiInput.GetAxis("Yaw")*yawMultiplier;

		attackTime = 5.0f;
		isAttacking = false;
		isReturning = false;

		firstAttack = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Update blending of spear shader based on height
		shaftRenderer.material.SetFloat("_Blend", Mathf.Clamp01(this.transform.position.y/500));

		// Take in and smooth WiiInput
		lastPitch = Mathf.Lerp(lastPitch, 90.0f + WiiInput.GetAxis("Pitch"), Time.deltaTime*3.0f);
		lastYaw = Mathf.Lerp(lastYaw, WiiInput.GetAxis("Yaw")*yawMultiplier, Time.deltaTime*3.0f);

		// Move spear according to input if not attacking
		if (!isAttacking)
			gameObject.transform.localEulerAngles = new Vector3(lastPitch, lastYaw, 0.0f);

		// If attack input registered, store current transform info, then begin attacking
		if (((WiiInput.GetButton("A") && WiiInput.GetButton("B") && Mathf.Abs(WiiInput.GetAxis("Roll")) > 9.0f) 
		    || Input.GetMouseButtonDown(0)) && !isAttacking && !isReturning) {
			originalPosition = spearHead.localPosition;
			originalRotation = spearHead.localEulerAngles;
			isAttacking = true;
		}

		// Attack the target currently inside attack range
		if (isAttacking) {
			if (currTarget) {
				if (firstAttack) {
					try {
						currTarget.gameObject.GetComponentInParent<SandShark>().freeze();
					}
					catch {}
					try {
						currTarget.gameObject.GetComponentInParent<SandWorm>().freeze();
					}
					catch{}
					this.GetComponent<AudioSource>().Play();
					firstAttack = false;
				}
				attack (currTarget);
			}
			else {
				isAttacking = false;
			}
		}
		else if(isReturning){
			spearHead.localPosition = Vector3.Lerp(spearHead.localPosition, originalPosition, Time.deltaTime*5.0f);
			spearHead.localEulerAngles = Vector3.Lerp(spearHead.localEulerAngles, originalRotation, Time.deltaTime*5.0f);
			if (Vector3.Distance(spearHead.localPosition,originalPosition) < .2f) {
				// Spear is close enough, lock it to correct transform
				spearHead.localPosition = originalPosition;
				spearHead.localEulerAngles = originalRotation;
				isReturning = false;
			}
		}
	}

	void attack(Transform target) {
		spearHead.position = Vector3.Lerp(spearHead.position, target.position, Time.deltaTime*10.0f);
		spearHead.localEulerAngles = Vector3.Lerp(spearHead.localEulerAngles, new Vector3(80,90,180), Time.deltaTime*5.0f);
		if (Vector3.Distance(spearHead.position,target.position) < .2f) {
			this.firstAttack = true;
			currTarget = null;
			isAttacking = false;
			Destroy(target.parent.gameObject);
			isReturning = true;
		}
	}


	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Enemy") {
		currTarget = coll.transform;
		}
	}

	void OnTriggerExit(Collider coll) {
		if (!isAttacking)
			currTarget = null;
	}
}