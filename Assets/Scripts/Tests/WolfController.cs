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
		if (prey) {
			vehicle.ApplyForce (vehicle.Pursue (prey));
			if ((prey.transform.position - transform.position).magnitude < 2) {
				Destroy (prey);
				prey = null;
			}
		} else {
			vehicle.ApplyForce (vehicle.Wander ());
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