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

	private bool isAttacking;
	private bool isReturning;

	private bool firstAttack;
	private float attackTime;
	private Transform currTarget;

	// Used for returning spear
	private Vector3 originalPosition;
	private Vector3 originalRotation;

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
		// Take in and smooth WiiInput
		lastPitch = Mathf.Lerp(lastPitch, 90.0f + WiiInput.GetAxis("Pitch"), Time.deltaTime*3.0f);
		lastYaw = Mathf.Lerp(lastYaw, WiiInput.GetAxis("Yaw")*yawMultiplier, Time.deltaTime*3.0f);

		// Move spear according to input if not attacking
		if (!isAttacking)
			gameObject.transform.localEulerAngles = new Vector3(lastPitch, lastYaw, 0.0f);

		// If attack input registered, store current transform info, then begin attacking
		if (((WiiInput.GetButton("A") && WiiInput.GetButton("B") && Mathf.Abs(WiiInput.GetAxis("Roll")) > 9.0f) 
		    || Input.GetMouseButtonDown(0)) && !isAttacking) {
			originalPosition = spearHead.position;
			originalRotation = spearHead.localEulerAngles;
			isAttacking = true;
		}

		// Attack the target currently inside attack range
		if (isAttacking) {
			if (currTarget) {
				if (firstAttack) {
					currTarget.gameObject.GetComponentInParent<SandShark>().freeze();
					firstAttack = false;
				}
				attack (currTarget);
			}
			else {
				// TODO: implement whiff
			}
		}
		else if(isReturning){
			spearHead.position = Vector3.Lerp(spearHead.position, originalPosition, Time.deltaTime*5.0f);
			spearHead.localEulerAngles = Vector3.Lerp(spearHead.localEulerAngles, originalRotation, Time.deltaTime*5.0f);
			if (Vector3.Distance(spearHead.position,originalPosition) < .2f) {
				// Spear is close enough, lock it to correct transform
				spearHead.position = originalPosition;
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
			Destroy(target.gameObject);
			isReturning = true;
		}
	}


	void OnTriggerEnter(Collider coll) {
		currTarget = coll.transform;
	}

	void OnTriggerExit(Collider coll) {
		if (!isAttacking)
			currTarget = null;
	}
}