using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public GameObject fox;
	public GameObject rabbit;
	public GameObject carrot;

	public int numFoxes;
	public int numRabbits;
	public int numCarrots;
	public float secondsPerCarrot = 1;

	public Text text;

	Vector3 playerPos;

	int foxCount;
	int rabbitCount;
	int carrotCount;

	int rabbitsEaten;
	int carrotsEaten;

	int foxesBorn;
	int rabbitsBorn;

	void Start () {
		Spawn (fox, numFoxes);
		Spawn (rabbit, numRabbits);
		Spawn (carrot, numCarrots);

		StartCoroutine (SpawnCarrots ());

		if (false) {
			Time.timeScale = 5;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
	}

	void Update () {
//		text.text = GenerateText ();
	}

	void Spawn (GameObject prefab, int number = 1) {
		for (int i = 0; i < number; i++) {
			GameObject obj = (GameObject)Instantiate (prefab, new Vector3 (Random.Range (-40, 40), 0.5f, Random.Range (-40, 40)), Quaternion.identity);
			obj.GetComponent<Rigidbody> ().AddForce (Random.insideUnitCircle);
		}
	}

	IEnumerator SpawnCarrots () {
		while (true) {
			Spawn (carrot);
			yield return new WaitForSeconds (secondsPerCarrot);
		}
	}

	string GenerateText () {
		return "Fox count: " + numFoxes.ToString () + "\nRabbit count: " + numRabbits.ToString () + "\nCarrot count: " + numCarrots.ToString () + "\n\nRabbits eaten: " + rabbitsEaten.ToString () + "\nCarrots eaten: " + carrotsEaten.ToString () + "\n\nFoxes born: " + foxesBorn.ToString () + "\nRabbits born: " + rabbitsBorn.ToString ();
	}
}