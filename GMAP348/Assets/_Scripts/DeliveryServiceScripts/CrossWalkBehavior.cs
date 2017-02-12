﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrossWalkBehavior : MonoBehaviour {

	public enum State {
		Cars,
		Peds,
		Chaos,
		Blocked
	}

	[Tooltip("1 out of [chance]")]
	public int chance = 5;

	[HideInInspector]
	public int pedsInTriggers = 0;

	//private Project2GameManager gameManager;
	private State currState = State.Peds;

	private GameObject roadBlockMesh;

	private NavMeshObstacle[] pedObstacle;
	private NavMeshObstacle[] carObstacle;

	// Use this for initialization
	void Start() {
		//gameManager = GameObject.FindGameObjectWithTag ("ManagerTag").GetComponent<Project2GameManager> ();

		roadBlockMesh = transform.GetChild(3).gameObject;

		// Set Nav Mesh Obstacles references
		pedObstacle = transform.GetChild(1).GetComponentsInChildren<NavMeshObstacle>();
		carObstacle = transform.GetChild(2).GetComponentsInChildren<NavMeshObstacle>();
	}

	// Update is called once per frame
	void Update() {

		switch(currState) {
		case State.Cars:
			for (int i = 0; i < carObstacle.Length; i++) {
				carObstacle[i].carving = false;
				carObstacle[i].enabled = false;
			}
			for (int i = 0; i < pedObstacle.Length; i++) {
				pedObstacle[i].enabled = true;
				pedObstacle[i].carving = true;
			}

			if (pedsInTriggers >= 3 && currState != State.Blocked) {
				if ((int)Random.Range(0, chance + 1) == 1) {
					currState = State.Chaos;
				} else {
					currState = State.Peds;
				}
			}
			
			break;
		case State.Peds:
			for (int i = 0; i < carObstacle.Length; i++) {
				carObstacle[i].enabled = true;
				carObstacle[i].carving = true;
			}
			for (int i = 0; i < pedObstacle.Length; i++) {
				pedObstacle[i].carving = false;
				pedObstacle[i].enabled = false;
			}

			if (pedsInTriggers <= 1 && currState != State.Blocked) {
				currState = State.Cars;
			}
			
			break;
		case State.Chaos:
			for (int i = 0; i < carObstacle.Length; i++) {
				carObstacle[i].carving = false;
				carObstacle[i].enabled = false;
			}
			for (int i = 0; i < pedObstacle.Length; i++) {
				pedObstacle[i].carving = false;
				pedObstacle[i].enabled = false;
			}
			
			break;
		case State.Blocked:
			for (int i = 0; i < carObstacle.Length; i++) {
				carObstacle[i].enabled = true;
				carObstacle[i].carving = true;
			}
			for (int i = 0; i < pedObstacle.Length; i++) {
				pedObstacle[i].enabled = true;
				pedObstacle[i].carving = true;
			}

			break;
		}

		if (currState == State.Blocked) {
			roadBlockMesh.SetActive(true);
		} else {
			roadBlockMesh.SetActive(false);
		}
	}
}