using UnityEngine;
using System.Collections;

public class BoundsController : MonoBehaviour {
	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Boid")) {
			Vector3 toOrigin = gameObject.transform.position - other.gameObject.transform.position;
			other.GetComponent<Rigidbody> ().AddForce (toOrigin.normalized * 10, ForceMode.Impulse);
//			other.gameObject.transform.position = gameObject.transform.position;
		}
	}
}
