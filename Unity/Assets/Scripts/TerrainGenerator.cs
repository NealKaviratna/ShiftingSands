using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {	
	private int Samples = 100;
	private float Width = 1000;
	int PerlinLevels = 4;
	

	// Use this for initialization
	void Start () {

		float edge = Width / (Samples - 1);
		
		// Create the vertices and texture coords
		var vertices = new Vector3[Samples * Samples];  
		var uvs = new Vector2[Samples * Samples];
		
		// Fill in the vertices and uv data
		for (int i = 0, p = 0; i < Samples; i++) {  
			for (int j = 0; j < Samples; j++) {
				// Current vertex
				var center = new Vector3(i * edge, 0, j * edge);
				// Height of this vertex (from heightmap)
				float h = SampleHeight(center.x, center.z);
				center.y = h;
				vertices[p] = center;
				// UV coords in [0,1] space
				uvs[p++] = new Vector2(i/(Samples - 1f),
				                       j/(Samples - 1f));
			}
		}

		int quadCount = (Samples - 1) * (Samples - 1);
		int[] triangles = new int[3 * 2 * quadCount];

		Vector3[] trianglenormals = new Vector3[quadCount*2];
		Vector3[] addednormals = new Vector3[Samples * Samples];
		for (int i = 0; i < addednormals.Length; i++) {
			addednormals[i] = Vector3.zero;
		}
		int[] totalAdded = new int[Samples * Samples];

		// Create triangles TODO: add Normals for shaders
		for (int t = 0; t < quadCount; t++) {
			int i = t / (Samples - 1);
			int j = t % (Samples - 1);
			
			triangles[6 * t + 0] = CalculateIndex(i + 1, j, Samples);
			triangles[6 * t + 1] = CalculateIndex(i, j, Samples);
			triangles[6 * t + 2] = CalculateIndex(i, j + 1, Samples);
			triangles[6 * t + 3] = CalculateIndex(i + 1, j, Samples);
			triangles[6 * t + 4] = CalculateIndex(i, j + 1, Samples);
			triangles[6 * t + 5] = CalculateIndex(i + 1, j + 1, Samples);

			trianglenormals[2*t] = Vector3.Cross((vertices[triangles[6 * t + 0]] - vertices[triangles[6 * t + 1]]),
			                                     (vertices[triangles[6 * t + 0]] - vertices[triangles[6 * t + 2]]));
			trianglenormals[2*t+1] = Vector3.Cross((vertices[triangles[6 * t + 3]] - vertices[triangles[6 * t + 4]]),
			                                     (vertices[triangles[6 * t + 3]] - vertices[triangles[6 * t + 5]]));

			addednormals[CalculateIndex(i + 1, j, Samples)] += trianglenormals[2*t];
			totalAdded[CalculateIndex(i + 1, j, Samples)]++;
			addednormals[CalculateIndex(i, j, Samples)] += trianglenormals[2*t];
			totalAdded[CalculateIndex(i , j, Samples)]++;
			addednormals[CalculateIndex(i , j + 1, Samples)] += trianglenormals[2*t];
			totalAdded[CalculateIndex(i, j + 1, Samples)]++;

			addednormals[CalculateIndex(i + 1, j, Samples)] += trianglenormals[2*t+1];
			totalAdded[CalculateIndex(i + 1, j, Samples)]++;
			addednormals[CalculateIndex(i, j + 1, Samples)] += trianglenormals[2*t+1];
			totalAdded[CalculateIndex(i, j + 1, Samples)]++;
			addednormals[CalculateIndex(i + 1, j + 1, Samples)] += trianglenormals[2*t+1];
			totalAdded[CalculateIndex(i + 1, j + 1, Samples)]++;
		}

		for (int i = 0; i < addednormals.Length; i++) {
			addednormals[i] /= totalAdded[i];
			Vector3.Normalize(addednormals[i]);
		}

		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
		GetComponent<MeshCollider>().enabled = false;
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.normals = addednormals;
	}
	
	// Update is called once per frame
	void Update () {
		// For some reason Unity won't calculate collisions with this mesh if you don't reaffirm it is enabled at least
		// once during runtime. TODO:Either change this so it only has to happen once, or figure out a better workaround
		GetComponent<MeshCollider>().enabled = true;
	}

	public float SampleHeight(float x, float y) {
		// Poll height from Perlin noise TODO: refine heightmap
		return SamplePerlin(x, y) * 1.2f;
	}

	private float SamplePerlin(float x, float y) {  
		float width = Width / 10f;
		float result = 0;
		
		for (int i = 0; i < PerlinLevels; i++) {
			result += (Mathf.PerlinNoise(x / width, y / width))
				* width / 2;
			width /= 10;
		}
		
		return result;
	}

	// Helper function for triangle generation
	private int CalculateIndex(int i, int j, int Samples) {
		return i * Samples + j;
	}
}
