using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoDeliveryPoint : MonoBehaviour {

	public float deliveryPointTime = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		deliveryPointTime += Time.deltaTime;
	}

}
