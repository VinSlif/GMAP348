﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project1GameManager : MonoBehaviour {

	public GameObject pickUpPrefab;

	public Transform delPickPoint;
	public bool isPickUpSpawned = false;
	public bool isDeliverPicked = false;
	public static bool isDelivering = false;
	private GameObject[] delLoc;

	public GameObject pedHolder;
	public GameObject pedPrefab;
	private GameObject[] pedestrian;
	public int pedSpawn = 1;
	private Transform[] pedLoc;

	// Use this for initialization
	void Start () {
		delLoc = GameObject.FindGameObjectsWithTag("DeliveryPoint");

		GameObject[] pedLocObject = GameObject.FindGameObjectsWithTag("PedestrianPoint").OrderBy( go => go.name ).ToArray();
		pedLoc = new Transform[pedLocObject.Length];
		for (int i=0; i < pedLocObject.Length; i++) {
			pedLoc[i] = pedLocObject[i].transform;
		}
		SpawnPedestrians(pedLoc.Length);

		ClearDeliverPoints();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDelivering) {
			ClearDeliverPoints();
			if (!isPickUpSpawned) {
				SetPedestrians(pedSpawn);
				Instantiate(pickUpPrefab, delPickPoint.position, delPickPoint.localRotation);
				isDeliverPicked = false;
				isPickUpSpawned = true;
			}
		} else {
			if (!isDeliverPicked) {
				PickDeliveryPoints(0);
				SetPedestrians(pedSpawn);
				isDeliverPicked = true;
				isPickUpSpawned = false;
			}
		}

		// Debug
		if (Input.GetKeyDown (KeyCode.F1)) {
			for (int i = 0; i < 40; i++) {
				pedestrian[i].GetComponent<Pedestrians>().Crossing(i);
			}
		}
	}

	void SpawnPedestrians(int needed) {
		pedestrian = new GameObject[needed];
		for (int i=0; i < needed; i++) {
			pedestrian[i] = Instantiate(pedPrefab, pedLoc[i].position, pedLoc[i].localRotation);
			//pedestrian[i].SetActive(false);
			pedestrian[i].name = "pedestrian " + i + " (clone)";
			pedestrian[i].transform.parent = pedHolder.transform;
		}
	}

	void ClearDeliverPoints() {
		for (int i=0; i < delLoc.Length; i++) {
			delLoc[i].SetActive(false);
		}
	}

	void PickDeliveryPoints(int needed) {
		for (int i=0; i < needed + 1; i++) {
			delLoc[Random.Range(0, delLoc.Length)].SetActive(true);
		}
	}

	void SetPedestrians(int needed) {
		for (int i=0; i < needed; i++) {
			int randomPedestrian;
			do {
				randomPedestrian = Random.Range (0, pedLoc.Length + 1);
			} while (pedestrian [randomPedestrian].GetComponent<Pedestrians> ().crossingBool);
			pedestrian[randomPedestrian].GetComponent<Pedestrians>().Crossing(randomPedestrian);
		}
	}

	public void ChangePedLocation (int pedIndex) {
		int newIndex = 0;
		do {
			newIndex = Random.Range (0, pedestrian.Length);
		} while (newIndex == pedIndex && pedestrian [newIndex].GetComponent<Pedestrians> ().crossingBool);

		int temp = pedestrian [pedIndex].GetComponent<Pedestrians> ().karmaCount;
		pedestrian [pedIndex].GetComponent<Pedestrians> ().karmaCount = pedestrian [newIndex].GetComponent<Pedestrians> ().karmaCount;
		pedestrian [newIndex].GetComponent<Pedestrians> ().karmaCount = temp;
	}
}