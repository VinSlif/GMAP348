using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : StateMachineBehaviour {

	[Header("Location Information")]
	public string locationName;

	[Tooltip("General description of the location\nused everytime the location is visited")]
	[TextArea(5, 20)]
	public string description;

	[Header("Location Content")]
	[Tooltip("Must be the exact spelling of an item")]
	public List<string> items;

	[Tooltip("Must be the exact spelling of a person")]
	public List<string> people;

	[HideInInspector]
	public bool isEnabled = false;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateEnter(animator, stateInfo, layerIndex);
		isEnabled = true;
		animator.SetInteger("Direction", 0);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		isEnabled = false;
		base.OnStateExit(animator, stateInfo, layerIndex);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}