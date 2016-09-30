using UnityEngine;
using System.Collections;

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

		lastMeal = Time.time + Random.Range (0, fastingTime);
		lastSex = Time.time + Random.Range (0, abstainingTime);

		sex = Random.value < 0.5f ? Sex.Male : Sex.Female;
		GetComponent<Renderer> ().material.color = sex == Sex.Male ? maleColor : femaleColor;
	}

	void Update () {
		if (Time.time - lastMeal > starvationTime)
			Destroy (this.gameObject);

		isHungry = isHorny = false;

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
			} else if (foodTag != "" && isHungry && obj.CompareTag (foodTag)) {
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
	}

	void FixedUpdate () {
		Vector3 force = Vector3.zero;

		if (predator) {
			force += vehicle.Evade (predator) * 4;
			force += vehicle.AvoidObstacles ();
		} else if (food) {
			force += vehicle.Pursue (food);
		} else if (mate) {
			force += vehicle.Pursue (mate);
		} else {
			force += vehicle.Wander () * 2;
			force += vehicle.AvoidObstacles ();
		}

		vehicle.ApplyForce (force);
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject == food) {
			Destroy (collision.gameObject);
			lastMeal = Time.time;
		} else if (collision.gameObject == mate) {
			lastSex = Time.time;
			if (GetSex () == Sex.Female)
				Instantiate (baby, transform.position, Quaternion.identity);
		}
	}

	Sex GetSex () {
		return sex;
	}
}
