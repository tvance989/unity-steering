using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {
	public float maxSpeed = 10f;
	public float maxForce = 3f;

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
	public Vector3 Seek (GameObject obj) { return Seek (obj.transform.position); }

	/** Move as fast as possible away from a static point. */
	public Vector3 Flee (Vector3 target) {
		Vector3 desired = transform.position - target;
		desired = desired.normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Flee (GameObject obj) { return Flee (obj.transform.position); }

	/** Seek a target until it's close; then approach (seek) slowly. */
	public Vector3 Arrive (Vector3 target, float arrivalRange = 0) {
		Vector3 desired = target - transform.position;

		float d = desired.magnitude;
		if (d < arrivalRange)
			desired = desired.normalized * maxSpeed * d / arrivalRange;

		return Steer (desired);
	}
	public Vector3 Arrive (GameObject obj, float arrivalRange = 0) { return Arrive (obj.transform.position, arrivalRange); }

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

	protected Vector3 wanderDisplacement = Vector3.zero;
	public Vector3 Wander(float offset, float radius, float wanderProbability) {
		if (Random.value <= wanderProbability) {
			Vector2 point = Random.insideUnitCircle.normalized;
			wanderDisplacement = new Vector3 (point.x, 0, point.y) * radius;
		}

		Vector3 desired = transform.position + rb.velocity.normalized * offset + wanderDisplacement;
		return Seek (desired);
	}
	public Vector3 Wander3D(float offset, float radius, float wanderProbability) {
		if (Random.value <= wanderProbability)
			wanderDisplacement = Random.onUnitSphere * radius;

		Vector3 desired = transform.position + rb.velocity.normalized * offset + wanderDisplacement;
		return Seek (desired);
	}

	//.Offset pursuit?
	//.Explore
	//.Forage
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
	public Vector3 Cohere (GameObject[] objects, float arrivalRange = 0) {
		Vector3 center = Vector3.zero;
		if (objects.Length == 0)
			return center;

		foreach (GameObject obj in objects)
			center += obj.transform.position;

		return Arrive (center / objects.Length, arrivalRange);
	}

	/** Arrive at a point behind the leader. */
	//.need param for things to separate from? or just handle that in the controller?
	public Vector3 Follow (GameObject leader, float followDistance, float bufferLength, float arrivalRange) {
		Vector3 force = Vector3.zero;

		Vector3 lv = leader.GetComponent<Rigidbody> ().velocity;

		float d = (leader.transform.position - gameObject.transform.position).magnitude;

		float bufferRadius = followDistance / 2;

		if (d < bufferRadius) {
			// If way too close, just get away.
			return Flee (leader);
		} else if (d < bufferLength + bufferRadius) {
			// If kinda close, see if they're in front of the leader and evade if necessary.
			RaycastHit hit;
			if (Physics.SphereCast (leader.transform.position, bufferRadius, lv, out hit, bufferLength))
				if (hit.collider.gameObject == this.gameObject)
					return Evade (leader);
		} else {
			// Arrive at point behind the leader.
			Vector3 desired = leader.transform.position - lv.normalized * followDistance;
			return Arrive (desired, arrivalRange);
		}

		return force;
	}
}