using UnityEngine;
using System.Collections;

public class SceneTransitionController : MonoBehaviour {

	private Texture loadingTexture;
	public float alphaFadeValue = 0;
	public bool faded = false;
	
	public bool fadeIn;
	public bool fadeOut;
	public int levelToLoad;

	public GameObject dustClouds;
	public float fadeTimer = 3.0f;
	public GameObject[] activeOnFadeIn;
	public TowerFallScript Cameraman;
	
	// Use this for initialization
	void Start () {
		loadingTexture = Resources.Load("loading_screen") as Texture;
		if (Application.loadedLevel == 5) loadingTexture = Resources.Load("White_Texture") as Texture;
	}
	
	// Update is called once per frame
	void Update () {
		if (faded) Application.LoadLevel(levelToLoad);

		if (fadeIn || fadeOut) {
			foreach (ParticleEmitter p in dustClouds.GetComponentsInChildren<ParticleEmitter>()) {
				p.emit = true;
			}
		}
		if ((fadeIn || fadeOut) && fadeTimer > 0.0) {
			fadeTimer -= Time.deltaTime;
		}
	}
	
	void OnGUI() {
		if (fadeOut && fadeTimer <= 0.0) {
			alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 5);
			GUI.color = new Color(1, 1, 1, Mathf.Min(alphaFadeValue, 1.0f));
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), loadingTexture );
			if (alphaFadeValue >= 1) {
				faded = true;
			}
		}
		else if (fadeIn && fadeTimer <= 0.0) {		
			alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 5);
			GUI.color = new Color(1, 1, 1, Mathf.Min(alphaFadeValue, 1.0f));
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), loadingTexture );
			if (alphaFadeValue <= 0)  {
				foreach (ParticleEmitter p in dustClouds.GetComponentsInChildren<ParticleEmitter>()) {
					p.emit = false;
				}
				fadeTimer = 3.0f;
				fadeIn = false;

				foreach(GameObject o in activeOnFadeIn) {
					o.SetActive(true);
				}
				if (Cameraman) Cameraman.isFalling = true;
			}
		}
		else if (fadeIn && fadeTimer >= 0.0) {
			GUI.color = new Color(1, 1, 1, Mathf.Min(alphaFadeValue, 1.0f));
			GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), loadingTexture );
		}
	}
}
