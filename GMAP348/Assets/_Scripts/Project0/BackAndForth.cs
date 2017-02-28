using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour {

	public Vector3 delta = new Vector3(0, 1.5f, 0);
	public float speed = 2.0f; 
	private Vector3 startPos;

	void Start () {
		startPos = transform.position;
	}

	void Update () {
		Vector3 v = startPos;
		v.x += delta.x * Mathf.Sin (Time.time * speed);
		v.y += delta.y * Mathf.Sin (Time.time * speed);
		v.z += delta.z * Mathf.Sin (Time.time * speed);
		transform.position = v;
	}
}