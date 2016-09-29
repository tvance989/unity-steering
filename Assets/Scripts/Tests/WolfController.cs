using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WolfController : MonoBehaviour {
	Vehicle vehicle;
	GameObject prey;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
	}

	void FixedUpdate () {
		Vector3 force = Vector3.zero;

		if (prey) {
			force += vehicle.Pursue (prey);
		} else {
			force += vehicle.Wander ();
			force += vehicle.AvoidObstacles ();
		}

		vehicle.ApplyForce (force);
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject == prey) {
			Destroy (other.gameObject);
			prey = null;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (!prey && other.CompareTag ("Sheep"))
			prey = other.gameObject;
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject == prey)
			prey = null;
	}
}