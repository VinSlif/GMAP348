using System.Collections;
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

	public State currState = State.Cars;

	public float chance;
	private bool gotChance = false;

	private NavMeshObstacle[] pedObstacle;
	private NavMeshObstacle[] carObstacle;

	// Use this for initialization
	void Start() {
		// Set Nav Mesh Obstacles references
		pedObstacle = transform.GetChild(1).GetComponentsInChildren<NavMeshObstacle>();
		carObstacle = transform.GetChild(2).GetComponentsInChildren<NavMeshObstacle>();
	}
	
	// Update is called once per frame
	void Update() {
		if (currState == State.Cars) {

			if (carObstacle[0].carving == true) { // Prevent recursion
				for (int i = 0; i < carObstacle.Length; i++) {
					carObstacle[i].carving = false;
				}
				for (int i = 0; i < pedObstacle.Length; i++) {
					pedObstacle[i].carving = true;
				}
			}

		} else if (currState == State.Peds) {
		
			if (pedObstacle[0].carving == true) { // Prevent recursion
				for (int i = 0; i < carObstacle.Length; i++) {
					carObstacle[i].carving = true;
				}
				for (int i = 0; i < pedObstacle.Length; i++) {
					pedObstacle[i].carving = false;
				}
			}

		} else if (currState == State.Chaos) {

			if (chance < Random.Range(0, 100.0f) && !gotChance) {
				gotChance = true;

				for (int i = 0; i < carObstacle.Length; i++) {
					carObstacle[i].carving = false;
				}
				for (int i = 0; i < pedObstacle.Length; i++) {
					pedObstacle[i].carving = false;
				}
			} else {
				gotChance = false;
				currState = State.Peds;
			}

		}
	}
}