using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {
	public float maxSpeed = 10f;
	public float maxForce = 3f;
	public float arrivalRange = 5f;

	protected Rigidbody rb;

	protected void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	/** steering force = desired velocity - current velocity */
	Vector3 Steer (Vector3 desired) {
		return desired - rb.velocity;
	}

	/** Move as fast as possible toward a static point. */
	public Vector3 Seek (Vector3 target) {
		Vector3 desired = target - transform.position;
		desired = desired.normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Seek (GameObject obj) {
		return Seek (obj.transform.position);
	}

	/** Move as fast as possible away from a static point. */
	public Vector3 Flee (Vector3 target) {
		Vector3 desired = transform.position - target;
		desired = desired.normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Flee (GameObject obj) {
		return Flee (obj.transform.position);
	}

	/** Seek an object's future position. */
	public Vector3 Pursue (GameObject obj) {
		Vector3 future = obj.transform.position + obj.GetComponent<Rigidbody> ().velocity;
		return Seek (future);
	}

	/** Flee from an object's future position. */
	public Vector3 Evade (GameObject obj) {
		Vector3 future = obj.transform.position + obj.GetComponent<Rigidbody> ().velocity;
		return Flee (future);
	}

	/** Seek a target until it's close; then approach (seek) slowly. */
	public Vector3 Arrive (Vector3 target) {
		Vector3 desired = target - transform.position;

		float d = desired.magnitude;
		desired = desired.normalized * maxSpeed;
		if (d < arrivalRange)
			desired *= d / arrivalRange;

		return Steer (desired);
	}
	public Vector3 Arrive (GameObject obj) {
		return Arrive (obj.transform.position);
	}

//	public Vector3 Wander() {
//	}

	//.Offset pursuit
	//.Wander, Explore, Forage
	//.FollowPath
	//.ContainWithin
	//.AvoidObstacles
	//.AvoidCollisions
	//.Shadow

	/** Steer away from objects. The closer an object, the greater the separation force from that object. */
	public Vector3 Separate (GameObject[] objects) {
		Vector3 sum = Vector3.zero;
		if (objects.Length == 0)
			return sum;

		foreach (GameObject obj in objects) {
			Vector3 away = transform.position - obj.transform.position;
			sum += away.normalized / away.magnitude;
		}

		Vector3 desired = sum.normalized * maxSpeed / objects.Length;

		return Steer (desired * maxSpeed);
	}

	/** Steer in the average direction of other objects. */
	public Vector3 Align (GameObject[] objects) {
		Vector3 dir = Vector3.zero;
		if (objects.Length == 0)
			return dir;

		foreach (GameObject obj in objects)
			dir += obj.GetComponent<Rigidbody> ().velocity;

		return Steer (dir.normalized * maxSpeed);
	}

	/** Arrive at the center of mass of other objects. */
	public Vector3 Cohere (GameObject[] objects) {
		Vector3 center = Vector3.zero;
		if (objects.Length == 0)
			return center;

		foreach (GameObject obj in objects)
			center += obj.transform.position;

		return Arrive (center / objects.Length);
	}

	/** Arrive at a point behind the leader. */
	public Vector3 Follow (GameObject obj, float distance) {
		Vector3 desired = obj.transform.position - obj.GetComponent<Rigidbody> ().velocity.normalized * distance;
		return Arrive (desired);
	}
}