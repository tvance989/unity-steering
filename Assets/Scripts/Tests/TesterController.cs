using UnityEngine;
using System.Collections;

public class TesterController : Vehicle {
	public GameObject mark;

	int num_collisions = 0;
	
	void FixedUpdate () {
		Vector3 force = Vector3.zero;

//		force += Follow (mark, 1, 4);
//		force += EvadeOrWander ();
		force += Arrive (mark);
		force += Wander ();
//		force += Seek (mark);
		force += AvoidObstacles () * 3;
		force += AvoidCollisions () * 2;
//		force += Arrive (Vector3.up);
//		force += Evade (mark);

		ApplyForce (force);
	}

	Vector3 EvadeOrWander() {
		if((transform.position - mark.transform.position).magnitude < 10)
			return Evade (mark);
		else
			return Wander ();
	}

	void OnDrawGizmos () {
//		Gizmos.DrawRay(transform.position, steering);
//		Gizmos.DrawRay (ray1);
//		Gizmos.DrawRay (ray2);
	}

	void OnCollisionEnter (Collision other) {
		num_collisions++;
		Debug.Log (num_collisions.ToString () + " collisions for Tester");
	}
}