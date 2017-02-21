using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public GameObject[] inventory;

    private int activeItem;
    public int seedsHeld;
	// Use this for initialization
	void Start () {
        inventory = GameObject.FindGameObjectsWithTag("Inventory");
        seedsHeld = 0;
        for (int i=0; i<inventory.Length; i++)
        {
            if (!inventory[i].name.Equals("Shovel"))
                inventory[i].SetActive(false);
            else
                activeItem = i;
        }
	}
	
	// Update is called once per frame
	void Update () {
        float scrolling = Input.GetAxis("Mouse ScrollWheel");
        if (scrolling > 0)
        {
            inventory[activeItem].SetActive(false);
            if (activeItem == inventory.Length - 1)
                activeItem = 0;
            else
                activeItem++;
            if(!inventory[activeItem].name.Equals("Seed") || seedsHeld > 0)
                inventory[activeItem].SetActive(true);
        }
        else if (scrolling < 0)
        {
            inventory[activeItem].SetActive(false);
            if (activeItem == 0)
                activeItem = inventory.Length - 1;
            else
                activeItem--;
            if (!inventory[activeItem].name.Equals("Seed") || seedsHeld > 0)
                inventory[activeItem].SetActive(true);
        }
    }

    public void plantedSeed()
    {
        seedsHeld--;
        if (inventory[activeItem].name.Equals("Seed") && seedsHeld < 1)
            inventory[activeItem].SetActive(false);
    }
}
