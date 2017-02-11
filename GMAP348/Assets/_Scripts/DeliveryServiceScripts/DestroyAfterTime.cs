using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

	public float t;

	void Start() {
		Destroy (this.gameObject, t);
	}
}