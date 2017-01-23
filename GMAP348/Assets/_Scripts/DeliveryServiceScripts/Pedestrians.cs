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
	private float percentageDone;
	private float currWalkPos;

	private float carEnterTime;
	private float carStayTime;
	private bool honked = false;
	private bool angry = false;

	private float randomCrossTime;


	// Use this for initialization
	void Start () {
		ped_transform = this.gameObject.transform;
		ped_transform.position = new Vector3 (ped_transform.position.x, -3, ped_transform.position.z);
		randomCrossTime = Random.Range (1f, 5f);
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

		if (percentageDone >= 1f) {
			FinishedCrossing();
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player") {
			if (karmaCount == 3) {
				duration = 4f;
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
					duration = 4f;
				}
			} else {
				if ((carStayTime - carEnterTime) >= 3f) {
					karmaCount++;
					duration = 4f;
					if (karmaCount > 3) {
						karmaCount = 3;
					}
				}
			}
		} 

		if (col.gameObject.tag == "DeliveryPoint") {
			Crossing(col.gameObject.GetComponent<PseudoDeliveryPoint>().deliveryPointTime);
			Debug.Log ("Called");
		}
		Debug.Log ("Called");
	}

	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player" && angry) {
			angry = false;
			duration = 8f;
			timeStart = Time.time - 4f;
			currTime = Time.time;
		}
	}

	private void Crossing (float crossTime) {
		if (crossTime >= randomCrossTime){
			ped_transform.position = new Vector3 (ped_transform.position.x, 0, ped_transform.position.z);
			peopleObj.transform.localPosition = new Vector3( 0, 0, -3f);
			timeStart = Time.time;
			currTime = Time.time;
			honked = false;
			duration = 8f;
			angry = false;
		}
	}

	private void FinishedCrossing () {
		ped_transform.position = new Vector3 (ped_transform.position.x, -3, ped_transform.position.z);
	}
}
