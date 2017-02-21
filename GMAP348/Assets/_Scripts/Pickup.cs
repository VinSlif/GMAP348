using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        Invoke("setTrigger", 2.5f);
	}
	
	// Update is called once per frame
	void Update () {
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<Inventory>().seedsHeld++;
            Destroy(gameObject);
        }
        
    }

    private void setTrigger()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Collider>().isTrigger = true;
    }
}
