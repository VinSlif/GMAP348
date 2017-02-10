using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour {

	public float deliverStart;
	public float deliverNum = 3.0f;

	//private Project2GameManager gameManager;

	[HideInInspector]
	public int index = 0;

	void Start() {
		//gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();
		deliverStart = Project1GameManager.delivered;
	}

	void Update() {
		if (Project2GameManager.delivered > deliverStart + deliverNum) {
			Destroy(this.gameObject);
		}
	}
}