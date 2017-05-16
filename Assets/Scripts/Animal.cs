using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Vehicle))]

public class Animal : MonoBehaviour {
	public GameObject baby;
	public Color maleColor;
	public Color femaleColor;

	public string predatorTag;
	public string foodTag;

	public float sensoryRange;
	public float fastingTime;
	public float starvationTime;
	public float abstainingTime;

	Vehicle vehicle;
	Rigidbody rb;

	GameObject predator;
	GameObject food;
	GameObject mate;

	float lastMeal;
	bool isHungry;
	float lastSex;
	bool isHorny;

	enum Sex {Male, Female};
	Sex sex;

	void Start () {
		vehicle = GetComponent<Vehicle> ();
		rb = GetComponent<Rigidbody> ();

		isHungry = isHorny = false;

		lastMeal = Time.time + Random.Range (0, fastingTime);
		lastSex = Time.time + Random.Range (0, abstainingTime);

		sex = Random.value < 0.5f ? Sex.Male : Sex.Female;

		Color color = sex == Sex.Male ? maleColor : femaleColor;
		GetComponent<Renderer> ().material.color = color;
		GetComponent<TrailRenderer> ().material.color = color;

		Debug.Log (GetInstanceID ().ToString () + " is born");
	}

	void Update () {
		if (Time.time - lastMeal > starvationTime)
			Destroy (this.gameObject);

		if (Time.time - lastMeal > fastingTime)
			isHungry = true;
		if (Time.time - lastSex > abstainingTime)
			isHorny = true;

		float minPredator = Mathf.Infinity;
		float minFood = Mathf.Infinity;
		float minMate = Mathf.Infinity;
		predator = food = mate = null;

		foreach (Collider collider in Physics.OverlapSphere(transform.position, sensoryRange)) {
			GameObject obj = collider.gameObject;
			if (obj == this.gameObject)
				continue;
			
			if (predatorTag != "" && obj.CompareTag (predatorTag)) {
				float d = (obj.transform.position - transform.position).magnitude;
				if (d < minPredator) {
					predator = obj;
					minPredator = d;
				}
			} else if (isHungry && foodTag != "" && isHungry && obj.CompareTag (foodTag)) {
				float d = (obj.transform.position - transform.position).magnitude;
				if (d < minFood) {
					food = obj;
					minFood = d;
				}
			} else if (!isHungry && isHorny && obj.CompareTag (gameObject.tag) && obj.GetComponent<Animal> ().GetSex () != sex) {
				float d = (obj.transform.position - transform.position).magnitude;
				if (d < minMate) {
					mate = obj;
					minMate = d;
				}
			}
		}

		if (rb.velocity.sqrMagnitude > 1)
			transform.rotation = Quaternion.LookRotation (rb.velocity, Vector3.up);//.
	}

	void FixedUpdate () {
		Vector3 force = Vector3.zero;

		if (predator) {
			force += vehicle.Evade (predator) * 4;
			force += vehicle.AvoidObstacles ();
		} else if (food) {
			force += vehicle.Pursue (food);
		} else if (!isHungry && mate) {
			force += vehicle.Pursue (mate) * 2;
		} else {
			force += vehicle.Wander ();
			force += vehicle.AvoidObstacles () * 2;
		}

		vehicle.ApplyForce (force);
	}

	void OnDrawGizmos () {
		if (isHungry) {
			Gizmos.DrawCube (transform.position + Vector3.up * 2, Vector3.one * 0.5f);
		}
		if (isHorny) {
			Gizmos.DrawSphere (transform.position + Vector3.up * 3, 0.5f);
		}
	}

	void OnDrawGizmosSelected () {
		Gizmos.DrawRay (transform.position, vehicle.GetSteering ());
//		Gizmos.DrawRay (transform.position, rb.velocity);
		Gizmos.DrawWireSphere (transform.position, sensoryRange);

		if (predator) {
			Gizmos.DrawWireSphere (predator.transform.position, 2);
		} else if (food) {
			Gizmos.DrawWireSphere (food.transform.position, 2);
		} else if (mate) {
			Gizmos.DrawWireSphere (mate.transform.position, 2);
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject == food) {
			Eat (collision.gameObject);
		} else if (collision.gameObject == mate) {
			Love (collision.gameObject);
		}
	}

	void Eat (GameObject obj) {
		Debug.Log (GetInstanceID ().ToString () + " eats " + obj.GetInstanceID ());
		Destroy (obj);
		lastMeal = Time.time;
		isHungry = false;
	}

	void Love (GameObject obj) {
		Debug.Log (GetInstanceID ().ToString () + " does it with " + obj.GetInstanceID ());
		lastSex = Time.time;
		isHorny = false;

		if (GetSex () == Sex.Female) {
			GameObject baby = GiveBirth ();
			Debug.Log (GetInstanceID ().ToString () + " gives birth to " + baby.GetInstanceID ());
		}
	}

	Sex GetSex () {
		return sex;
	}

	GameObject GiveBirth () {
		return (GameObject)Instantiate (baby, transform.position, Quaternion.identity);
	}
}
