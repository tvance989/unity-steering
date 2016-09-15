using UnityEngine;
using System.Collections;

public class ElbowRoomController : MonoBehaviour {
	public BoidController boid;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			boid.AddCrowder (other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			boid.RemoveCrowder (other.gameObject);
	}
}