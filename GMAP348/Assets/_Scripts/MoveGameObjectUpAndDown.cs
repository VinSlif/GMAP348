using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGameObjectUpAndDown : MonoBehaviour {

	[Tooltip("Amount (in meters) going up and down")]
	public float maxUpAndDown = 1.0f;
	[Tooltip("Up/Down speed")]
	public float speed = 50.0f;

	// angle to determin the height by using the sinus
	private float angle = -90.0f;
	// height of the object when the script starts
	private Vector3 startPos;

	// radians to degrees
	private const float radToDegree = Mathf.PI / 180;

	void Start() {
		startPos = transform.localPosition;
	}

	void Update() {
		angle += speed * Time.deltaTime;

		if (angle > 270) {
			angle -= 360;
		}

		transform.localPosition = new Vector3(startPos.x,
			startPos.y + maxUpAndDown * (1 + Mathf.Sin(angle * radToDegree)) / 2,
			startPos.z);
	}
}