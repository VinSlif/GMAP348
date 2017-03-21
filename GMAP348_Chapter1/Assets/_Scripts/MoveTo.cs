using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveTo : MonoBehaviour {
	
	public Transform goal;
	private Vector3 start;

	private NavMeshAgent agent;

	void Start() {
		start = transform.position;

		agent = GetComponent<NavMeshAgent>();
		agent.destination = goal.position;
	}

	void Update() {
		if (Vector3.Distance(transform.position, goal.position) <= 5.0f) {
			agent.destination = start;
		} else if (Vector3.Distance(transform.position, start) <= 5.0f) {
			agent.destination = goal.position;
		}
	}
}