using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PedestrianLeaderBehavior : MonoBehaviour {

	public enum State {
		Start,
		Travelling,
		Visiting,
	}

	[Serializable]
	public class Followers {
		public GameObject prefab;
		public int count = 2;

		[HideInInspector]
		public GameObject[] go;

		public void GetFollowers(int needed, Transform parent) {
			go = new GameObject[needed];

			for (int i = 0; i < needed; i++) {
				go[i] = Instantiate(prefab, parent.position, Quaternion.identity);
				go[i].name = "PedestrianFollower " + i + " (clone)";
				go[i].GetComponent<PedestrianFollowerBehavior>().leader = parent.gameObject;
				go[i].transform.parent = parent.parent;
			}
		}

		public void CheckFollowers(int needed, Transform parent) {
			for (int i = 0; i < needed; i++) {
				if (go[i] == null) {
					go[i] = Instantiate(prefab, parent.position, Quaternion.identity);
					go[i].name = "PedestrianFollower " + i + " (clone)";
					go[i].GetComponent<PedestrianFollowerBehavior>().leader = parent.gameObject;
					go[i].transform.parent = parent.parent;
				}
			}
		}
	}

	public int index;
	public Color color;

	public Followers followers = new Followers();

	private Project2GameManager gameManager;
	private State currState = State.Start;

	private NavMeshAgent agent;

	private Vector3 finalDestination;
	private float checkDistance = 3.0f;

	private float visitDuration = 0;
	private float visitTimer = 0;

	// Use this for initialization
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();

		agent = GetComponent<NavMeshAgent>();

		followers.GetFollowers(followers.count, this.transform);
		AllChangeColor(color);
	}
	
	// Update is called once per frame
	void Update() {

		switch(currState) {
		case State.Start:
			finalDestination = gameManager.ped.SelectPoint();
			agent.SetDestination(finalDestination);

			currState = State.Travelling;

			break;
		case State.Travelling:
			if (Vector3.Distance(transform.position, finalDestination) < checkDistance) {
				// set visit timers
				visitDuration = UnityEngine.Random.Range(2.0f, 5.0f);
				visitTimer = visitDuration;

				// pick visit destination
				finalDestination = gameManager.ped.GetClosestDestination(transform.position, gameManager.ped.visitLoc).position;
				agent.SetDestination(finalDestination);

				currState = State.Visiting;
			}

			break;
		case State.Visiting:
			visitTimer -= Time.deltaTime;
			if (visitTimer <= 0) {
				// reset
				currState = State.Start;
			}

			break;
		}

		followers.CheckFollowers(followers.count, this.transform);

	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == gameManager.car.tag) {
			agent.enabled = false;
			gameObject.GetComponent<Rigidbody>().isKinematic = false;
			agent.enabled = false;

			Destroy(this.gameObject, 5.0f);
		}
	}

	void AllChangeColor(Color color) {
		MeshRenderer[] all = transform.parent.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer rend in all) {
			rend.material.color = color;
		}
	}
}