using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class TextAdventureItem : TextAdventureEnums {

	public string itemName;
	public ItemType type;
	[TextArea(5, 20)]
	public string description;
	public int damage;

	[Header("Use this to update")]
	[Range(0, 1)]
	public int updater;

	void Start() {
		itemName = gameObject.name;
	}

	void OnValidate() {
		itemName = gameObject.name;
	}
}