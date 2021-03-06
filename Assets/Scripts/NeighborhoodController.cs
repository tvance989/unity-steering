﻿using UnityEngine;
using System.Collections;

public class NeighborhoodController : MonoBehaviour {
	public GameObject boid;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			boid.SendMessage ("AddNeighbor", other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			boid.SendMessage ("RemoveNeighbor", other.gameObject);
	}
}