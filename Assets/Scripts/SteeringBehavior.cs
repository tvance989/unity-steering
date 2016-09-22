using UnityEngine;
using System.Collections;

public abstract class SteeringBehavior : MonoBehaviour {
	public abstract Vector3 CalcForce ();
}
