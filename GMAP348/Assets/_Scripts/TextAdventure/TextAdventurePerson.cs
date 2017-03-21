using System.Collections;
using UnityEngine;

public class TextAdventurePerson : TextAdventureEnums {

	public string personName;
	[TextArea(5, 20)]
	public string description;

	[HideInInspector]
	public Animator conversationTree;

	// Node information
	[HideInInspector]
	public DialogueNode[] allDialogues;


	private void Awake() {
		// Set name to game object name
		personName = gameObject.name;

		// Get Animator
		conversationTree = gameObject.GetComponent<Animator>();

		// Get all behaviours from the animator
		allDialogues = conversationTree.GetBehaviours<DialogueNode>();
	}
}