using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerController : MonoBehaviour {

	public float speed = 5.0f;

	public static bool isHonkong = false;

	void Start() {
		
	}

	void Update() {
		transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime,
			0,
			Input.GetAxis("Vertical") * speed * Time.deltaTime
		);

		if (Input.GetKey(KeyCode.Space)) {
			isHonkong = true;
		} else {
			isHonkong = false;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Pick Up") {
			Destroy(col.gameObject);
		}
	}
}