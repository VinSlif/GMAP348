using UnityEngine;

public class TextAdventureEnums : MonoBehaviour {
	public enum DialoguePath {
		Stay,
		Yes,
		No,
		Direct,
		Reserved,
		End,
	}

	public enum ItemType {
		Weapon,
		Clue,
		Misc,
		NotItem,
	}

	public enum Directions {
		Stay,
		North,
		East,
		South,
		West,
	}
}