using UnityEngine;
using System.Collections;

public class WolfController : Vehicle {
	public GameObject[] prey;

	void FixedUpdate () {
		GameObject closest = prey [0];
		float min = Mathf.Infinity;
		for (int i = 1; i < prey.Length; i++) {
			if (!prey [i])
				continue;
			Vector3 diff = transform.position - prey [i].transform.position;
			float d = diff.magnitude;
			if (d < min) {
				closest = prey [i];
				min = d;
			}
		}
		if (min < 1)
			Destroy (closest);
		rb.AddForce (Pursue (closest));
	}
}