using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehavior : PlantTypes {
	[System.Serializable]
	public class SeedMesh {
		public GameObject kush;
		public GameObject coca;
		public GameObject poppy;
		public GameObject shroom;

		public void Hide() {
			kush.SetActive(false);
			coca.SetActive(false);
			poppy.SetActive(false);
			shroom.SetActive(false);
		}

		public void Display(PlantType type) {
			switch(type) {
			case PlantType.Coca:
				coca.SetActive(true);
				break;
			case PlantType.Kush:
				kush.SetActive(true);
				break;
			case PlantType.Poppy:
				poppy.SetActive(true);
				break;
			case PlantType.Psilocybin:
				shroom.SetActive(true);
				break;
			}
		}
	}

	public SeedMesh mesh = new SeedMesh();
	public PlantType seedType;

	private Project3GameManager manager;

	private Project3Player inven;
	//[HideInInspector]
	public GameObject player;
	//[HideInInspector]
	public bool playerSpawned = false;

	void Start() {
		inven = GameObject.FindWithTag("Player").GetComponent<Project3Player>();
		manager = GameObject.FindWithTag("ManagerTag").GetComponent<Project3GameManager>();

		mesh.Hide();
		mesh.Display(seedType);

		if (player != null) {
			playerSpawned = true;
			Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
		}
	}

	void Update() {
		if (seedType == PlantType.None) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Player" && !playerSpawned) {
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
			Destroy(gameObject);
		}

		if (col.gameObject.tag == "Ground" && playerSpawned) {
			GameObject getPlant = manager.plants.GetPlantGameObject(seedType);
			if (getPlant != null) {
				GameObject newPlant = Instantiate(getPlant, transform.position, Quaternion.identity);
				newPlant.transform.parent = transform.parent.transform;
			}
			Destroy(gameObject);
		}
	}
}