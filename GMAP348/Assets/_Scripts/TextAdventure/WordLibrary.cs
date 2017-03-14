using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordLibrary : TextAdventureEnums {
	public enum ActionType {
		Examine,
		Converse,
		PickUp,
		Inventory,
		Move,
		Use,
		NotAction
	}

	[Header("Action Verb Library")]
	public List<string> examineVerbs;
	public List<string> converseVerbs;
	public List<string> pickupVerbs;
	public List<string> inventoryVerbs;
	public List<string> movementVerbs;
	public List<string> itemUseVerbes;

	[HideInInspector]
	public List<string> examineVerbsCheck;
	[HideInInspector]
	public List<string> converseVerbsCheck;
	[HideInInspector]
	public List<string> pickupVerbsCheck;
	[HideInInspector]
	public List<string> inventoryVerbsCheck;
	[HideInInspector]
	public List<string> movementVerbsCheck;
	[HideInInspector]
	public List<string> itemUseVerbesCheck;

	/// <summary>
	/// Interprets a string to enum.
	/// </summary>
	/// <returns>ActionType of a word.</returns>
	/// <param name="word">Word needed to interpret ActionType.</param>
	public ActionType ActionStringToEnum(string word) {
		// Test word against words in all Libraries
		if (examineVerbsCheck.Contains(word)) {
			// Check Examine verbs
			return ActionType.Examine;

		} else if (converseVerbsCheck.Contains(word)) {
			// Check Conversation starting verbs
			return ActionType.Converse;

		} else if (pickupVerbsCheck.Contains(word)) {
			// Check for item Pickup verbs
			return ActionType.PickUp;

		} else if (inventoryVerbsCheck.Contains(word)) {
			// Check for Inventory check requests
			return ActionType.Inventory;

		} else if (movementVerbsCheck.Contains(word)) {
			// Check if attempting to Move
			return ActionType.Move;

		} else if (itemUseVerbesCheck.Contains(word)) {
			// Check if attempting to use an item
			return ActionType.Use;

		} else {
			// Return if word is not in Library
			return ActionType.NotAction;
		}
	}

	[Header("Preposition Library")]
	public List<string> prepositions;

	[HideInInspector]
	public List<string> prepositionCheck;

	/// <summary>
	/// Checks if a word is in the Preposition
	/// Library
	/// </summary>
	/// <returns><c>true</c>, a word is a preposition, <c>false</c> if it is not.</returns>
	/// <param name="word">Word used to check against the Preposition Library.</param>
	public bool CheckIfPreposition(string word) {
		if (prepositionCheck.Contains(word)) {
			return true;
		} else {
			return false;
		}
	}

	[Header("Conversation Library")]
	public List<string> yesWords;
	public List<string> noWords;
	public List<string> directWords;
	public List<string> reservedWords;
	public List<string> endWords;

	[HideInInspector]
	public List<string> yesWordsCheck;
	[HideInInspector]
	public List<string> noWordsCheck;
	[HideInInspector]
	public List<string> directWordsCheck;
	[HideInInspector]
	public List<string> reservedWordsCheck;
	[HideInInspector]
	public List<string> endWordsCheck;

	public DialoguePath ConversationStringToEnum(string response) {
		// Test word against words in all Conversation Libraries
		if (yesWordsCheck.Contains(response)) {
			// Check if response is in Yes library
			return DialoguePath.Yes;

		} else if (noWordsCheck.Contains(response)) {
			// Check if response is in No library
			return DialoguePath.No;

		} else if (directWordsCheck.Contains(response)) {
			// Check if response is in Direct library
			return DialoguePath.Direct;

		} else if (reservedWordsCheck.Contains(response)) {
			// Check if response is in Reserved library
			return DialoguePath.Reserved;

		} else if (endWordsCheck.Contains(response)) {
			// Check if response is in End Library
			return DialoguePath.End;

		} else {
			// Return if response is not in Library
			return DialoguePath.Stay;
		}
	}


	/// <summary>
	/// Gets the determiner of a noun.
	/// </summary>
	/// <returns>The noun determiner.</returns>
	/// <param name="noun">Noun to be interpreted.</param>
	public string GetNounDeterminer(string noun) {
		// Check if noun ends with s
		if (noun[noun.Length - 1] == 's') {
			return "";
		} else {
			// Check if noun starts with a vowel
			if ("AEIOU".IndexOf(noun[0]) != -1) {
				return "An";
			} else {
				return "A";
			}
		}
	}

	/// <summary>
	/// Gets the distinct of one string list into another list.
	/// </summary>
	/// <param name="checkList">Used to get list of elements.</param>
	/// <param name="toList">List that will have unique elements.</param>
	public void GetDistinctElements(List<string> checkList, List<string> toList) {
		toList.Clear();
		var uniqueCheck = new HashSet<string>(checkList);
		foreach(string el in uniqueCheck) {
			toList.Add(el.ToLower());
		}
	}

	/// <summary>
	/// Capitalizes the first character of a string.
	/// 
	/// Created by: Equiso
	/// http://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case-for-maximum-performance#4135491
	/// </summary>
	/// <returns>The letter to upper.</returns>
	/// <param name="str">String.</param>
	public string FirstLetterToUpper(string str) {
		if (str == null)
			return null;

		if (str.Length > 1)
			return char.ToUpper(str[0]) + str.Substring(1);

		return str.ToUpper();
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
			if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')
			    || c == '\'' || c == '/') {
				sb.Append(c);
			}
		}
		return sb.ToString();
	}
}