using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrians : MonoBehaviour {

	public int karmaCount = 0;
	public int pedIndex = 0;

	public SpriteRenderer karmaDis;
	public Sprite[] karmaSprites;

	public Color pedColor;

	private GameObject peopleObj;
	private Transform ped_transform;

	private float yInactive = -10f;
	private float yActive = 0f;

	private float timeStart;
	private float currTime;
	private float duration = 0f;
	private float fullDuration = 10f;
	private float partDuration = 2f;
	private float percentageDone;
	private float currWalkPos;

	private const int LOWKARMA = -2;
	private const int HIGHKARMA = 2;

	private float carEnterTime;
	private float carStayTime;
	private bool honked = false;
	private bool angry = false;

	public bool crossingBool = false;
	private Project1GameManager gameManager;

	public GameObject blocker;
	public GameObject streetBlock;

	public bool wasHit = false;

	private float ragdollTimer = 0;
	public float ragdollTime = 5.0f;

	private Vector3[] resetPos = new Vector3[3];
	private Quaternion[] resetRot = new Quaternion[3];

	// Use this for initialization
	void Start() {
		gameManager = GameObject.FindGameObjectWithTag("ManagerTag").GetComponent<Project1GameManager>();
		peopleObj = this.gameObject.transform.GetChild(1).gameObject;
		ped_transform = this.gameObject.transform;
		ped_transform.position = new Vector3(ped_transform.position.x, yInactive, ped_transform.position.z);
		ChangeMaterial();
		//IgnoreCollisionWithPlayer();
		GetPedestrianOriginalPos();
	}
	
	// Update is called once per frame
	void Update() {
		wasHit = blocker.GetComponent<WasHit>().wasHit;
		if (!wasHit) {

			ragdollTimer = ragdollTime;

			currTime += Time.deltaTime;
			percentageDone = (currTime - timeStart) / duration;
			currWalkPos = Mathf.Lerp(-3f, 3f, percentageDone);

			if (!angry) {
				peopleObj.transform.localPosition = new Vector3(0, 0, currWalkPos);
			} else {
				peopleObj.transform.localPosition = new Vector3(0, 0, 0);
			}

			if (percentageDone >= 1f && crossingBool) {
				FinishedCrossing();
				crossingBool = false;
			}

		} else {
			ragdollTimer -= Time.deltaTime;

			karmaCount = HIGHKARMA;

			if (ragdollTimer <= 0) {
				ragdollTimer = ragdollTime;
				UnRagdollPedestrians();
				FinishedCrossing();
				crossingBool = false;
			} else {
				RagdollPedestrians();
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			if (karmaCount == HIGHKARMA) {
				duration = partDuration;
			}
			carEnterTime = Time.time;
			carStayTime = Time.time;
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.tag == "Player" && !honked) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				honked = true;
				karmaCount--;
				if (karmaCount < LOWKARMA) {
					karmaCount = LOWKARMA;
					angry = true;
				} else if (karmaCount > LOWKARMA) {
					duration = partDuration;
				}
				karmaDis.sprite = karmaSprites[karmaCount + 2];
			} else {
				carStayTime += Time.deltaTime;
				if ((carStayTime - carEnterTime) >= 3f) {
					karmaCount++;
					duration = partDuration;
					if (karmaCount > HIGHKARMA) {
						karmaCount = HIGHKARMA;
					}
					karmaDis.sprite = karmaSprites[karmaCount + 2];
				}
			}
		} 
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "Player" && angry) {
			angry = false;
			duration = fullDuration;
			timeStart = Time.time - (fullDuration / 2);
			currTime = Time.time;
		}
	}

	public void Crossing() {
		karmaDis.sprite = karmaSprites[karmaCount + 2];
		crossingBool = true;
		ped_transform.position = new Vector3(ped_transform.position.x, yActive, ped_transform.position.z);
		peopleObj.transform.localPosition = new Vector3(0, 0, -3f);
		timeStart = Time.time;
		currTime = Time.time;
		honked = false;
		duration = fullDuration;
		angry = false;
	}

	private void FinishedCrossing() {
		ped_transform.position = new Vector3(ped_transform.position.x, yInactive, ped_transform.position.z);

		ResetPedestrianPos();

		gameManager.ChangePedLocation(pedIndex);
	}

	private void ChangeMaterial() {
		MeshRenderer[] allchildren = peopleObj.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer child in allchildren) {
			child.gameObject.GetComponent<MeshRenderer>().material.color = pedColor;
		}
	}

	void IgnoreCollisionWithPlayer() {
		Rigidbody[] allchildren = peopleObj.GetComponentsInChildren<Rigidbody>();
		foreach(Rigidbody child in allchildren) {
			Physics.IgnoreCollision(child.gameObject.GetComponent<Collider>(),
				GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
		}
	}

	void GetPedestrianOriginalPos() {
		Rigidbody[] allchildren = peopleObj.GetComponentsInChildren<Rigidbody>();
		int i = 0;
		foreach(Rigidbody child in allchildren) {
			resetPos[i] = child.gameObject.GetComponent<Transform>().localPosition;
			resetRot[i] = child.gameObject.GetComponent<Transform>().localRotation;
			i++;
		}
	}

	void ResetPedestrianPos() {
		Rigidbody[] allchildren = peopleObj.GetComponentsInChildren<Rigidbody>();
		int i = 0;
		foreach(Rigidbody child in allchildren) {
			child.gameObject.transform.localPosition = resetPos[i];
			child.gameObject.transform.localRotation = resetRot[i];
			i++;
		}
	}

	void UnRagdollPedestrians() {
		Rigidbody[] allchildren = peopleObj.GetComponentsInChildren<Rigidbody>();
		foreach(Rigidbody child in allchildren) {
			child.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	void RagdollPedestrians() {
		Rigidbody[] allchildren = peopleObj.GetComponentsInChildren<Rigidbody>();
		foreach(Rigidbody child in allchildren) {
			child.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		}
	}
}