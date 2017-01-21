using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGameObjectUpAndDown : MonoBehaviour {

	[Tooltip("Amount (in meters) going up and down")]
	public float maxUpAndDown  = 1;
	[Tooltip("Up/Down speed")]
	public float speed = 50;

	private float angle = -90;					// angle to determin the height by using the sinus
	private float startHeight;					// height of the object when the script starts

	// radians to degrees
	private const float radToDegree = Mathf.PI/180;

	void Start() {
		startHeight = transform.localPosition.y;
	}

	void Update() {
		angle += speed * Time.deltaTime;
		if (angle > 270) angle -= 360;

		transform.localPosition = new Vector3(0,
			startHeight + maxUpAndDown * (1 + Mathf.Sin(angle * radToDegree)) / 2,
			0);
	}
}