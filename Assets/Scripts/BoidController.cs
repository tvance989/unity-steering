using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidController : MonoBehaviour {
	public float separationWeight;
	public float alignmentWeight;
	public float cohesionWeight;

	public float maxSpeed;
	public float maxForce;

	private Rigidbody rb;
	private List<GameObject> neighbors;
	private List<GameObject> crowders;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		neighbors = new List<GameObject> ();
		crowders = new List<GameObject> ();
	}
	
	void Update () {
		Flock ();
	}

	void Flock() {
		rb.AddForce (Separate () * separationWeight);
		rb.AddForce (Align () * alignmentWeight);
		rb.AddForce (Cohere () * cohesionWeight);
	}

	Vector3 Separate() {
		Vector3 v = new Vector3 (0, 0, 0);
		if (crowders.Count == 0)
			return v;

		foreach (GameObject boid in crowders) {
			Vector3 away = gameObject.transform.position - boid.transform.position;
			v += away.normalized / away.magnitude;
		}

		return Steer (v.normalized * maxSpeed);
	}

	Vector3 Align() {
		Vector3 v = new Vector3 (0, 0, 0);
		if (neighbors.Count == 0)
			return v;

		foreach (GameObject boid in neighbors)
			v += boid.GetComponent<Rigidbody> ().velocity;

		return Steer (v.normalized * maxSpeed);
	}

	Vector3 Cohere() {
		Vector3 centerOfMass = new Vector3 (0, 0, 0);
		if (neighbors.Count == 0)
			return centerOfMass;

		foreach (GameObject boid in neighbors)
			centerOfMass += boid.transform.position;

		return Arrive (centerOfMass / neighbors.Count);
	}

	Vector3 Arrive(Vector3 target) {
		Vector3 desired = target - gameObject.transform.position;
		float d = desired.magnitude;

		float arbitrary = 5f;
		if (d < arbitrary)
			desired = desired.normalized * d * maxSpeed / arbitrary;
		else
			desired = desired.normalized * maxSpeed;

		return Steer (desired);
	}

	Vector3 Steer(Vector3 desired) {
		Vector3 steering = desired - rb.velocity;
		if (steering.magnitude > maxForce)
			return steering.normalized * maxForce;
		return steering;
	}

	public void AddNeighbor(GameObject boid) {
		neighbors.Add (boid);
	}
	public void RemoveNeighbor(GameObject boid) {
		neighbors.Remove (boid);
	}

	public void AddCrowder(GameObject boid) {
		crowders.Add (boid);
	}
	public void RemoveCrowder(GameObject boid) {
		crowders.Remove (boid);
	}
}