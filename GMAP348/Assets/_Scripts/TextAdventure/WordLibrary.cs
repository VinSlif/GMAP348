using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordLibrary : MonoBehaviour {
	public enum ActionType {
		Examine,
		Converse,
		PickUp,
		Inventory,
		Attack,
		NotAction
	}

	[Header("Used to check Action verb")]
	public List<string> examineVerbs;
	public List<string> converseVerbs;
	public List<string> pickUpVerbs;
	public List<string> inventoryVerbs;
	public List<string> attackVerbs;

	public ActionType ActionStringToEnum(string word) {
		if (examineVerbs.Contains(word)) {
			return ActionType.Examine;
		} else if (converseVerbs.Contains(word)) {
			return ActionType.Converse;
		} else if (pickUpVerbs.Contains(word)) {
			return ActionType.PickUp;
		} else if (inventoryVerbs.Contains(word)) {
			return ActionType.Inventory;
		} else if (attackVerbs.Contains(word)) {
			return ActionType.Attack;
		} else {
			return ActionType.NotAction;
		}
	}

	[Header("Used to check propositions")]
	public List<string> prepositions;

	public bool CheckIfPreposition(string word) {
		if (prepositions.Contains(word)) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Removes the special characters from a string.
	/// 
	/// Created by: Adam Tal
	/// http://stackoverflow.com/questions/16725848/how-to-split-text-into-words
	/// </summary>
	/// <returns>A string with no special characters.</returns>
	/// <param name="str">String to remove special characters.</param>
	public static string RemoveSpecialCharacters(string str) {
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach(char c in str) {
			if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '\'') {
				sb.Append(c);
			}
		}
		return sb.ToString();
	}
}