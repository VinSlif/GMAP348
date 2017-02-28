using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehavior : PlantTypes {

	public PlantType seedType;
	private Project3Player inven;
    private GameObject player;

	void Awake() {
		inven = GameObject.FindWithTag("Player").GetComponent<Project3Player>();
        player = GameObject.FindWithTag("Player");

    }

	void OnCollisionEnter(Collision col) {
        /*if (col.gameObject.tag == "Player") {
			Destroy(gameObject);
		}*/
        if (col.gameObject.tag == "Ground")
        {
            Invoke("invokeDestroy", 2);
        }
    }

    void invokeDestroy()
    {
        Destroy(gameObject);
    }

	public void OnDestroy() {
        GameObject newPlant = new GameObject();
        switch (seedType)
        {
            case PlantType.Coca:
                //inven.inventory.coca++;
                newPlant = Instantiate(inven.prefab.cocaPlant,
                                              new Vector3(transform.position.x, 0, transform.position.z),
                                              Quaternion.identity);
                //newPlant.transform.parent = holder;
                break;
            case PlantType.Kush:
                //inven.inventory.kush++;
                newPlant = Instantiate(inven.prefab.kushPlant,
                                              new Vector3(transform.position.x, 0, transform.position.z),
                                              Quaternion.identity);
                break;
            case PlantType.Poppy:
                //inven.inventory.poppy++;
                newPlant = Instantiate(inven.prefab.poppyPlant,
                                              new Vector3(transform.position.x, 0, transform.position.z),
                                              Quaternion.identity);
                break;
            case PlantType.Psilocybin:
                //inven.inventory.shrooms++;
                newPlant = Instantiate(inven.prefab.shroomPlant,
                                              new Vector3(transform.position.x, 0, transform.position.z),
                                              Quaternion.identity);
                break;
        }
	}
}