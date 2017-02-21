using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project3Player : PlantTypes {

	[System.Serializable]
	public class UsableItems {

		public GameObject[] items;

		//[HideInInspector]
		public int activeItem = 0;

		//[HideInInspector]
		public float plantingTime = 0.20f;
		//[HideInInspector]
		public float plantingTimer = 0;

		public void SelectItem() {
			float scrolling = Input.GetAxis("Mouse ScrollWheel");
			if (scrolling > 0) {
				if (activeItem > items.Length - 1) {
					activeItem = 0;
				} else {
					activeItem++;
				}

			} else if (scrolling < 0) {
				if (activeItem < 0) {
					activeItem = items.Length - 1;
				} else {
					activeItem--;
				}
			}

			if (Input.GetKey(KeyCode.Alpha1)) {
				activeItem = 0;
			} else if (Input.GetKey(KeyCode.Alpha2)) {
				activeItem = 1;
			} else if (Input.GetKey(KeyCode.Alpha3)) {
				activeItem = 2;
			} else if (Input.GetKey(KeyCode.Alpha4)) {
				activeItem = 3;
			} else if (Input.GetKey(KeyCode.Alpha5)) {
				activeItem = 4;
			} else if (Input.GetKey(KeyCode.Alpha6)) {
				activeItem = 5;
			}

			ActivateItem(activeItem);
		}

		public void ActivateItem(int index) {
			for (int i = 0; i < items.Length; i++) {
				items[i].SetActive(false);
			}
			items[index].SetActive(true);
		}

		public void ShovelAction(GameObject go) {
			Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.red);

			RaycastHit hit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2)) {
				if (hit.transform.tag == "Plant") {
					if (!hit.transform.gameObject.GetComponent<PlantBehavior>().harvesting) {
						hit.transform.gameObject.GetComponent<PlantBehavior>().harvesting = true;
					}
				}
			}
		}

		public void SeedAction(GameObject plant, Transform pos, Transform holder) {
			plantingTimer -= Time.deltaTime;

			if (plantingTimer <= 0) {
				GameObject newPlant = Instantiate(plant,
					                      new Vector3(pos.position.x, .53f, pos.position.z + 1),
					                      Quaternion.identity);
				newPlant.transform.parent = holder;
				plantingTimer = plantingTime;
			}
		}
	}

	[System.Serializable]
	public class Inventory {
		public int cash = 0;

		public int kush = 0;
		public int poppy = 0;
		public int coca = 0;
		public int shrooms = 0;
	}

	[System.Serializable]
	public class Prefabs {
		public GameObject kushPlant;
		public GameObject poppyPlant;
		public GameObject cocaPlant;
		public GameObject shroomPlant;
	}

	[System.Serializable]
	public class Holders {
		public GameObject plants;
		public GameObject enemies;
	}


	public Prefabs prefab = new Prefabs();
	public Holders holder = new Holders();
	public UsableItems usable = new UsableItems();
	public Inventory inventory = new Inventory();

	// Use this for initialization
	void Start() {
		usable.ActivateItem(usable.activeItem);
	}
	
	// Update is called once per frame
	void Update() {
		// Get item
		usable.SelectItem();

		// Use item
		if (Input.GetButton("Fire1")) {
			switch(usable.activeItem) {
			case 0:
				usable.ShovelAction(usable.items[0]);
				break;
			case 1:
				// hose action
				break;
			case 2:
				usable.SeedAction(prefab.kushPlant, transform, holder.plants.transform);
				break;
			case 3:
				usable.SeedAction(prefab.cocaPlant, transform, holder.plants.transform);
				break;
			case 4:
				usable.SeedAction(prefab.poppyPlant, transform, holder.plants.transform);
				break;
			case 5:
				usable.SeedAction(prefab.shroomPlant, transform, holder.plants.transform);
				break;
			}
		}
	}
}