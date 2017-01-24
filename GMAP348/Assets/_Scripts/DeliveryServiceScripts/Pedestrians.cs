using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrians : MonoBehaviour {

	public int karmaCount = 0;

	public GameObject peopleObj;
	private Transform ped_transform;
	private float timeStart;
	private float currTime;
	private float duration = 0f;
	private float fullDuration = 10f;
	private float partDuration = 2f;
	private float percentageDone;
	private float currWalkPos;

	private float carEnterTime;
	private float carStayTime;
	private bool honked = false;
	private bool angry = false;

	private bool crossingBool = false;


	// Use this for initialization

	void Start () {
		ped_transform = this.gameObject.transform;
		ped_transform.position = new Vector3 (ped_transform.position.x, -3f, ped_transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		currTime += Time.deltaTime;
		percentageDone = (currTime - timeStart) / duration;
		currWalkPos = Mathf.Lerp (-3, 3, percentageDone);

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
			if (karmaCount == 3) {
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
				if (karmaCount < -3) {
					karmaCount = -3;
					angry = true;
				} else if (karmaCount > -3){
					duration = partDuration;
				}
			} else {
				carStayTime += Time.deltaTime;
				if ((carStayTime - carEnterTime) >= 3f) {
					karmaCount++;
					duration = partDuration;
					if (karmaCount > 3) {
						karmaCount = 3;
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

	public void Crossing () {
		crossingBool = true;
		ped_transform.position = new Vector3 (ped_transform.position.x, 0, ped_transform.position.z);
		peopleObj.transform.localPosition = new Vector3( 0, 0, -3f);
		timeStart = Time.time;
		currTime = Time.time;
		honked = false;
		duration = fullDuration;
		angry = false;
	}

	private void FinishedCrossing () {
		Debug.Log ("called Finished");
		ped_transform.position = new Vector3 (ped_transform.position.x, -3f, ped_transform.position.z);
	}
}