using UnityEngine;
using System.Collections;

public class FoxController : MonoBehaviour {
	public float sensoryRange;
	public float fastingTime;
	public float starvationTime;
	public int mealsUntilSex;

	public GameObject baby;

	Vehicle vehicle;

	GameObject prey;
	GameObject mate;

	float lastMeal;
	bool isHungry;
	int mealsSinceSex;
	bool isHorny;

	enum Sex {Male, Female};
	Sex sex;

	void Start () {
		vehicle = GetComponent<Vehicle> ();

		lastMeal = Time.time + Random.Range (0, fastingTime);
		mealsSinceSex = 0;

		sex = Random.value < 0.5f ? Sex.Male : Sex.Female;
		GetComponent<Renderer> ().material.color = sex == Sex.Male ? Color.red : Color.magenta;
	}

	void Update () {
		if (Time.time - lastMeal > starvationTime)
			Destroy (this.gameObject);
		
		isHungry = isHorny = false;
		if (Time.time - lastMeal > fastingTime)
			isHungry = true;
		if (mealsSinceSex >= mealsUntilSex)
			isHorny = true;

		float minFox = Mathf.Infinity;
		float minRabbit = Mathf.Infinity;
		prey = mate = null;
		foreach (Collider collider in Physics.OverlapSphere(transform.position, sensoryRange)) {
			GameObject obj = collider.gameObject;
			if (obj == this.gameObject)
				continue;
			
			if (isHungry && obj.CompareTag ("Rabbit")) {
				float d = (obj.transform.position - transform.position).magnitude;
				if (d < minRabbit) {
					prey = obj.gameObject;
					minRabbit = d;
				}
			} else if (isHorny && obj.CompareTag ("Fox") && obj.GetComponent<FoxController> ().GetSex () != sex) {
				float d = (obj.transform.position - transform.position).magnitude;
				if (d < minFox) {
					mate = obj.gameObject;
					minFox = d;
				}
			}
		}
	}

	void FixedUpdate () {
		Vector3 force = Vector3.zero;

		if (prey) {
			force += vehicle.Pursue (prey) * 4;
		} else if (mate) {
			force += vehicle.Pursue (mate) * 2;
		} else {
			force += vehicle.Wander ();
		}
		force += vehicle.AvoidObstacles ();

		vehicle.ApplyForce (force);
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject == mate) {
			mealsSinceSex = 0;
			if (sex == Sex.Female)
				Instantiate (baby, transform.position, Quaternion.identity);
		} else if (collision.gameObject == prey) {
			Destroy (collision.gameObject);
			lastMeal = Time.time;
		}
	}

	Sex GetSex () {
		return sex;
	}
}
