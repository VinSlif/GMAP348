using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Project3Player : PlantTypes {

	[System.Serializable]
	public class UsableItems {
		public GameObject[] items;

		[HideInInspector]
		public int activeItem = 0;

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
			}

			ActivateItem(activeItem);
		}

		public void ActivateItem(int index) {
			// Deactivate all items
			for (int i = 0; i < items.Length; i++) {
				items[i].SetActive(false);
			}
			// Activate selected item
			items[index].SetActive(true);

		}
	}

	[System.Serializable]
	public class ItemActions {
		public GameObject seed;

		//[HideInInspector]
		public float plantingTime = 0.20f;
		//[HideInInspector]
		public float plantingTimer = 0;

		public Animator shovelAnim;
		[HideInInspector]
		public bool isSwinging = false;
	}

	[System.Serializable]
	public class Inventory {
		public int cash = 0;

		public int kush = 0;
		public int poppy = 0;
		public int coca = 0;
		public int shrooms = 0;

		public PlantType GetSeed() {
			PlantType getType;
			if (kush > 0) {
				kush--;
				getType = PlantType.Kush;
			} else if (poppy > 0) {
				poppy--;
				getType = PlantType.Poppy;
			} else if (coca > 0) {
				coca--;
				getType = PlantType.Coca;
			} else if (shrooms > 0) {
				shrooms--;
				getType = PlantType.Psilocybin;
			} else {
				getType = PlantType.None;
			}
			return getType;
		}
	}

	[System.Serializable]
	public class Health {
		public float hp = 100.0f;
		[HideInInspector]
		public float maxHealth;

		public Slider hpSlider;

		public void UpdateSlider() {
			hpSlider.value = hp / maxHealth;
		}
	}


	public UsableItems usable = new UsableItems();
	public ItemActions action = new ItemActions();
	public Health health = new Health();
	public Inventory inventory = new Inventory();

	private Project3GameManager manager;


	void Start() {
		manager = GameObject.FindWithTag("ManagerTag").GetComponent<Project3GameManager>();

		health.maxHealth = health.hp;
		health.UpdateSlider();
	}

	// Update is called once per frame
	void Update() {
		// Get item
		usable.SelectItem();
		health.UpdateSlider();

		action.plantingTimer -= Time.deltaTime;

		// Use item
		if (Input.GetButton("Fire1")) {
			switch(usable.activeItem) {
			// shovel action
			case 0:
				action.isSwinging = true;
				RaycastHit hit;
				if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4)) {
					if (hit.transform.tag == "Plant") {
						hit.transform.gameObject.GetComponent<PlantBehavior>().health -= 1.0f;
					}
				}

				break;
			// hose action
			case 1:
				break;
			// fertilizer action
			case 2:

				break;
			// seed action
			case 3:
				if (action.plantingTimer <= 0) {
					GameObject newSeed = Instantiate(action.seed,
						                     new Vector3(transform.position.x, 0.53f, transform.position.z + 1),
						                     Quaternion.identity);
					newSeed.GetComponent<SeedBehavior>().seedType = inventory.GetSeed();
					newSeed.GetComponent<SeedBehavior>().player = gameObject;
					newSeed.transform.parent = manager.plants.holder.transform;
					action.plantingTimer = action.plantingTime;
				}
				break;
			}
		}

		if (usable.items[0].activeSelf) {
			action.shovelAnim.SetBool("swing", action.isSwinging);
		}
		action.isSwinging = false;
	}
}