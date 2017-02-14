using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountPedsInTrigger : MonoBehaviour {

	private Project2GameManager gameManager;

	private CrossWalkBehavior cross;

	private bool setBlockState = false;
	public float waitTime = 10.0f;
	private float waitTimer = 0;

	// Use this for initialization
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project2GameManager>();
		cross = transform.parent.GetComponent<CrossWalkBehavior>();
	}

	void Update() {
		if (setBlockState) {
			waitTimer -= Time.deltaTime;

			if (waitTimer <= 0) {
				cross.currState = CrossWalkBehavior.State.Blocked;
				setBlockState = false;
			}
		} else {
			waitTimer = waitTime;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == gameManager.ped.tag) {
			cross.pedsInTrigger++;
		}

		if (col.gameObject.tag == gameManager.car.tag) {
			if (col.gameObject.GetComponent<CarBehavior>().didCrime
			    && col.gameObject.GetComponent<CarBehavior>().arrestingOfficer == null) {
				setBlockState = true;
			}
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.tag == gameManager.car.tag) {
			if (col.gameObject.GetComponent<CarBehavior>().didCrime
			    && col.gameObject.GetComponent<CarBehavior>().arrestingOfficer == null) {
				setBlockState = true;
			}
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == gameManager.ped.tag) {
			cross.pedsInTrigger--;
		}
	}
}