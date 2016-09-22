using UnityEngine;
using System.Collections;

public class ElbowRoomController : MonoBehaviour {
	public GameObject boid;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			boid.SendMessage ("AddCrowder", other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Boid"))
			boid.SendMessage ("RemoveCrowder", other.gameObject);
	}
}