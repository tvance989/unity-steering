using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour {
	public int n;

	void Start () {
		SpawnRandom ();
	}

	void SpawnRegular () {
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				GameObject cube = SpawnCube (new Vector3 (-40 + 80 * i / n, 0, -40 + 80 * j / n));
			}
		}
	}

	void SpawnRandom () {
		for (int i = 0; i < n * n; i++) {
			GameObject cube = SpawnCube (new Vector3 (Random.Range (-40, 40), 0, Random.Range (-40, 40)));
		}
	}

	void SpawnPerlin () {
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				if (Mathf.PerlinNoise ((float)i, (float)j) < 0.5f) {
					GameObject cube = SpawnCube (new Vector3 (-40 + 80 * i / n, 0, -40 + 80 * j / n));
				}
			}
		}
	}

	GameObject SpawnCube (Vector3 pos) {
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.position = pos;
		cube.GetComponent<Renderer> ().material.color = Color.yellow;
		cube.transform.localScale = new Vector3 (0.5f, 3, 0.5f);
		return cube;
	}
}
