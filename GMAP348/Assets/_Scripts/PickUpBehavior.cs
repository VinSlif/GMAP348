using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehavior : MonoBehaviour {


	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			if (!Project1GameManager.isDelivering) {
				Project1GameManager.isDelivering = true;
				transform.parent = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
			}
		}

		if (col.gameObject.tag == "DeliveryPoint") {
			Project1GameManager.isDelivering = false;
			col.gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}