using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : PlantTypes {

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
		[Tooltip("Time needed for stage to grow")]
		public float growTime = 10.0f;
		//[HideInInspector]
		public float growTimer = 0;

		[Tooltip("Time for plant to decay if not watered")]
		public float decayTime = 30.0f;
		//[HideInInspector]
		public float decayTimer = 0;

		[Tooltip("Time for plant to start decaying after being watered")]
		public float waterTime = 30.0f;
		//[HideInInspector]
		public float waterTimer = 0;
	}

	[Serializable]
	public class Growth {
		[Tooltip("The growth stage of the plant to display")]
		public GameObject stagesParent;

		//[HideInInspector]
		public GameObject[] stages;

		public void HideGrowth() {
			for (int i = 0; i < stages.Length; i++) {
				stages[i].SetActive(false);
			}
		}

		public void DisplayGrowth(int growthStage) {
			for (int i = 0; i < growthStage; i++) {
				stages[i].SetActive(true);
			}
		}
	}

	/*
	[Serializable]
	public class Decay {
		[Tooltip("Enemy plant to spawn if plant fully decays")]
		public GameObject decayedPlant;

		public void SetDecay() {
			if (!decayedPlant.activeSelf) {
				decayedPlant.SetActive(true);
			}
		}
	}
	*/

	[Serializable]
	public class Harvest {
		[Tooltip("Plant seed to spawn when harvesting")]
		public GameObject seed;
		[Tooltip("The number of seeds that will spawn when harvesting")]
		public int drops = 2;

		//[HideInInspector]
		public int toDrop = 0;

		public void SpawnSeeds(int needed, Transform pos, PlantType type) {
			for (int i = 0; i < needed; i++) {
				GameObject newSeed = Instantiate(seed, pos.position, Quaternion.identity);
				newSeed.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.Range(-5.0f, 5.0f) * 50.0f,
					UnityEngine.Random.Range(0, 10.0f) * 50.0f,
					UnityEngine.Random.Range(-5.0f, 5.0f) * 50.0f);
				newSeed.GetComponent<SeedBehavior>().seedType = type;
			}
		}
	}


	private State currState = State.Start;
	[Tooltip("The type of plant this is")]
	public PlantType type = PlantType.Kush;

	public Timers timer = new Timers();
	public Growth growth = new Growth();
	public Harvest harvest = new Harvest();
	//public Decay decay = new Decay();

	//[HideInInspector]
	public bool watered = false;
	//[HideInInspector]
	public bool fertilized = false;
	//[HideInInspector]
	public bool harvesting = false;


	// Update is called once per frame
	void Update() {
		switch(currState) {
		case State.Start:
			// Set all timers
			timer.growTimer = timer.growTime;
			timer.decayTimer = timer.decayTime;
			timer.waterTimer = timer.waterTime;

			// Get + Hide growth elements
			growth.stages = new GameObject[growth.stagesParent.transform.childCount];
			int i = 0;
			foreach(Transform child in growth.stagesParent.transform.GetComponent<Transform>()) {
				growth.stages[i] = child.gameObject;
				i++;
			}
			growth.HideGrowth();

			currState = State.Stage1;

			break;
		case State.Stage1:

			growth.DisplayGrowth((int)currState);

			if (CheckWater()) {
				CheckDecay();
			} else {
				CheckGrowth(State.Stage2);
			}

			break;
		case State.Stage2:

			growth.DisplayGrowth((int)currState);

			if (CheckWater()) {
				CheckDecay();
			} else {
				CheckGrowth(State.Full);
			}

			break;
		case State.Full:

			growth.DisplayGrowth((int)currState);

			if (fertilized) {
				harvest.toDrop = harvest.drops * 2;
			} else {
				harvest.toDrop = harvest.drops;
			}

			if (harvesting) {
				currState = State.Harvest;
			}

			break;
		case State.Harvest:
			
			harvest.SpawnSeeds(harvest.toDrop, transform, type);
			Destroy(gameObject);

			break;
		case State.Dead:
			//decay.SetDecay();
			Debug.Log("Plant " + gameObject.name + " is dead");

			break;
		}
	}


	// Determines if plant should be decaying
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

	// Checks if the plant should be dead
	void CheckDecay() {
		timer.decayTimer -= Time.deltaTime;

		if (timer.decayTimer <= 0) {
			currState = State.Dead;
		}
	}

	// Checks if the plant has reached next growth stage
	public void CheckGrowth(State next) {
		timer.growTimer -= Time.deltaTime;

		if (timer.growTimer <= 0) {
			timer.decayTimer = timer.decayTime;
			timer.growTimer = timer.growTime;
			currState = next;
		}
	}
}