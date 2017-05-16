using UnityEngine;
using System.Collections;

public class PlayerController : Vehicle {
	void FixedUpdate() {
		Vector3 force = Vector3.zero;

		force += Steer (Vector3.forward * Input.GetAxis ("Vertical") * maxSpeed);
		force += Steer (Vector3.right * Input.GetAxis ("Horizontal") * maxSpeed);
		force += PlayItSafe () * 0.5f;

		ApplyForce (force);
	}

	Vector3 PlayItSafe() {
		return AvoidObstacles () + AvoidCollisions ();
	}
}
