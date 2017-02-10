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
		Waiting,
		ReachedGoal
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

	// Use this for initialization
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();

		agent = GetComponent<NavMeshAgent>();

		followers.GetFollowers(followers.count, this.transform);
		AllChangeColor(color);
	}
	
	// Update is called once per frame
	void Update() {

		if (currState == State.Start) {
			agent.destination = gameManager.ped.SelectPoint();

			currState = State.Travelling;

		} else if (currState == State.Travelling) {

		} else if (currState == State.Waiting) {

		} else if (currState == State.ReachedGoal) {

			// reset
			currState = State.Start;
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