using UnityEngine;

public class DeductHealthOnCol : MonoBehaviour {
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Plant") {
			col.gameObject.GetComponent<PlantBehavior>().health -= 1.0f;
		}
	}
}