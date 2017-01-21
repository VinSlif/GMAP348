﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameObject : MonoBehaviour {

	[Tooltip("Rotation speed")]
	public float speed = 10.0f;

	void Update() {
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}