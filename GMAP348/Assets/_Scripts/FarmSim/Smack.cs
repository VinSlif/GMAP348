using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smack : MonoBehaviour {
    GameObject cam;
	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            Vector3 rayOrigin = transform.position;
            RaycastHit hit;

            if(Physics.Raycast(rayOrigin, transform.forward, out hit, 2))
            {
                Debug.Log(hit.collider.tag);
                Transform plant = hit.transform;
                while(plant.parent)
                {
                    plant = plant.parent.transform;
                }
                if (plant.tag == "Plant")
                    Destroy(plant.gameObject);
            }
        }
    }
}
