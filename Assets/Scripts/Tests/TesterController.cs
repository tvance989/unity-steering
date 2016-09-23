﻿using UnityEngine;
using System.Collections;

public class TesterController : MonoBehaviour {
	public GameObject mark;

	private Rigidbody rb;
	private Vehicle vehicle;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		vehicle = GetComponent<Vehicle> ();
	}
	
	void FixedUpdate () {
		if((transform.position - mark.transform.position).magnitude < 10)
			rb.AddForce (vehicle.Evade (mark) * 7);
		else
			rb.AddForce (vehicle.Wander (3, 2, 0.1f) * 3);
	}
}