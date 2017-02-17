using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : MonoBehaviour {

	public enum State {
		Start,
		Stage1,
		Stage2,
		Full,
		Harvest,
		Dead
	}

	[Serializable]
	public class Timers {
		public float growTime = 10.0f;
		[HideInInspector]
		public float growTimer = 0;

		public float decayTime = 30.0f;
		[HideInInspector]
		public float decayTimer = 0;

		public float waterTime = 30.0f;
		[HideInInspector]
		public float waterTimer = 0;
	}

	public State currState = State.Start;
	public Timers timer = new Timers();

	public GameObject[] growthStages = new GameObject[3];

	public GameObject seed;
	public float drops = 2.0f;
	private float toDrop = 0;

	[HideInInspector]
	public bool watered = false;
	[HideInInspector]
	public bool fertilized = false;
	[HideInInspector]
	public bool harvesting = false;
	
	// Update is called once per frame
	void Update() {
		switch(currState) {
		case State.Start:
			timer.growTimer = timer.growTime;
			timer.decayTimer = timer.decayTime;
			timer.waterTimer = timer.waterTime;

			HideGrowth();

			currState = State.Stage1;

			break;
		case State.Stage1:

			DisplayGrowth(currState);

			if (CheckWater()) {
				CheckDecay();
			} else {
				CheckGrowth(State.Stage2);
			}

			break;
		case State.Stage2:

			DisplayGrowth(currState);

			if (CheckWater()) {
				CheckDecay();
			} else {
				CheckGrowth(State.Full);
			}

			break;
		case State.Full:

			DisplayGrowth(currState);

			if (fertilized) {
				toDrop = drops * 2;
			} else {
				toDrop = drops;
			}

			if (harvesting) {
				currState = State.Harvest;
			}

			break;
		case State.Harvest:
			for (int i = 0; i < toDrop; i++) {
				GameObject newSeed = Instantiate(seed, transform.position, Quaternion.identity);
				newSeed.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.Range(-5.0f, 5.0f) * 50.0f,
					UnityEngine.Random.Range(0, 10.0f) * 50.0f,
					UnityEngine.Random.Range(-5.0f, 5.0f) * 50.0f);
			}

			Destroy(gameObject);

			break;
		case State.Dead:
			break;
		}
	}

	bool CheckWater() {
		bool setToDecay = false;

		if (watered) {
			timer.waterTimer = timer.waterTime;
			timer.decayTimer = timer.decayTime;
			watered = false;
		}

		timer.waterTimer -= Time.deltaTime;

		if (timer.waterTimer <= 0) {
			setToDecay = true;
		}

		return setToDecay;
	}

	void CheckDecay() {
		timer.decayTimer -= Time.deltaTime;

		if (timer.decayTimer <= 0) {
			currState = State.Dead;
		}
	}

	void CheckGrowth(State next) {
		timer.growTimer -= Time.deltaTime;

		if (timer.growTimer <= 0) {
			timer.decayTimer = timer.decayTime;
			timer.growTimer = timer.growTime;
			currState = next;
		}

	}

	void HideGrowth() {
		for (int i = 0; i < growthStages.Length; i++) {
			growthStages[i].SetActive(false);
		}
	}

	void DisplayGrowth(State state) {
		for (int i = 0; i < (int)state; i++) {
			growthStages[i].SetActive(true);
		}
	}
}