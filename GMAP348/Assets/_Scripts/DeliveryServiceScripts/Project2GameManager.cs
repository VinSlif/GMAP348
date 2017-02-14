using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Project2GameManager : MonoBehaviour {

	// Car
	[Serializable]
	public class CarSetUp {
		[Tooltip("How many cars are going to spawn")]
		public int toSpawn = 2;
		[Tooltip("The Car prefab")]
		public GameObject prefab;
		[Tooltip("GameObject to organize generated Car(s)")]
		public GameObject holder;
		[Tooltip("The Delivery Car tag")]
		public string tag = "DeliveryCar";

		public void Spawn(int needed, GameObject[] pos) {
			for (int i = 0; i < needed; i++) {
				GameObject newCar = Instantiate(prefab,
					                    pos[(int)UnityEngine.Random.Range(0, pos.Length)].transform.position,
					                    Quaternion.identity);
				newCar.name = "car " + i + " (clone)";
				newCar.GetComponent<CarBehavior>().index = i;
				newCar.GetComponent<CarBehavior>().color = new Color(((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
					((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
					((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
					1);
				newCar.transform.parent = holder.transform;
			}
		}
	}

	// PickUp
	[Serializable]
	public class PickUpSetUp {
		[Tooltip("The Pick Up prefab")]
		public GameObject prefab;
		[Tooltip("The location to spawn the pick up")]
		public Transform pickPoint;
		[Tooltip("GameObject to organize generated Pick Ups")]
		public GameObject holder;

		public void Spawn(int index) {
			GameObject pickUp = Instantiate(prefab,
				                    pickPoint.position,
				                    pickPoint.localRotation);
			pickUp.name = "PickUp " + index + " (clone)";
			pickUp.GetComponent<PickUpBehavior>().index = index;
			pickUp.transform.parent = holder.transform;
		}
	}

	// Delivery
	[Serializable]
	public class DeliverySetUp {
		[Tooltip("The Delivery Point tag")]
		public string pointTag = "DeliveryPoint";

		[HideInInspector]
		public GameObject[] go;
		[HideInInspector]
		public float totalNeeded;
		[HideInInspector]
		public List<int> indexes;

		public void GetGameObjects(string tag) {
			go = GameObject.FindGameObjectsWithTag(tag);
		}

		public void ClearPoints() {
			for (int i = 0; i < go.Length; i++) {
				go[i].SetActive(false);
			}
		}

		public Vector3 SelectPoint() {
			int index = (int)UnityEngine.Random.Range(0, go.Length);

			if (indexes.Count > 0) {
				while (indexes.Contains(index)) {
					index = (int)UnityEngine.Random.Range(0, go.Length);
				}
			}

			if (indexes.Count >= totalNeeded) {
				indexes.RemoveAt(0);
			}

			indexes.Add(index);
			go[index].SetActive(true);
			return go[index].transform.position;
		}

		public Vector3 randomPoint() {
			int index = (int)UnityEngine.Random.Range(0, go.Length);
			return go[index].transform.position;
		}

		public int interpretPoint(Vector3 pos) {
			int index = -1;
			for (int i = 0; i < go.Length; i++) {
				if (go[i].transform.position == pos) {
					index = i;
					break;
				}
			}
			return index;
		}
	}

	// Pedestrians
	[Serializable]
	public class PedestrianSetUp {
		[Tooltip("How many Pedestrian Leaders are going to spawn")]
		public int toSpawn = 10;
		[Tooltip("Pedestrian Leader prefab")]
		public GameObject prefab;
		[Tooltip("GameObject to organize generated Pedestrian Leaders")]
		public GameObject holder;
		[Tooltip("The Pedestrian tag")]
		public string tag = "Pedestrian";
		[Tooltip("The Pedestrin Location Point tag")]
		public string pointTag = "PedestrianPoint";
		[Tooltip("The Pedestrin Visit Point tag")]
		public string visitTag = "VisitPoint";

		[HideInInspector]
		public List<GameObject> pedestrians;
		[HideInInspector]
		public Transform[] loc;
		[HideInInspector]
		public Transform[] visitLoc;

		public void GetLocatorPoints(string tag) {
			GameObject[] locObject = GameObject.FindGameObjectsWithTag(tag).OrderBy(go => go.name).ToArray();

			loc = new Transform[locObject.Length];
			for (int i = 0; i < loc.Length; i++) {
				loc[i] = locObject[i].transform;
			}
		}

		public void GetVisitPoints(string tag) {
			GameObject[] locObject = GameObject.FindGameObjectsWithTag(tag).OrderBy(go => go.name).ToArray();

			visitLoc = new Transform[locObject.Length];
			for (int i = 0; i < visitLoc.Length; i++) {
				visitLoc[i] = locObject[i].transform;
			}
		}

		public void Spawn(int needed) {
			for (int i = 0; i < needed; i++) {
				pedestrians.Add(new GameObject());
				pedestrians[i].name = "PedestrianGroup " + i;
				pedestrians[i].transform.parent = holder.transform;

				GameObject newPedLeader = Instantiate(prefab,
					                          loc[(int)UnityEngine.Random.Range(0, loc.Length)].position,
					                          loc[(int)UnityEngine.Random.Range(0, loc.Length)].localRotation);
				newPedLeader.GetComponent<PedestrianLeaderBehavior>().color = new Color(((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
					((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
					((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
					1);
				newPedLeader.GetComponent<PedestrianLeaderBehavior>().index = i;
				newPedLeader.name = "PedestrianLeader (clone)";
				newPedLeader.transform.parent = pedestrians[i].transform;
			}
		}

		public Vector3 SelectPoint() {
			return loc[(int)UnityEngine.Random.Range(0, loc.Length)].position;
		}

		public Transform GetClosestDestination(Vector3 pos, Transform[] locPoints) {
			Transform selected = null;
			float closestDist = Mathf.Infinity;

			foreach(Transform t in locPoints) {
				float checkDist = Vector3.Distance(pos, t.position);
				if (checkDist < closestDist) {
					selected = t;
					closestDist = checkDist;
				}
			}
			return selected;
		}

		public void CheckPedestrians(int count) {
			for (int i = 0; i < count; i++) {
				if (pedestrians[i] == null) {
					pedestrians[i] = (new GameObject());
					pedestrians[i].name = "PedestrianGroup " + i;
					pedestrians[i].transform.parent = holder.transform;

					GameObject newPedLeader = Instantiate(prefab,
						                          loc[(int)UnityEngine.Random.Range(0, loc.Length)].position,
						                          loc[(int)UnityEngine.Random.Range(0, loc.Length)].localRotation);
					newPedLeader.GetComponent<PedestrianLeaderBehavior>().color = new Color(((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
						((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
						((int)UnityEngine.Random.Range(0, 256)) / 255.0f,
						1);
					newPedLeader.GetComponent<PedestrianLeaderBehavior>().index = i;
					newPedLeader.name = "PedestrianLeader (clone)";
					newPedLeader.transform.parent = pedestrians[i].transform;
				}
			}
		}
	}

	// Cross Walk
	[Serializable]
	public class CrossWalkSetUp {
		[Tooltip("Cross Walk prefab")]
		public GameObject prefab;
		[Tooltip("GameObject to organize generated Cross Walks")]
		public GameObject holder;
		[Tooltip("The Cross Walk Point tag")]
		public string pointTag = "CrossWalkPoint";

		[HideInInspector]
		public Transform[] loc;

		public void GetLocatorPoints(string tag) {
			GameObject[] locObject = GameObject.FindGameObjectsWithTag(tag).OrderBy(go => go.name).ToArray();

			loc = new Transform[locObject.Length];
			for (int i = 0; i < loc.Length; i++) {
				loc[i] = locObject[i].transform;
			}
		}

		public void Place(int needed) {
			for (int i = 0; i < needed; i++) {
				GameObject newCross = Instantiate(prefab,
					                      loc[i].position,
					                      loc[i].localRotation);
				newCross.name = "CrossWalk " + i + " (clone)";
				newCross.transform.parent = holder.transform;
			}
		}
	}

	// Cop
	[Serializable]
	public class CopSetUp {
		[Tooltip("The Cop Car prefab")]
		public GameObject prefab;
		[Tooltip("The Cop Car spawn point")]
		public Transform spawnPoint;
		[Tooltip("GameObject to organize generated Cops")]
		public GameObject holder;

		public GameObject Spawn(GameObject chaseCar) {
			GameObject spawnCar = Instantiate(prefab,
				                      spawnPoint.position,
				                      spawnPoint.localRotation);
			spawnCar.GetComponent<CopBehavior>().chasingCar = chaseCar;
			spawnCar.transform.parent = holder.transform;

			return spawnCar;
		}
	}


	public static float delivered = 0;

	// Cars
	public CarSetUp car = new CarSetUp();
	// Pick Up
	public PickUpSetUp pickUp = new PickUpSetUp();
	// Delivery
	public DeliverySetUp del = new DeliverySetUp();
	// Pedestrians
	public PedestrianSetUp ped = new PedestrianSetUp();
	// Cross Walk
	public CrossWalkSetUp cross = new CrossWalkSetUp();
	// Cop
	public CopSetUp cop = new CopSetUp();

	private RaycastHit carHit;


	// Use this for initialization
	void Start() {
		// Get locator points
		ped.GetLocatorPoints(ped.pointTag);
		ped.GetVisitPoints(ped.visitTag);
		cross.GetLocatorPoints(cross.pointTag);

		// Initialize delivery points
		del.GetGameObjects(del.pointTag);
		del.totalNeeded = car.toSpawn;
		del.ClearPoints();

		// Spawn characters/Generate objects
		ped.Spawn(ped.toSpawn);
		car.Spawn(car.toSpawn, del.go);
		cross.Place(cross.loc.Length);
	}
	
	// Update is called once per frame
	void Update() {

		// check if all pedestrian leaders still exist
		ped.CheckPedestrians(ped.toSpawn);

		// handle police chase situation
		if (Input.GetMouseButtonDown(0)) {
			Ray tempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(tempRay, out carHit, 100.0f)) {
				if (carHit.transform.gameObject.GetComponent<CarBehavior>() != null) {
					//Debug.DrawLine(tempRay.origin, carHit.point, Color.cyan);
					if (carHit.collider.gameObject.GetComponent<CarBehavior>().didCrime
					    && carHit.collider.gameObject.GetComponent<CarBehavior>().arrestingOfficer == null) {
						carHit.transform.gameObject.GetComponent<CarBehavior>().arrestingOfficer = cop.Spawn(carHit.transform.gameObject);
					}
				}
			}
		}

		// Quit game
		if (Input.GetKey(KeyCode.Space)) {
			//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			Application.Quit();
		}
	}
}