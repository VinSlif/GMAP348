using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrians : MonoBehaviour {

	public int karmaCount = 0;
	private int pedIndex = 0;

	public Material[] karmaMat = new Material[3];
	// index 0 = Karma_1
	//       1 = Karma_0
	//       2 = Karma_-1

	private GameObject peopleObj;
	private Transform ped_transform;

	private float yInactive = -10f;
	private float yActive = 0f;

	private float timeStart;
	private float currTime;
	private float duration = 0f;
	private float fullDuration = 10f;
	private float partDuration = 2f;
	private float percentageDone;
	private float currWalkPos;

	private const int LOWKARMA = -2;
	private const int HIGHKARMA = 2;

	private float carEnterTime;
	private float carStayTime;
	private bool honked = false;
	private bool angry = false;

	public bool crossingBool = false;
	private Project1GameManager gameManager;


	// Use this for initialization

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project1GameManager> ();
		peopleObj = this.gameObject.transform.GetChild (1).gameObject;
		ped_transform = this.gameObject.transform;
		ped_transform.position = new Vector3 (ped_transform.position.x, yInactive, ped_transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		currTime += Time.deltaTime;
		percentageDone = (currTime - timeStart) / duration;
		currWalkPos = Mathf.Lerp (-3f, 3f, percentageDone);

		if (!angry) {
			peopleObj.transform.localPosition = new Vector3( 0, 0, currWalkPos);
		} else {
			peopleObj.transform.localPosition = new Vector3( 0, 0, 0);
		}

		if (percentageDone >= 1f && crossingBool) {
			FinishedCrossing();
			crossingBool = false;
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player") {
			Debug.Log("Karma: " + karmaCount);
			if (karmaCount == HIGHKARMA) {
				duration = partDuration;
			}
			carEnterTime = Time.time;
			carStayTime = Time.time;
		}
	}

	void OnTriggerStay (Collider col) {
		if (col.gameObject.tag == "Player" && !honked) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				honked = true;
				karmaCount--;
				ChangeMaterial ();
				if (karmaCount < LOWKARMA) {
					karmaCount = LOWKARMA;
					angry = true;
				} else if (karmaCount > LOWKARMA){
					duration = partDuration;
				}
			} else {
				carStayTime += Time.deltaTime;
				if ((carStayTime - carEnterTime) >= 3f) {
					karmaCount++;
					duration = partDuration;
					ChangeMaterial ();
					if (karmaCount > HIGHKARMA) {
						karmaCount = HIGHKARMA;
					}
				}
			}
		} 
	}

	void OnTriggerExit (Collider col) {
		Debug.Log("Karma: " + karmaCount);
		if (col.gameObject.tag == "Player" && angry) {
			angry = false;
			duration = fullDuration;
			timeStart = Time.time - (fullDuration/2);
			currTime = Time.time;
		}
	}

	public void Crossing (int index) {
		ChangeMaterial ();
		crossingBool = true;
		ped_transform.position = new Vector3 (ped_transform.position.x, yActive, ped_transform.position.z);
		peopleObj.transform.localPosition = new Vector3( 0, 0, -3f);
		timeStart = Time.time;
		currTime = Time.time;
		honked = false;
		duration = fullDuration;
		angry = false;
		pedIndex = index;
	}

	private void FinishedCrossing () {
		Debug.Log ("called Finished");
		ped_transform.position = new Vector3 (ped_transform.position.x, yInactive, ped_transform.position.z);
		gameManager.ChangePedLocation (pedIndex);
	}

	private void ChangeMaterial () {
		int newMat;
		if (karmaCount < 0) {
			newMat = 2;
		} else if (karmaCount == 0) {
			newMat = 1;
		} else {
			newMat = 0;
		}
			
		Transform[] allchildren = peopleObj.GetComponentsInChildren<Transform>();
		foreach (Transform child in allchildren) {
			if (child.gameObject.GetComponent<MeshRenderer> () != null) {
				child.gameObject.GetComponent<MeshRenderer> ().material = karmaMat [newMat];
			}
		}
	}
}