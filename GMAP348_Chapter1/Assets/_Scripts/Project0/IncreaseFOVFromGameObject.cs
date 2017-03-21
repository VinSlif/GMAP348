using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFOVFromGameObject : MonoBehaviour {

	public GameObject target;
	public float maxFOV = 130.0f;

	// Update is called once per frame
	void Update () {
		float newFOV = (target.transform.position - transform.position).sqrMagnitude;

		Camera.main.fieldOfView = (newFOV > maxFOV) ? maxFOV : newFOV;
	}
}