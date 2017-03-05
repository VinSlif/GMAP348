using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class TextAdventureItem : MonoBehaviour {
	public enum Type {
		Weapon,
		Clue,
		Misc,
		NotItem
	}

	public string itemName;
	public Type type;
	public string description;
	public int damage;

	[Header("Use this to update")]
	[Range(0, 1)]
	public int slider;

	void Start() {
		itemName = gameObject.name;
	}

	void OnValidate() {
		itemName = gameObject.name;
	}
}