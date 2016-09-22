using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float turnForce;
	public float jumpForce;

	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space) && Physics.Raycast (transform.position, Vector3.down, 0.5f))
			Jump ();
	}

	void FixedUpdate() {
		float x = Input.GetAxis ("Horizontal") * turnForce;
		float z = Input.GetAxis ("Vertical") * turnForce;

		Vector3 movement = new Vector3 (x, 0, z);

		rb.AddForce (movement);
	}

	void Jump() {
		rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.Impulse);
	}
}
