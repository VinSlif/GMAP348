using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianBehavior : MonoBehaviour {


	[Serializable]
	public class Crossing {
		[Header("Set as needed")]
		[Tooltip("in seconds")]
		public float crossTime = 5.0f;
		[Header("DO NOT TOUCH")]
		public float crossTimer = 0;
		public bool isCrossing = false;
		public int karmaTrack = 0;
	}

	[Serializable]
	public class Display {
		[Header("Set as needed")]
		public GameObject pedMeshes;
		public GameObject blockObject;
	}


	public Crossing cross = new Crossing();
	public Display disp = new Display();

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
		if (cross.isCrossing) {
			cross.crossTimer -= Time.deltaTime;
			if (cross.crossTimer <= 0) {
				cross.isCrossing = false;
			}
		} else {
			cross.crossTimer = cross.crossTime;
		}
	}

	void OnTiggerStay(Collider col) {
		if (col.gameObject.tag == "Player") {
			if (TopDownPlayerController.isHonkong) {
				cross.karmaTrack--;
			} else {
				cross.karmaTrack++;
			}
		}
	}
}