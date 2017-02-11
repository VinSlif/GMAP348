using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PedestrianFollowerBehavior : MonoBehaviour {

	[HideInInspector]
	public GameObject leader;

	private Project2GameManager gameManager;
	private NavMeshAgent agent;

	// Use this for initialization
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();

		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update() {
		if (leader != null) {
			agent.destination = leader.transform.position;
		} else {
			Destroy(this.transform.parent.gameObject);
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == gameManager.car.tag) {
			agent.enabled = false;
			gameObject.GetComponent<Rigidbody>().isKinematic = false;
			agent.enabled = false;

			Destroy(this.gameObject, 5.0f);
		}
	}
}