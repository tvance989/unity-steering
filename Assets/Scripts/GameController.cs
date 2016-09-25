using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject boid;
	public int numBoids;

	void Start () {
		for (int i = 0; i < numBoids; i++) {
			GameObject obj = (GameObject)Instantiate (boid, Random.insideUnitSphere * 25, Quaternion.identity);

			Vector3 pos = obj.transform.position;
			obj.transform.position = new Vector3 (pos.x, Mathf.Abs (pos.y), pos.z);

			obj.GetComponent<Rigidbody> ().AddForce (Random.insideUnitSphere * 10, ForceMode.Impulse);
		}
	}
}