using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountPedsInTrigger : MonoBehaviour {

	private Project2GameManager gameManager;

	private CrossWalkBehavior cross;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("ManagerTag").GetComponent<Project2GameManager> ();
		cross = transform.parent.GetComponent<CrossWalkBehavior> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == gameManager.ped.tag) {
			cross.pedsInTriggers++;
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == gameManager.ped.tag) {
			cross.pedsInTriggers--;
		}
	}
}