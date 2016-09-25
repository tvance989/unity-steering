using UnityEngine;
using System.Collections;

public class TesterController : MonoBehaviour {
	public GameObject mark;

	private Vehicle vehicle;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
	}
	
	void FixedUpdate () {
		Vector3 force = Vector3.zero;

//		force += vehicle.Follow (mark, 1, 4, 0.5f);
		force += EvadeOrWander ();

		vehicle.ApplyForce (force);
	}

	Vector3 EvadeOrWander() {
		if((transform.position - mark.transform.position).magnitude < 10)
			return vehicle.Evade (mark);
		else
			return vehicle.Wander (3, 2, 0.1f);
	}
}