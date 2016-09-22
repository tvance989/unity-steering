using UnityEngine;
using System.Collections;

public class TeleportyBoundary : MonoBehaviour {
	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Boid")) {
			other.gameObject.transform.position *= -1;
		}
	}
}