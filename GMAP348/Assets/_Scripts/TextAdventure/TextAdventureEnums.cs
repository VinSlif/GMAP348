using UnityEngine;

public class TextAdventureEnums : MonoBehaviour {

	// Controls Location Animator state flow
	public enum Directions {
		Stay,
		North,
		East,
		South,
		West,
	}

	// Controls Dialogue Animator state flow
	public enum DialoguePath {
		Stay,
		Yes,
		No,
		Direct,
		Reserved,
		End,
	}

	// Defines the type of an item
	public enum ItemType {
		NotItem,
		Weapon,
		Clue,
		Misc,
	}

	// Defines the type of action
	public enum ActionType {
		NotAction,
		Examine,
		Converse,
		PickUp,
		Move,
		Use,
	}

	// Defines what type of use
	// action was typed
	public enum UseAction {
		NotAction,
		Combine,
		Attack,
		Place,
		Toggle,
		Eat,
		Drink,
		Smell,
		Listen,
		Touch,
	}

	// Defines what the player
	// is attempting to check
	public enum CheckID {
		Inventory,
		Room,
		NoID,
	}
}