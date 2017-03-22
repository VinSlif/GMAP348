using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordLibrary : TextAdventureEnums {
	
	[Header("Action Verb Library")]
	public List<string> examineVerbs;
	public List<string> converseVerbs;
	public List<string> pickupVerbs;
	public List<string> movementVerbs;

	private List<string> examineVerbsCheck;
	private List<string> converseVerbsCheck;
	private List<string> pickupVerbsCheck;
	private List<string> movementVerbsCheck;

	/// <summary>
	/// Interprets a string to an action enum.
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

		} else if (movementVerbsCheck.Contains(word)) {
			// Check if attempting to Move
			return ActionType.Move;

		} else if (IsUseVerb(word)) { // check all use library
			// Check if attempting to use an item
			return ActionType.Use;

		} else {
			// Return if word is not in Library
			return ActionType.NotAction;
		}
	}


	[Header("Preposition Library")]
	public List<string> prepositions;

	private List<string> prepositionCheck;

	/// <summary>
	/// Checks if a word is in the Preposition Library
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


	[Header("Item Use Library")]
	public List<string> combineVerbs;
	public List<string> attackVerbs;
	public List<string> placeVerbs;
	public List<string> toggleVerbs;
	public List<string> eatVerbs;
	public List<string> drinkVerbs;
	public List<string> smellVerbs;
	public List<string> listenVerbs;
	public List<string> touchVerbs;

	private List<string> combineVerbsCheck;
	private List<string> attackVerbsCheck;
	private List<string> placeVerbsCheck;
	private List<string> toggleVerbsCheck;
	private List<string> eatVerbsCheck;
	private List<string> drinkVerbsCheck;
	private List<string> smellVerbsCheck;
	private List<string> listenVerbsCheck;
	private List<string> touchVerbsCheck;

	private bool IsUseVerb(string verb) {
		if (combineVerbsCheck.Contains(verb) || attackVerbsCheck.Contains(verb) ||
		    placeVerbsCheck.Contains(verb) || toggleVerbsCheck.Contains(verb) ||
		    eatVerbsCheck.Contains(verb) || drinkVerbsCheck.Contains(verb) ||
		    smellVerbsCheck.Contains(verb) || listenVerbsCheck.Contains(verb) ||
		    touchVerbsCheck.Contains(verb)) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Interpets the type of use action to an enum.
	/// </summary>
	/// <returns>The use action enum.</returns>
	/// <param name="phrase">Phrase to be interpreted.</param>
	public UseAction UseActionStringToEnum(string phrase) {
		// Check if phrase is in Attack library
		foreach(string s in attackVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Attack;
			}
		}

		// Check if phrase is in Combine library
		foreach(string s in combineVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Combine;
			}
		}

		// Check if phrase is in Drink library
		foreach(string s in drinkVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Drink;
			}
		}

		// Check if phrase is in Eat library
		foreach(string s in eatVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Eat;
			}
		}

		// Check if phrase is in Listen library
		foreach(string s in listenVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Listen;
			}
		}

		// Check if phrase is in Place library
		foreach(string s in placeVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Place;
			}
		}

		// Check if phrase is in Smell library
		foreach(string s in smellVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Smell;
			}
		}

		// Check if phrase is in Toggle library
		foreach(string s in toggleVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Toggle;
			}
		}

		// Check if phrase is in Touch library
		foreach(string s in touchVerbsCheck) {
			if (phrase.Contains(s)) {
				return UseAction.Touch;
			}
		}

		// Return if phrase is not in Library
		return UseAction.NotAction;
	}



	[Header("Object Identifiers")]
	public List<string> inventory;
	public List<string> location;

	[HideInInspector]
	public List<string> inventoryCheck;
	[HideInInspector]
	public List<string> locationCheck;

	public CheckID IdentityExamineTarget(string potentialTarget) {
		if (inventoryCheck.Contains(potentialTarget)) {
			return CheckID.Inventory;
		} else if (locationCheck.Contains(potentialTarget)) {
			return CheckID.Room;
		} else {
			return CheckID.NoID;
		}
	}


	[Header("Conversation Library")]
	public List<string> yesPhrases;
	public List<string> noPhrases;
	public List<string> directPhrases;
	public List<string> reservedPhrases;
	public List<string> endPhrases;

	private List<string> yesPhrasesCheck;
	private List<string> noPhrasesCheck;
	private List<string> directPhrasesCheck;
	private List<string> reservedPhrasesCheck;
	private List<string> endPhrasesCheck;

	/// <summary>
	/// Interprets the type of response based on the
	/// Conversation Library
	/// </summary>
	/// <returns>Response Enum.</returns>
	/// <param name="response">Player response to NPC conversation.</param>
	public DialoguePath ConversationStringToEnum(string response) {
		// Check if response is in Yes library
		foreach(string s in yesPhrasesCheck) {
			if (response.Contains(s)) {
				return DialoguePath.Yes;
			}
		}

		// Check if response is in No library
		foreach(string s in noPhrasesCheck) {
			if (response.Contains(s)) {
				return DialoguePath.No;
			}
		}

		// Check if response is in Direct Library
		foreach(string s in directPhrasesCheck) {
			if (response.Contains(s)) {
				// Check if response is in Direct library
				return DialoguePath.Direct;
			}
		}

		// Check if response is in Reserved library
		foreach(string s in reservedPhrasesCheck) {
			if (response.Contains(s)) {
				return DialoguePath.Reserved;
			}
		}

		// Check if response is in End Library
		foreach(string s in endPhrasesCheck) {
			if (response.Contains(s)) {
				return DialoguePath.End;
			}
		}

		// Return if response is not in Library
		return DialoguePath.Stay;
	}

	// Organize Library Functions

	public void Init() {
		// Set unique verbs to use as checks
		examineVerbsCheck = GetDistinctElements(examineVerbs);
		converseVerbsCheck = GetDistinctElements(converseVerbs);
		pickupVerbsCheck = GetDistinctElements(pickupVerbs);
		movementVerbsCheck = GetDistinctElements(movementVerbs);

		// Set uniuqe preposition words
		prepositionCheck = GetDistinctElements(prepositions);

		// Set unique item use verbs
		combineVerbsCheck = GetDistinctElements(combineVerbs);
		attackVerbsCheck = GetDistinctElements(attackVerbs);
		placeVerbsCheck = GetDistinctElements(placeVerbs);
		toggleVerbsCheck = GetDistinctElements(toggleVerbs);
		eatVerbsCheck = GetDistinctElements(eatVerbs);
		drinkVerbsCheck = GetDistinctElements(drinkVerbs);
		smellVerbsCheck = GetDistinctElements(smellVerbs);
		listenVerbsCheck = GetDistinctElements(listenVerbs);
		touchVerbsCheck = GetDistinctElements(touchVerbs);

		// Set unique object identifiers
		inventoryCheck = GetDistinctElements(inventory);
		locationCheck = GetDistinctElements(location);

		// Set unique conversastion words
		yesPhrasesCheck = GetDistinctElements(yesPhrases);
		noPhrasesCheck = GetDistinctElements(noPhrases);
		directPhrasesCheck = GetDistinctElements(directPhrases);
		reservedPhrasesCheck = GetDistinctElements(reservedPhrases);
		endPhrasesCheck = GetDistinctElements(endPhrases);
	}

	/// <summary>
	/// Gets the distinct elements of one string list into another list.
	/// </summary>
	/// <param name="checkList">Used to get list of elements.</param>
	/// <param name="toList">List that will have unique elements.</param>
	private List<string> GetDistinctElements(List<string> checkList) {
		List<string> toList = new List<string>();
		var uniqueCheck = new HashSet<string>(checkList);
		foreach(string el in uniqueCheck) {
			toList.Add(el.ToLower());
		}
		return toList;
	}


	// Formatting Functions

	/// <summary>
	/// Gets the determiner of a noun.
	/// </summary>
	/// <returns>The noun determiner.</returns>
	/// <param name="noun">Noun to be interpreted.</param>
	public string GetNounDeterminer(string noun) {
		// Ensure check is lowercase
		noun = noun.ToLower();

		// Check if noun ends with s
		if (noun[noun.Length - 1] == 's') {
			return "";
		} else {
			// Check if noun starts with a vowel
			if ("aeiou".IndexOf(noun[0]) != -1) {
				return "An";
			} else {
				return "A";
			}
		}
	}

	/// <summary>
	/// Removes the first word of a sentence.
	/// </summary>
	/// <returns>The sentence without the first word.</returns>
	/// <param name="sentence">Sentence.</param>
	public string RemoveFirstWord(string sentence) {
		string[] words = sentence.Split(' ');
		string removedFirstWordSentence = "";

		if (words.Length > 1) {
			for (int i = 1; i < words.Length; i++) {
				removedFirstWordSentence += words[i] + " ";
			}
			var stringBldr = new System.Text.StringBuilder(removedFirstWordSentence);
			stringBldr.Remove(removedFirstWordSentence.LastIndexOf(" "), 1);
			removedFirstWordSentence = stringBldr.ToString();
		}

		return removedFirstWordSentence;
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
	/// Formats a list of strings into a single, formatted string with commas and an 'and.'
	/// </summary>
	/// <returns>A formatted string of the list.</returns>
	/// <param name="itemList">List of strings.</param>
	public string FormatCommaList(List<string> itemList) {
		string formatted = "";
		int numItems = 0;
		foreach(string s in itemList) {
			if (s != "") {
				formatted += s + ", ";
				numItems++;
			}
		}

		formatted = new System.Text.StringBuilder(formatted).Remove(formatted.LastIndexOf(", "), 2).ToString();

		if (numItems == 2) {
			var stringBldr = new System.Text.StringBuilder(formatted);
			stringBldr.Insert(formatted.LastIndexOf(", ") + 2, " and ");
			stringBldr.Remove(formatted.LastIndexOf(", "), 2);
			formatted = stringBldr.ToString();

		} else if (numItems > 2) {
			var stringBldr = new System.Text.StringBuilder(formatted);
			stringBldr.Insert(formatted.LastIndexOf(", ") + 2, "and ");
			formatted = stringBldr.ToString();
		}

		return formatted;
	}

	/// <summary>
	/// Removes the special characters from a string.
	/// 
	/// Created by: Adam Tal
	/// http://stackoverflow.com/questions/16725848/how-to-split-text-into-words
	/// </summary>
	/// <returns>A string with no special characters.</returns>
	/// <param name="str">String to remove special characters.</param>
	public string RemoveSpecialCharacters(string str) {
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