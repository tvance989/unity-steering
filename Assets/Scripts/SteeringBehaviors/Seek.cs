using UnityEngine;
using System.Collections;

public class Seek : SteeringBehavior {
	public Vector3 target;

	public override Vector3 CalcForce() {
		return vehicle.Seek (target);
	}
}
