using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CopBehavior : MonoBehaviour {

	public enum State {
		Start,
		Chase,
		Arrest,
		Retire
	}

	private Project2GameManager gameManager;
	private State currState = State.Chase;
	private NavMeshAgent agent;
	private GameObject chasingCar;
	private Vector3 policeStation;

	// Use this for initialization
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();

		agent = GetComponent<NavMeshAgent>();

		policeStation = new Vector3 (43.0f, 0, 0);
	}

	// Update is called once per frame
	void Update() {

		if (currState == State.Start) {

			if (chasingCar != null) {
				currState = State.Chase;
			}

		} else if (currState == State.Chase) {
			// Chase Car
			agent.destination = //chasingCar.transform.position;
				policeStation;

		} else if (currState == State.Arrest) {
			agent.destination = policeStation;
			if (Vector3.Distance (this.gameObject.transform.position, policeStation) <= 1.0f) {
				currState = State.Retire;
			}
		} else if (currState == State.Retire) {
			chasingCar.GetComponent<CarBehavior> ().didCrime = false;
			chasingCar.GetComponent<CarBehavior> ().arrested = false;
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject == chasingCar && currState == State.Chase) {
			col.gameObject.GetComponent<CarBehavior> ().arrested = true;
			currState = State.Arrest;
		}
	}

	//public void ChaseCar (GameObject car) {
	public void ChaseCar (int carID) {
		if (chasingCar != null)
			chasingCar.GetComponent<CarBehavior> ().arrested = false;

		chasingCar = gameManager.car.cars[carID];
		currState = State.Chase;
	}
}