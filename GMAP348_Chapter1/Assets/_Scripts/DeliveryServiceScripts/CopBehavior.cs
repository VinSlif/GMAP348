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
		Return,
		Retire
	}

	//private Project2GameManager gameManager;
	private State currState = State.Chase;

	private NavMeshAgent agent;

	[HideInInspector]
	public GameObject chasingCar;

	private Vector3 policeStation;

	// Use this for initialization
	void Start() {
		//gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();

		agent = GetComponent<NavMeshAgent>();

		policeStation = transform.position;
	}

	// Update is called once per frame
	void Update() {

		switch(currState) {
		case State.Start:
			currState = State.Chase;

			break;
		case State.Chase:
			agent.SetDestination(chasingCar.transform.position);

			if (Vector3.Distance(transform.position, chasingCar.transform.position) <= 2.0f) {

				currState = State.Return;
			}

			break;
		case State.Return:

			agent.SetDestination(policeStation);

			agent.speed = 4.0f;

			if (Vector3.Distance(gameObject.transform.position, policeStation) <= 3.0f) {
				currState = State.Retire;
			}

			break;
		case State.Retire:
			chasingCar.GetComponent<CarBehavior>().didCrime = false;
			Destroy(this.gameObject);

			break;
		}
	}

	// Don't know if redundant from above
	void OnCollisionEnter(Collision col) {
		if (col.gameObject == chasingCar && currState == State.Chase) {
			currState = State.Return;
		}
	}
}