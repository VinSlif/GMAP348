﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PedestrianLeaderBehavior : MonoBehaviour {

	public enum State {
		Start,
		Travelling,
		Visiting
	}

	private float visitTimer = 0;
	private float visitDuration = 0;

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
	public float checkDistance = 2.0f;
	public Color color;

	public Followers followers = new Followers();

	private Project2GameManager gameManager;
	private State currState = State.Start;
	private NavMeshAgent agent;
	private Vector3 finalDestination;

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
			finalDestination = gameManager.ped.SelectPoint ();
			agent.destination = finalDestination;

			currState = State.Travelling;

		} else if (currState == State.Travelling) {
			if (Vector3.Distance (transform.position, finalDestination) < checkDistance) {
				visitDuration = UnityEngine.Random.Range (2.0f, 5.0f);
				visitTimer = 0;
				currState = State.Visiting;
				Debug.Log (this.gameObject.transform.parent.name + " is visiting");
			}

		} else if (currState == State.Visiting) {
			visitTimer += Time.deltaTime;
			if (visitTimer >= visitDuration) {
				// reset
				currState = State.Start;
			}
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