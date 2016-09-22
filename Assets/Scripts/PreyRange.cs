using UnityEngine;
using System.Collections;

public class PreyRange : MonoBehaviour {
	public BoidController predator;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			predator.AddNeighbor (other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			predator.RemoveNeighbor (other.gameObject);
	}
}