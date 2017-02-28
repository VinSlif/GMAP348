using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnCollision : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}