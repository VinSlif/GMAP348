using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour {

	public Vector3 direction = new Vector3(0, 0, 0);
	public float speed = 5.0f;
	
	// Update is called once per frame
	void Update () {
		transform.Translate(direction.x * speed * Time.deltaTime,
			direction.y * speed * Time.deltaTime,
			direction.z * speed * Time.deltaTime);
	}
}
