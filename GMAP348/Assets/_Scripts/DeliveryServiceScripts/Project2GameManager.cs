using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Project2GameManager : MonoBehaviour {

	public List<Vector3> path;
	public List<Vector3> carWaypoints;

	public GameObject pickUpPrefab;

	public Text deliverTime;
	private float deliveryTimer = 0;
	public float deliveryTime = 10.0f;

	public GameObject gameOverUI;

	public Transform delPickPoint;
	public static float delivered = -1;
	public static bool isDelivering = false;

	private bool isPickUpSpawned = false;
	private bool isDeliverPicked = false;

	private GameObject[] delLoc;


	public GameObject pedHolder;
	public GameObject pedPrefab;
	private GameObject[] pedestrian;
	private bool[] pedBlocked;
	private bool[] pedBlockedSpawned;

	public GameObject streetBlockHolder;
	public GameObject streetBlock;

	public int pedSpawn = 1;

	private Transform[] pedLoc;
	private Color[] pedColor;

	// Use this for initialization
	void Start() {
		gameOverUI.SetActive(false);

		delLoc = GameObject.FindGameObjectsWithTag("DeliveryPoint");

		CreateCarWaypoints ();

		GameObject[] pedLocObject = GameObject.FindGameObjectsWithTag("PedestrianPoint").OrderBy(go => go.name).ToArray();
		pedLoc = new Transform[pedLocObject.Length];
		for (int i = 0; i < pedLocObject.Length; i++) {
			pedLoc[i] = pedLocObject[i].transform;
		}
		GetPedestrianColor(pedLoc.Length);
		SpawnPedestrians(pedLoc.Length);

		pedBlocked = new bool[pedLoc.Length];
		GetBlockedStreets();
		pedBlockedSpawned = new bool[pedLoc.Length];
		for (int i = 0; i < pedBlocked.Length; i++) {
			pedBlockedSpawned[i] = false;
		}
		ClearDeliverPoints();

		deliveryTimer = deliveryTime;
	}
	
	// Update is called once per frame
	void Update() {
		if (deliveryTimer > 0) {

			deliveryTimer -= Time.deltaTime;
			deliverTime.text = (Mathf.Round(deliveryTimer * 100.0f) / 100.0f).ToString();

			if (!isDelivering) {
				ClearDeliverPoints();
				if (!isPickUpSpawned) {
					GetBlockedStreets();
					GetSpawnedBlockedStreets();
					SetPedestrians(pedSpawn);
					Instantiate(pickUpPrefab, delPickPoint.position, delPickPoint.localRotation);
					isDeliverPicked = false;
					isPickUpSpawned = true;
				}
			} else {
				if (!isDeliverPicked) {
					PickDeliveryPoints((int)Random.Range(1, delivered + 2));
					GetBlockedStreets();
					GetSpawnedBlockedStreets();
					SetPedestrians(pedSpawn);
					isDeliverPicked = true;
					isPickUpSpawned = false;

					delivered++;
					deliveryTimer = deliveryTime;
				}
			}
		} else {
			gameOverUI.SetActive(true);

			if (Input.GetKey(KeyCode.Space)) {
				//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				Application.Quit();
			}
		}

		// Debug
		if (Input.GetKeyDown(KeyCode.F1)) {
			for (int i = 0; i < 40; i++) {
				pedestrian[i].GetComponent<Pedestrians>().Crossing();
			}
		}
	}

	void SpawnPedestrians(int needed) {
		pedestrian = new GameObject[needed];
		for (int i = 0; i < needed; i++) {
			pedestrian[i] = Instantiate(pedPrefab, pedLoc[i].position, pedLoc[i].localRotation);
			//pedestrian[i].SetActive(false);
			pedestrian[i].GetComponent<Pedestrians>().pedColor = pedColor[i];
			pedestrian[i].GetComponent<Pedestrians>().pedIndex = i;
			pedestrian[i].name = "pedestrian " + i + " (clone)";
			pedestrian[i].transform.parent = pedHolder.transform;
		}
	}

	void GetPedestrianColor(int needed) {
		pedColor = new Color[needed];
		for (int i = 0; i < needed; i++) {
			pedColor[i] = new Color(((int)Random.Range(0, 255)) / 255.0f,
				((int)Random.Range(0, 255)) / 255.0f,
				((int)Random.Range(0, 255)) / 255.0f,
				1);
		}
	}

	void ClearDeliverPoints() {
		for (int i = 0; i < delLoc.Length; i++) {
			delLoc[i].SetActive(false);
		}
	}

	void PickDeliveryPoints(int needed) {
		for (int i = 0; i < needed; i++) {
			delLoc[Random.Range(0, delLoc.Length)].SetActive(true);
		}
	}

	void SetPedestrians(int needed) {
		for (int i = 0; i < pedBlocked.Length; i++) {
			if (pedBlocked[i]) {
				BlockStreet(i);
			}
		}

		for (int i = 0; i < needed; i++) {
			int randomPedestrian;
			do {
				randomPedestrian = Random.Range(0, pedLoc.Length);
			} while (pedestrian[randomPedestrian].GetComponent<Pedestrians>().crossingBool
			         && !pedestrian[randomPedestrian].GetComponent<Pedestrians>().wasHit);

			pedestrian[randomPedestrian].GetComponent<Pedestrians>().Crossing();
		}
	}

	public void ChangePedLocation(int pedIndex) {
		int newIndex = 0;
		if (!pedestrian[newIndex].GetComponent<Pedestrians>().wasHit) {
			do {
				newIndex = Random.Range(0, pedestrian.Length);
			} while (newIndex == pedIndex
			         && pedestrian[newIndex].GetComponent<Pedestrians>().crossingBool
			         && !pedestrian[newIndex].GetComponent<Pedestrians>().wasHit);
		}

		int temp = pedestrian[pedIndex].GetComponent<Pedestrians>().karmaCount;
		pedestrian[pedIndex].GetComponent<Pedestrians>().karmaCount = pedestrian[newIndex].GetComponent<Pedestrians>().karmaCount;
		pedestrian[newIndex].GetComponent<Pedestrians>().karmaCount = temp;
	}

	void BlockStreet(int index) {
		if (!pedBlockedSpawned[index]) {
			GameObject roadBlock = Instantiate(streetBlock, pedLoc[index].position, pedLoc[index].rotation);
			roadBlock.GetComponent<RoadBlockBehavior>().index = index;
			roadBlock.name = "RoadBlock " + index;
			roadBlock.transform.parent = streetBlockHolder.transform;
		}
	}

	void GetBlockedStreets() {
		for (int i = 0; i < pedLoc.Length; i++) {
			pedBlocked[i] = pedestrian[i].GetComponent<Pedestrians>().wasHit;
		}
	}

	void SetBlockedStreets() {
		for (int i = 0; i < pedLoc.Length; i++) {
			pedestrian[i].GetComponent<Pedestrians>().wasHit = pedBlocked[i];
		}
	}

	void GetSpawnedBlockedStreets() {
		if (streetBlockHolder.transform.childCount > 0) {
			int children = streetBlockHolder.transform.childCount;
			for (int i = 0; i < children; i++) {
				if (transform.GetChild(i).GetComponent<RoadBlockBehavior>() != null) {
					int index = transform.GetChild(i).GetComponent<RoadBlockBehavior>().index;
					pedBlockedSpawned[index] = true;
				}
			}
		}
	}

	public void CommunicateHitBool(int index, bool state) {
		pedBlocked[index] = state;
	}

	void CreateCarWaypoints() {
		for (int z = 40; z >= -40; z-=5) {
			for (int x = -40; x <= 40; x += 5) {
				if (z % 4 != 0) {
					if (x % 4 == 0) {
						carWaypoints.Add (new Vector3(x,0,z));
					}
				} else {
					carWaypoints.Add (new Vector3(x,0,z));
				}
			}
 		}
	}
}