using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	Vehicle vehicle;

	void Start() {
		vehicle = GetComponent<Vehicle> ();
	}

	void FixedUpdate() {
		Vector3 force = Vector3.zero;

		force += vehicle.Steer (Vector3.forward * Input.GetAxis ("Vertical") * vehicle.maxSpeed);
		force += vehicle.Steer (Vector3.right * Input.GetAxis ("Horizontal") * vehicle.maxSpeed);
		force += PlayItSafe () * 0.3f;

		vehicle.ApplyForce (force);
	}

	Vector3 PlayItSafe() {
		return vehicle.AvoidObstacles () + vehicle.AvoidCollisions ();
	}
}
