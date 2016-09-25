using UnityEngine;
using System.Collections;

public abstract class SteeringBehavior : MonoBehaviour {
	protected Vehicle vehicle;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
	}
	
	public abstract Vector3 CalcForce ();
}
