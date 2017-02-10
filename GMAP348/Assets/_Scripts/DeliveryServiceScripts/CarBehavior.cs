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

	public bool didCrime = false;

	private Project2GameManager gameManager;
	private State currState = State.Start;
	private NavMeshAgent agent;

	// Prevent recursion
	private bool hasPoint = false;
	// Check if reached delivery point
	private float checkDistance = 3.0f;

	// Use this for initialization
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();

		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update() {

		if (currState == State.Start) {
			// Spawn pickup
			gameManager.pickUp.Spawn(index);

			hasPoint = false;

			currState = State.GetPickup;

		} else if (currState == State.GetPickup) {
			// Go to Pick Up
			agent.destination = gameManager.pickUp.pickPoint.position;

			// Handled by collision

		} else if (currState == State.Delivering) {
			if (!hasPoint) { // Set destination once
				agent.destination = gameManager.del.SelectPoint();
				hasPoint = true;
			}

			// Handled by collision

		} else if (currState == State.Delivered) {
			Project2GameManager.delivered++;

			// reset
			currState = State.Start;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.GetComponent<PickUpBehavior>() != null && currState == State.GetPickup) {
			if (col.gameObject.GetComponent<PickUpBehavior>().index == index) {
				col.transform.position = transform.GetChild(0).transform.position;
				col.transform.parent = transform;

				currState = State.Delivering;
			}
		}

		if (col.gameObject.tag == gameManager.del.pointTag && currState == State.Delivering) {
			if (Vector3.Distance(agent.destination, col.gameObject.transform.position) <= checkDistance) {
				Destroy(transform.GetChild(2).gameObject);
				col.gameObject.SetActive(false);

				currState = State.Delivered;
			}
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == gameManager.ped.tag) {
			didCrime = true;
		}
	}
}