using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class TextAdventurePerson : TextAdventureEnums {

	public string personName;
	[TextArea(5, 20)]
	public string description;

	private Animator conversationTree;

	[HideInInspector]
	public DialoguePath currPath = DialoguePath.Stay;

	[HideInInspector]
	public string getDialogue = "";

	// Node information
	private DialogueNode[] allDialogues;
	private DialogueNode dialogue;

	// Tracks node movement
	private int prevNode = 0;
	private int currNode = 0;


	[Header("Use this to update")]
	[Range(0, 1)]
	public int updater;

	private void Start() {
		// Set name to game object name
		personName = gameObject.name;

		// Set defailt dialogue path
		currPath = DialoguePath.Stay;

		// Get Animator
		conversationTree = gameObject.GetComponent<Animator>();

		// Get all behaviours from the animator
		allDialogues = conversationTree.GetBehaviours<DialogueNode>();
	}

	// Used for updating inspector variables
	private void OnValidate() {
		// Set name to game object name
		personName = gameObject.name;

		// Set defailt dialogue path
		currPath = DialoguePath.Stay;

		// Get Animator
		conversationTree = gameObject.GetComponent<Animator>();

		// Get all behaviours from the animator
		allDialogues = conversationTree.GetBehaviours<DialogueNode>();
	}

	private void Update() {
		// Get current node hash
		currNode = conversationTree.GetCurrentAnimatorStateInfo(0).shortNameHash;

		// Get currently enabled dialogue node behaviour
		for (int i = 0; i < allDialogues.Length; i++) {
			if (allDialogues[i].isEnabled) {
				dialogue = allDialogues[i];
			}
		}

		// Update variables if in new node
		if (currNode != prevNode) {
			getDialogue = dialogue.dialogue + "\n";
		}

		// Store node for comparison
		prevNode = currNode;

		UpdateAnimator();

		// Rest Dialogue Path
		currPath = DialoguePath.Stay;
	}

	public void UpdateAnimator() {
		conversationTree.SetInteger("Path", (int)currPath);
	}
}