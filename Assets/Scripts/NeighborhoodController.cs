using UnityEngine;
using System.Collections;

public class NeighborhoodController : MonoBehaviour {
	public BoidController boid;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			boid.AddNeighbor (other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			boid.RemoveNeighbor (other.gameObject);
	}
}