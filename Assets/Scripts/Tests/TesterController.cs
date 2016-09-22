using UnityEngine;
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
		rb.AddForce (vehicle.Evade (mark));
	}
}