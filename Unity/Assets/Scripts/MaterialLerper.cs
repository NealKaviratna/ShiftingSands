using UnityEngine;
using System.Collections;

public class MaterialLerper : MonoBehaviour {

	public Material mat1;
	public Material mat2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.renderer.material.Lerp(mat1,mat2, Mathf.Sin(Time.time));
	}
}
