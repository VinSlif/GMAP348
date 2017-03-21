using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Project3GameManager : PlantTypes {

	[System.Serializable]
	public class Plants {
		public GameObject kush;
		public GameObject coca;
		public GameObject poppy;
		public GameObject shroom;

		[Header("Use this to organize the scene")]
		public GameObject holder;

		public GameObject GetPlantGameObject(PlantType type) {
			GameObject plant = null;
			switch(type) {
			case PlantType.Coca:
				plant = coca;
				break;
			case PlantType.Kush:
				plant = kush;
				break;
			case PlantType.Poppy:
				plant = poppy;
				break;
			case PlantType.Psilocybin:
				plant = shroom;
				break;
			}
			return plant;
		}
	}

	[System.Serializable]
	public class Player {
		public GameObject go;
		public Text gameOverText;

		public void CheckHealth() {
			if (go.GetComponent<Project3Player>().health.hp <= 0) {
				gameOverText.gameObject.SetActive(true);
				Time.timeScale = 0;
			}
		}
	}

	[System.Serializable]
	public class InitSeeds {
		public GameObject seed;
		public int total = 20;
		public float squareBounds = 48.0f;
		public float startHeight = 20.0f;

		public void SpawnSeeds(GameObject player, Transform holder) {
			for (int i = 0; i < total; i++) {
				GameObject newSeed = Instantiate(seed,
					                     new Vector3(Random.Range(-squareBounds, squareBounds),
						                     startHeight,
						                     Random.Range(-squareBounds, squareBounds)),
					                     Quaternion.identity);
				newSeed.GetComponent<SeedBehavior>().seedType = RandomPlantType();
				newSeed.GetComponent<SeedBehavior>().player = player;
				newSeed.transform.parent = holder;
			}
		}

		public PlantType RandomPlantType() {
			float pick = Random.Range(0, 100);
			if (0 <= pick && pick <= 25) {
				return PlantType.Coca;
			} else if (0 <= pick && pick <= 25) {
				return PlantType.Kush;
			} else if (0 <= pick && pick <= 25) {
				return PlantType.Poppy;
			} else {
				return PlantType.Psilocybin;
			}
		}
	}

	public Player player = new Player();
	public Plants plants = new Plants();
	public InitSeeds placeSeeds = new InitSeeds();

	// Use this for initialization
	void Start() {
		placeSeeds.SpawnSeeds(player.go, plants.holder.transform);
		player.gameOverText.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update() {
		player.CheckHealth();
	}
}
