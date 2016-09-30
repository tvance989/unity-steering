using UnityEngine;
using System.Collections;

public class RabbitController : MonoBehaviour {
	public float sensoryRange;
	public float fastingTime;
	public float starvationTime;
	public float abstainingTime;

	public GameObject baby;

	Vehicle vehicle;

	GameObject predator;
	GameObject mate;
	GameObject food;

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
		GetComponent<Renderer> ().material.color = sex == Sex.Male ? Color.white : Color.gray;
	}
	
	void Update () {
		if (Time.time - lastMeal > starvationTime)
			Destroy (this.gameObject);
		
		isHungry = isHorny = false;
		if (Time.time - lastMeal > fastingTime)
			isHungry = true;
		if (Time.time - lastSex > abstainingTime)
			isHorny = true;
		
		float minFox = Mathf.Infinity;
		float minCarrot = Mathf.Infinity;
		float minRabbit = Mathf.Infinity;
		predator = food = mate = null;
		foreach (Collider collider in Physics.OverlapSphere(transform.position, sensoryRange)) {
			GameObject obj = collider.gameObject;
			if (obj == this.gameObject)
				continue;
			if (obj.CompareTag ("Fox")) {
				float d = (obj.transform.position - transform.position).magnitude;
				if (d < minFox) {
					predator = obj.gameObject;
					minFox = d;
				}
			} else if (isHungry && obj.CompareTag ("Carrot")) {
				float d = (obj.transform.position - transform.position).magnitude;
				if (d < minCarrot) {
					food = obj.gameObject;
					minCarrot = d;
				}
			} else if (isHorny && obj.CompareTag ("Rabbit") && obj.GetComponent<RabbitController> ().GetSex () != sex) {
				float d = (obj.transform.position - transform.position).magnitude;
				if (d < minRabbit) {
					mate = obj.gameObject;
					minRabbit = d;
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
			force += vehicle.Seek (food) * 2;
		} else if (mate) {
			force += vehicle.Pursue (mate) * 3;
		} else {
			force += vehicle.Wander ();
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
			if (sex == Sex.Female)
				Instantiate (baby, transform.position, Quaternion.identity);
		}
	}

	Sex GetSex () {
		return sex;
	}
}
