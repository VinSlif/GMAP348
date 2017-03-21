using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockBehavior : MonoBehaviour {

	public float deliverStart;
	public float deliverNum = 3.0f;

	private Project1GameManager gameManager;
	public int index = 0;

	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project1GameManager>();
		deliverStart = Project1GameManager.delivered;
	}

	void Update() {
		if (Project1GameManager.delivered > deliverStart + deliverNum) {
			gameManager.CommunicateHitBool(index, false);
			Destroy(this.gameObject);
		}
	}
}