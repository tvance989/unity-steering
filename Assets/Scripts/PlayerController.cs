using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float turnForce;
	public float jumpForce;

	Vehicle vehicle;

	void Start() {
		vehicle = GetComponent<Vehicle> ();
	}

	void FixedUpdate() {
		Vector3 force = Vector3.zero;

		force += vehicle.Steer (Vector3.forward * Input.GetAxis ("Vertical") * vehicle.maxSpeed);
		force += vehicle.Steer (Vector3.right * Input.GetAxis ("Horizontal") * vehicle.maxSpeed);

		vehicle.ApplyForce (force);
	}
}
