using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject wolf;
	public GameObject sheep;

	public int numWolves;
	public int numSheep;

	void Start () {
		SpawnWolves (numWolves);
		SpawnSheep (numSheep);
	}

	void SpawnWolves (int n) {
		for (int i = 0; i < n; i++) {
			GameObject obj = (GameObject)Instantiate (wolf, new Vector3 (Random.Range (-40, 40), 1, Random.Range (-40, 40)), Quaternion.identity);
		}
	}

	void SpawnSheep (int n) {
		for (int i = 0; i < n; i++) {
			GameObject obj = (GameObject)Instantiate (sheep, new Vector3 (Random.Range (-40, 40), 0.5f, Random.Range (-40, 40)), Quaternion.identity);
		}
	}
}