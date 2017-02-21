using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeed : MonoBehaviour {
    public GameObject plant;
    public GameObject player;
    public int cooldown;
    
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cooldown = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1") && cooldown == 0)
        {
            GameObject newSeed = Instantiate(plant, new Vector3(transform.position.x, .53f, transform.position.z + 1), Quaternion.identity);
            player.GetComponent<Inventory>().plantedSeed();
            cooldown = 20;
        }
        if(cooldown > 0)
            cooldown--;
    }
}
