using UnityEngine;
using System.Collections;

public class BouncyBoundary : MonoBehaviour {
	public float force;

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Boid")) {
			Vector3 toOrigin = gameObject.transform.position - other.gameObject.transform.position;
			other.GetComponent<Rigidbody> ().AddForce (toOrigin.normalized * force, ForceMode.Impulse);
		}
	}
}