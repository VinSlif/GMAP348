using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehavior : PlantTypes {

	public PlantType seedType;
	private Project3Player inven;

	void Awake() {
		inven = GameObject.FindWithTag("Player").GetComponent<Project3Player>();
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Player") {
			Destroy(gameObject);
		}
	}

	public void OnDestroy() {
		switch(seedType) {
		case PlantType.Coca:
			inven.inventory.coca++;
			break;
		case PlantType.Kush:
			inven.inventory.kush++;
			break;
		case PlantType.Poppy:
			inven.inventory.poppy++;
			break;
		case PlantType.Psilocybin:
			inven.inventory.shrooms++;
			break;
		}
	}
}