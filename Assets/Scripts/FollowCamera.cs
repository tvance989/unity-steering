using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public GameObject target;

	void Start () {
	}
	
	void Update () {
		transform.position = new Vector3 (target.transform.position.x, 10, target.transform.position.z - 10);
	}
}
