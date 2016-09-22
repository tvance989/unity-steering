using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidController : Vehicle {
	public float separationWeight = 4f;
	public float alignmentWeight = 5f;
	public float cohesionWeight = 3f;

	protected List<GameObject> neighbors;
	protected List<GameObject> crowders;

	protected Renderer renderer;

	protected void Start () {
		base.Start ();

		neighbors = new List<GameObject> ();
		crowders = new List<GameObject> ();

		renderer = GetComponent<Renderer> ();
	}

	protected void Update () {
//		transform.rotation = Quaternion.LookRotation (rb.velocity);

		if (crowders.Count > 0)
			renderer.material.color = Color.red;
		else
			if (neighbors.Count > 1)
				renderer.material.color = Color.green;
			else if (neighbors.Count == 1)
				renderer.material.color = Color.yellow;
			else
				renderer.material.color = Color.black;
	}

	protected void FixedUpdate() {
		Vector3 force = Vector3.zero;

		force += Separate (crowders.ToArray ()) * separationWeight;
		force += Align (neighbors.ToArray ()) * alignmentWeight;
		force += Cohere (neighbors.ToArray ()) * cohesionWeight;

		rb.AddForce (Vector3.ClampMagnitude (force, maxForce));
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