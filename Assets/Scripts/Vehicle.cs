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

	Vector3 Steer (Vector3 desired) {
		return desired - rb.velocity;
	}
		
	public Vector3 Seek (Vector3 target) {
		Vector3 desired = target - transform.position;
		desired = desired.normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Seek (GameObject obj) {
		return Seek (obj.transform.position);
	}
	public Vector3 Flee (Vector3 target) {
		Vector3 desired = transform.position - target;
		desired = desired.normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Flee (GameObject obj) {
		return Flee (obj.transform.position);
	}

	//move most of these to self-contained classes that implement ISteeringBehavior

	public Vector3 Pursue (GameObject obj) {
		Vector3 target = obj.transform.position + obj.GetComponent<Rigidbody> ().velocity;
		return Seek (target);
	}
	public Vector3 Evade (GameObject obj) {
		Vector3 target = obj.transform.position + obj.GetComponent<Rigidbody> ().velocity;
		return Flee (target);
	}

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

	//.Wander, Explore, Forage
	//.FollowPath
	//.ContainWithin
	//.AvoidObstacles
	//.AvoidCollisions
	//.Shadow

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

	public Vector3 Align (GameObject[] objects) {
		Vector3 dir = Vector3.zero;
		if (objects.Length == 0)
			return dir;

		foreach (GameObject obj in objects)
			dir += obj.GetComponent<Rigidbody> ().velocity;

		return Steer (dir.normalized * maxSpeed);
	}

	public Vector3 Cohere (GameObject[] objects) {
		Vector3 center = Vector3.zero;
		if (objects.Length == 0)
			return center;

		foreach (GameObject obj in objects)
			center += obj.transform.position;

		return Arrive (center / objects.Length);
	}

	public Vector3 Follow (GameObject obj, float distance) {
		Vector3 desired = obj.transform.position - obj.GetComponent<Rigidbody> ().velocity.normalized * distance;
		return Arrive (desired);
	}
}