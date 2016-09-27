using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {
	public float maxSpeed = 20;
	public float maxForce = 10;

	Rigidbody rb;
	Vector3 wanderDirection;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		wanderDirection = Random.insideUnitCircle.normalized * maxSpeed;
	}


	public void ApplyForce (Vector3 force) {
		rb.AddForce (Vector3.ClampMagnitude (force, maxForce));
		if (rb.velocity.magnitude > maxSpeed)
			rb.velocity *= 0.99f;
	}


	/** steering force = desired velocity - current velocity */
	public Vector3 Steer (Vector3 desired) {
		return desired - rb.velocity;
	}

	/** Move as fast as possible toward a static point. */
	public Vector3 Seek (Vector3 target) {
		Vector3 desired = (target - transform.position).normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Seek (GameObject obj) { return Seek (obj.transform.position); }

	/** Move as fast as possible away from a static point. */
	public Vector3 Flee (Vector3 target) {
		Vector3 desired = (transform.position - target).normalized * maxSpeed;
		return Steer (desired);
	}
	public Vector3 Flee (GameObject obj) { return Flee (obj.transform.position); }

	/** Seek a target until it's close; then approach (seek) slowly. */
	public Vector3 Arrive (Vector3 target) {
		float arrivalRange = 2;//.arbitrary
		Vector3 desired = target - transform.position;

		float d = desired.magnitude;
		if (d < arrivalRange)
			desired = desired.normalized * maxSpeed * d / arrivalRange;

		return Steer (desired);
	}
	public Vector3 Arrive (GameObject obj) { return Arrive (obj.transform.position); }

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

	public Vector3 Wander() {
		float jitter = 15;//.arbitrary
		float angle = jitter;
		if (Random.value < 0.5f)
			angle = -jitter;

		wanderDirection = Quaternion.AngleAxis (angle, Vector3.up) * wanderDirection;

		float radius = maxSpeed * 0.4f;//.arbitrary
		Vector3 center = rb.velocity.normalized * (maxSpeed - radius);
		Vector3 displacement = wanderDirection * radius;

		Vector3 desired = transform.position + center + displacement;
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
	public Vector3 Cohere (GameObject[] objects) {
		Vector3 center = Vector3.zero;
		if (objects.Length == 0)
			return center;

		foreach (GameObject obj in objects)
			center += obj.transform.position;

		return Arrive (center / objects.Length);
	}

	/** Arrive at a point behind the leader. */
	//.need param for things to separate from? or just handle that in the controller?
	public Vector3 Follow (GameObject leader, float followDistance, float bufferLength) {
		Vector3 lv = leader.GetComponent<Rigidbody> ().velocity;

		float d = (leader.transform.position - gameObject.transform.position).magnitude;

		float bufferRadius = followDistance / 2;

		if (d < bufferRadius) {
			// If way too close, just get away.
			return Flee (leader);
		} else if (d < bufferLength + bufferRadius) {
			// If kinda close, see if they're in front of the leader and evade if necessary.
			RaycastHit hit;
			//.gotta use spherecastall. otherwise it just returns the "first hit"
			if (Physics.SphereCast (leader.transform.position, bufferRadius, lv, out hit, bufferLength))
				if (hit.collider.gameObject == this.gameObject)
					return Evade (leader);
		} else {
			// Arrive at point behind the leader.
			Vector3 desired = leader.transform.position - lv.normalized * followDistance;
			return Arrive (desired);
		}

		return Vector3.zero;
	}

	public Vector3 AvoidObstacles() {
		float distance = maxSpeed / 2;
		float radius = 1;//.arbitrary

		RaycastHit hit;
		if (Physics.SphereCast (transform.position, radius, rb.velocity, out hit, distance))
			return Steer ((rb.velocity.normalized * distance - hit.collider.gameObject.transform.position).normalized * maxSpeed);

		return Vector3.zero;
	}
}