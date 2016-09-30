using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject fox;
	public GameObject rabbit;
	public GameObject carrot;

	public int numFoxes;
	public int numRabbits;
	public int numCarrots;

	Vector3 playerPos;

	void Start () {
		Spawn (fox, numFoxes);
		Spawn (rabbit, numRabbits);
		Spawn (carrot, numCarrots);
	}

	void Spawn (GameObject prefab, int number) {
		for (int i = 0; i < number; i++) {
			Instantiate (prefab, new Vector3 (Random.Range (-40, 40), 0.5f, Random.Range (-40, 40)), Quaternion.identity);
		}
	}
}