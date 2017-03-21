using UnityEngine;

//[RequireComponent(typeof(ParticleSystem))]
public class WaterParticleCollision : MonoBehaviour {

	private ParticleSystem sys;

	void Start() {
		sys = GetComponent<ParticleSystem>();
	}

	void Update() {
		if (Input.GetButton("Fire1")) {
			sys.Play(true);
		} else {
			//sys.Pause(true);
            sys.Stop();
		}
	}

	void OnParticleCollision(GameObject col) {
		if (col.GetComponent<PlantBehavior>() != null) {
			col.GetComponent<PlantBehavior>().watered = true;
		}
	}
}