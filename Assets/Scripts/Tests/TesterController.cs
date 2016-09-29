using UnityEngine;
using System.Collections;

public class TesterController : MonoBehaviour {
	public GameObject mark;

	Vehicle vehicle;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
	}
	
	void FixedUpdate () {
		Vector3 force = Vector3.zero;

//		force += vehicle.Follow (mark, 1, 4);
//		force += EvadeOrWander ();
//		force += vehicle.Pursue (mark);
		force += vehicle.Wander ();
//		force += vehicle.Seek (mark);
		force += vehicle.AvoidObstacles () * 2;
		force += vehicle.AvoidCollisions () * 5;
//		force += vehicle.Arrive (Vector3.up);
//		force += vehicle.Evade (mark);

		vehicle.ApplyForce (force);
	}

	Vector3 EvadeOrWander() {
		if((transform.position - mark.transform.position).magnitude < 10)
			return vehicle.Evade (mark);
		else
			return vehicle.Wander ();
	}
}