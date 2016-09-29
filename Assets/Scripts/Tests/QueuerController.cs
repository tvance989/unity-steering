using UnityEngine;
using System.Collections;

public class QueuerController : MonoBehaviour {
	public GameObject target;

	Vehicle vehicle;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
	}
	
	void FixedUpdate () {
		Vector3 force = Vector3.zero;

		force += vehicle.Arrive (target);
		force += vehicle.AvoidObstacles () * 2;
		force += vehicle.Queue () * 4;

		vehicle.ApplyForce (force);
	}
}
