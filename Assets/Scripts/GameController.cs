using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public BoidController boid;
	public int numBoids;

	void Start () {
		for (int i = 0; i < numBoids; i++) {
			BoidController obj = (BoidController)Instantiate (boid, Random.insideUnitSphere * 25, Quaternion.identity);
			Vector3 pos = obj.gameObject.transform.position;
			obj.gameObject.transform.position = new Vector3 (pos.x, Mathf.Abs (pos.y), pos.z);
			obj.gameObject.GetComponent<Rigidbody> ().AddForce (Random.insideUnitSphere * obj.maxSpeed, ForceMode.Impulse);
		}
	}
}