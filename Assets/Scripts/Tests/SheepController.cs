using UnityEngine;
using System.Collections;

public class SheepController : MonoBehaviour {
	Vehicle vehicle;
	GameObject predator;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
	}

	void Update () {
		GameObject closest = FindClosestWolf ();
		if ((closest.transform.position - transform.position).magnitude < 10)
			predator = closest;
		else
			predator = null;
	}

	GameObject FindClosestWolf () {
		GameObject closest = null;
		float distance = Mathf.Infinity;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Wolf")) {
			float curDistance = (go.transform.position - transform.position).sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}

	void FixedUpdate () {
		Vector3 force = Vector3.zero;

		if (predator && (transform.position - predator.transform.position).magnitude < 10) {
			force += vehicle.Evade (predator) * 4;
		} else {
			force += vehicle.Wander ();
		}
		force += vehicle.AvoidObstacles ();

		vehicle.ApplyForce (force);
	}

	/*
	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Wolf"))
			predator = other.gameObject;
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject == predator)
			predator = null;
	}
	*/
}