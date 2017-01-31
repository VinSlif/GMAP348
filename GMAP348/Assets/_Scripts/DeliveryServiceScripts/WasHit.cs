using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasHit : MonoBehaviour {

	public bool wasHit = false;

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Player") {
			wasHit = true;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			wasHit = true;
		}
	}
}