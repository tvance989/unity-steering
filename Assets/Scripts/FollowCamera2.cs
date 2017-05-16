using UnityEngine;
using System.Collections;

public class FollowCamera2 : MonoBehaviour {
	public Transform target;
	
	void Update () {
		transform.position = new Vector3 (target.transform.position.x, 10, target.transform.position.z);
	}
}
