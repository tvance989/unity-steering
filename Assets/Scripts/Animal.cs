using UnityEngine;
using System.Collections;

[System.Serializable]
public class DNA {
	public float maxSpeed, maxForce; // Vehicle properties
	public float sensoryRange; // How far can the animal see? (food, enemies, etc)
	public float fastingTime, starvationTime, abstainingTime; // Biological functions
}

[RequireComponent (typeof (Vehicle))]
public class Animal : MonoBehaviour {
	public GameObject baby;
	public Color maleColor;
	public Color femaleColor;

	public string predatorTag;
	public string foodTag;

	public DNA dna;

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

		lastMeal = Time.time + Random.Range (0, dna.fastingTime);
		lastSex = Time.time + Random.Range (0, dna.abstainingTime);

		sex = Random.value < 0.5f ? Sex.Male : Sex.Female;

		Color color = sex == Sex.Male ? maleColor : femaleColor;
		GetComponent<Renderer> ().material.color = color;
		GetComponent<TrailRenderer> ().material.color = color;

		Debug.Log (GetInstanceID ().ToString () + " is born");
	}

	void Update () {
		if (Time.time - lastMeal > dna.starvationTime)
			Destroy (this.gameObject);

		if (Time.time - lastMeal > dna.fastingTime)
			isHungry = true;
		if (Time.time - lastSex > dna.abstainingTime)
			isHorny = true;

		float minPredator = Mathf.Infinity;
		float minFood = Mathf.Infinity;
		float minMate = Mathf.Infinity;
		predator = food = mate = null;

		foreach (Collider collider in Physics.OverlapSphere(transform.position, dna.sensoryRange)) {
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
		Gizmos.DrawWireSphere (transform.position, dna.sensoryRange);

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
//		Debug.Log (GetInstanceID ().ToString () + " eats " + obj.GetInstanceID ());
		Destroy (obj);
		lastMeal = Time.time;
		isHungry = false;
	}

	void Love (GameObject obj) {
//		Debug.Log (GetInstanceID ().ToString () + " does it with " + obj.GetInstanceID ());
		lastSex = Time.time;
		isHorny = false;

		if (GetSex () == Sex.Female) {
			Animal newBaby = GiveBirth ();
//			Debug.Log (GetInstanceID ().ToString () + " gives birth to " + newBaby.GetInstanceID ());
		}
	}

	Sex GetSex () {
		return sex;
	}

	Animal GiveBirth () {
		GameObject obj = (GameObject)Instantiate (baby, transform.position, Quaternion.identity);
		Animal newBaby = obj.GetComponent<Animal> ();

		// Mutation
		float mutationRate = 0.5f;//.arbitrary mutation rate
		if (Random.value < mutationRate) {
			newBaby.dna.maxSpeed = dna.maxSpeed + Random.Range (-200, 200) / 100f;
			newBaby.GetComponent<Vehicle> ().maxSpeed = newBaby.dna.maxSpeed;
			Debug.Log (gameObject.tag + " speed mutation: " + newBaby.dna.maxSpeed);
		}
		if (Random.value < mutationRate) {
			newBaby.dna.maxForce = dna.maxForce + Random.Range (-200, 200) / 100f;
			newBaby.GetComponent<Vehicle> ().maxForce = newBaby.dna.maxForce;
			Debug.Log (gameObject.tag + " force mutation: " + newBaby.dna.maxForce);
		}
		if (Random.value < mutationRate) {
			newBaby.dna.sensoryRange = dna.sensoryRange + Random.Range (-100, 100) / 100f;
			Debug.Log (gameObject.tag + " perception mutation: " + newBaby.dna.sensoryRange);
		}

		return newBaby;
	}
}
