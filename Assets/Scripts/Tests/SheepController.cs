using UnityEngine;
using System.Collections;

public class SheepController : MonoBehaviour {
	Vehicle vehicle;
	GameObject predator;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
	}

	void FixedUpdate () {
		if (predator && (transform.position - predator.transform.position).magnitude < 10)
			vehicle.ApplyForce (vehicle.Evade (predator));
		else
			vehicle.ApplyForce (vehicle.Wander ());
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Wolf"))
			predator = other.gameObject;
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject == predator)
			predator = null;
	}
}