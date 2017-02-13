using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CarBehavior : MonoBehaviour {

	public enum State {
		Start,
		GetPickup,
		Delivering,
		Delivered,
		Chased,
		Arrested
	}

	[HideInInspector]
	public int index;
	[HideInInspector]
	public bool didCrime = false;
	[HideInInspector]
	public bool arresting = false;
	[HideInInspector]
	public GameObject arrestingOfficer;

	// Did crime timer
	public float criminalResetTime = 5.0f;
	private float crimeTimer = 0;

	public ParticleSystem bloodPrefab;

	public Color color;

	private Project2GameManager gameManager;
	private State currState = State.Start;
	private State prevState;
	private NavMeshAgent agent;

	// Prevent recursion
	private bool hasPoint = false;
	private Vector3 finalDestination;
	private Vector3 chaseDestination;
	private bool setToChaseState = false;
	// Check if reached delivery point
	private float checkDistance = 3.0f;

	// Use this for initialization
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();

		agent = GetComponent<NavMeshAgent>();

		AllChangeColor(color);
	}

	// Update is called once per frame
	void Update() {

		if (didCrime) {
			crimeTimer -= Time.deltaTime;
			if (crimeTimer <= 0) {
				didCrime = false;
			}
		} else {
			crimeTimer = criminalResetTime;
		}

		switch(currState) {
		case State.Start:
			// Spawn pickup
			gameManager.pickUp.Spawn(index);

			hasPoint = false;

			setToChaseState = false;
			chaseDestination = Vector3.zero;
			arrestingOfficer = null;

			if (arresting) {
				currState = State.Chased;
			}

			currState = State.GetPickup;
			break;
		case State.GetPickup:
			// Go to Pick Up
			agent.SetDestination(gameManager.pickUp.pickPoint.position);

			prevState = currState;

			if (arresting) {
				currState = State.Chased;
			}

			// Handled by collision
			break;
		case State.Delivering:
			if (!hasPoint) { // Set destination once
				finalDestination = gameManager.del.SelectPoint();
				agent.SetDestination(finalDestination);
				hasPoint = true;
			}

			prevState = currState;

			if (arresting) {
				currState = State.Chased;
			}

			// Handled by collision
			break;
		case State.Delivered:
			Project2GameManager.delivered++;

			// reset
			currState = State.Start;
			break;
		case State.Chased:

			/* Get Rid of pick up and delivery point
			 * not working right now
			if (!setToChaseState) {
				setToChaseState = true;
				finalDestination = Vector3.zero;

				if (prevState == State.Delivering) {
					gameManager.del.go[gameManager.del.interpretPoint(finalDestination)].SetActive(false);
				}

				GameObject[] pickups = FindObjectsOfType(typeof(PickUpBehavior)) as GameObject[];
				for (int i = 0; i < pickups.Length; i++) {
					if (pickups[i].GetComponent<PickUpBehavior>().index == index) {
						Destroy(pickups[i].gameObject);
					}
				}
			}
			*/

			if (!hasPoint) { // Set destination once
				chaseDestination = gameManager.del.randomPoint();
				agent.SetDestination(chaseDestination);
				hasPoint = true;
			}

			if (Vector3.Distance(transform.position, chaseDestination) <= checkDistance) {
				hasPoint = false;
			}
				
			if (Vector3.Distance(transform.position, arrestingOfficer.transform.position) <= checkDistance) {
				arresting = false;
				currState = State.Arrested;
			}

			break;
		case State.Arrested:
			if (arrestingOfficer != null) {
				agent.SetDestination(arrestingOfficer.transform.position);
			} else {
				didCrime = false;
				currState = State.Start;
			}

			break;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (currState == State.GetPickup && col.gameObject.GetComponent<PickUpBehavior>() != null) {
			if (col.gameObject.GetComponent<PickUpBehavior>().index == index) {
				col.transform.position = transform.GetChild(0).transform.position;
				col.transform.parent = transform;

				currState = State.Delivering;
			}
		}

		if (currState == State.Delivering && col.gameObject.tag == gameManager.del.pointTag) {
			if (Vector3.Distance(transform.position, finalDestination) <= checkDistance) {
				Destroy(transform.GetChild(2).gameObject);
				col.gameObject.SetActive(false);

				currState = State.Delivered;
			}
		}
	}

	void OnCollisionEnter(Collision col) {
		// will throw an error before start gets called
		if (col.gameObject.tag == gameManager.ped.tag) {
			didCrime = true;

			for (int i = 0; i < col.contacts.Length; i++) {
				ParticleSystem blood = Instantiate(bloodPrefab, col.contacts[i].point, Quaternion.identity);
				blood.transform.parent = transform;
			}

			Physics.IgnoreCollision(col.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
		}

		/*
		if (currState == State.Chased && didCrime && col.gameObject == arrestingOfficer) {
			// arrested changed to true by copCar
			currState = State.Arrested;
		}*/
	}

	void AllChangeColor(Color color) {
		MeshRenderer[] all = transform.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer rend in all) {
			rend.material.color = color;
		}
	}
}