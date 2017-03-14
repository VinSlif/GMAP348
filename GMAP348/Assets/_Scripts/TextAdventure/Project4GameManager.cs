using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Project4GameManager : WordLibrary {
	[Space(12)]
	[Header("Handles the Display")]
	public Canvas canvasUI;

	// Handles player input
	private InputField playerInput;
	private InputField.SubmitEvent se;

	// Tracks command mode
	private bool inDevMode = false;
	private bool inConversation = false;

	// Handles display output
	private Text output;
	private ScrollRect scrollBar;
	private string outputConcatenation;

	// Handles location
	private Animator locations;

	// Location information
	private LocationManager[] allLocInfo;
	private LocationManager locInfo;

	// Tracks location content
	public List<string> locItems;
	public List<string> locPeople;

	// Tracks location movement
	private int prevLocation = 0;
	private int currLocation = 0;

	// Tracks current conversation partner
	private TextAdventurePerson conversationPartner;

	// Tracks Dialogue Node information
	private DialogueNode dialogue;

	// Tracks Dialogue Node movement
	private int prevNode = 0;
	private int currNode = 0;

	// Reference play stats
	private TextAdventurePlayer player;

	[Header("Game Entities Reference")]
	public Transform allItemsParent;
	public Transform allPeopleParent;


	private void Awake() {
		// Set unique verbs to use as checks
		GetDistinctElements(examineVerbs, examineVerbsCheck);
		GetDistinctElements(converseVerbs, converseVerbsCheck);
		GetDistinctElements(pickupVerbs, pickupVerbsCheck);
		GetDistinctElements(inventoryVerbs, inventoryVerbsCheck);
		GetDistinctElements(movementVerbs, movementVerbsCheck);
		GetDistinctElements(itemUseVerbes, itemUseVerbesCheck);

		// Set uniuqe preposition words
		GetDistinctElements(prepositions, prepositionCheck);

		// Set unique conversastion words
		GetDistinctElements(yesWords, yesWordsCheck);
		GetDistinctElements(noWords, noWordsCheck);
		GetDistinctElements(directWords, directWordsCheck);
		GetDistinctElements(reservedWords, reservedWordsCheck);
		GetDistinctElements(endWords, endWordsCheck);

		// Get player
		player = gameObject.GetComponent<TextAdventurePlayer>();

		// Get Canvas fields
		playerInput = canvasUI.GetComponentInChildren<InputField>();
		scrollBar = canvasUI.GetComponentInChildren<ScrollRect>();
		output = scrollBar.gameObject.GetComponentInChildren<Text>();

		// Set up input listener
		se = new InputField.SubmitEvent();
		se.AddListener(SubmitInput);
		playerInput.onEndEdit = se;

		// Get Animator
		locations = GetComponent<Animator>();

		// Get all behaviours from the animator
		allLocInfo = locations.GetBehaviours<LocationManager>();
	}

	private void Update() {
		// Update location info
		GetLocationInfo();
		// Update dialouge info
		GetDialogueInfo();
	}


	/// <summary>
	/// Used to get and interpret the input field.
	/// </summary>
	/// <param name="newText">Text submitted in input field.</param>
	private void SubmitInput(string newText) {
		// Clear output concatenation
		outputConcatenation = "";

		// Display player command
		output.text += "> " + newText + "\n";

		// Get and Interpret player input
		CheckInput(newText.ToLower());

		// Display output
		output.text += outputConcatenation + "\n";

		// Reset Input Text Field
		playerInput.text = "";

		// Send the scroll to the bottom
		Canvas.ForceUpdateCanvases();
		scrollBar.verticalScrollbar.value = 0;
		Canvas.ForceUpdateCanvases();

		// Allow player to continue inputting text
		playerInput.ActivateInputField();
	}


	private void CheckInput(string inText) {
		// Split input into string array
		string[] pInputArr = inText.Split(' ');
		for (int i = 0; i < pInputArr.Length; i++) {
			pInputArr[i] = RemoveSpecialCharacters(pInputArr[i]);
		}

		// Check if input exists
		if (inText != "") {
			// Check if input starts with space
			if (pInputArr[0] != "") {
				// Check if in Command Mode
				if (inDevMode) {
					// Interpret command action
					InterpretDevText(pInputArr);
					// Check if in Conversation Mode
				} else if (inConversation) {
					// Interpret conversation action
					InterpretConversationText(pInputArr);

				} else {
					// Interpret player action
					InterpretText(pInputArr);
				}
			} else {
				outputConcatenation += "You cannot start with a space.\n";
			}
		} else {
			outputConcatenation += "You need to actually type something.\n";
		}
	}

	/// <summary>
	/// Interprets the text based on the Word Library.
	/// </summary>
	/// <param name="toInterpret">Input to be interpreted.</param>
	private void InterpretText(string[] toInterpret) {
		// Check input
		if (toInterpret[0][0] != '/') {
			
			// Check if array starts with preposition
			if (!CheckIfPreposition(toInterpret[0])) {
				// Get Verb Action Type
				ActionType currAction = ActionStringToEnum(toInterpret[0]);

				// Check if valid Action
				switch(currAction) {
				case ActionType.Examine:
					ExamineAction(toInterpret);

					break;
				case ActionType.PickUp:
					PickupAction(toInterpret);
						
					break;
				case ActionType.Inventory:
					InventoryAction(toInterpret);
						
					break;
				case ActionType.Converse:
					StartConversationAction(toInterpret);
						
					break;
				case ActionType.Use:
					UseItemAction(toInterpret);
						
					break;
				case ActionType.Move:
					MoveAction(toInterpret);

					break;
				default:
					outputConcatenation += "\"" + toInterpret[0] + "\" is not something I know how to do.";
					break;
				}
			} else {
				outputConcatenation += "You cannot start a sentence with a preposition.";
			}
		} else {
			if (toInterpret[0] == "/getmethedev") {
				inDevMode = true;
				outputConcatenation += "Dev Mode Activated";
			} else {
				outputConcatenation += toInterpret[0] + " command not found";
			}
		}
	}

	/// <summary>
	/// Adds an item from the location to the player's inventory.
	/// </summary>
	/// <param name="action">Input.</param>
	private void PickupAction(string[] action) {
		// Check if supplied a noun
		if (action.Length > 1) {
			for (int i = 0; i < action.Length; i++) {
				if (locItems.Contains(action[i])) {
					// Add item to inventory
					player.inventory.Add(GetItemByName(action[i]));

					// Remove item reference from Animator + current location info
					locInfo.items.Remove(FirstLetterToUpper(action[i]));
					locItems.Remove(action[i].ToLower());

					// Display picked up item
					outputConcatenation += "I picked up " + GetNounDeterminer(action[i]).ToLower() + " " + action[i];
					return;
				}
			}
			outputConcatenation += "That is not here.";
		} else {
			outputConcatenation += "Must supply an item to pick up.";
		}
	}

	private void ExamineAction(string[] action) {
		outputConcatenation += "Looking at ";
		// Check if room
		// Check if item
		// Check if person
	}

	/// <summary>
	/// Displays player inventory or checks item in inventory.
	/// </summary>
	/// <param name="action">Input.</param>
	private void InventoryAction(string[] action) {
		// Check if inventory is empty
		if (player.inventory.Count > 0) {
			// Check if examing item in inventory
			if (action.Length > 1) {
				GameObject itemGO = GetItemByName(action[1]);
				if (itemGO != null) {
					TextAdventureItem item = itemGO.GetComponent<TextAdventureItem>();
					if (DoesPlayerHaveItem(item.itemName)) {
						outputConcatenation += GetNounDeterminer(item.itemName) + " " + item.itemName + "\n" + item.description;
					} else {
						outputConcatenation += "I do not have " + GetNounDeterminer(item.itemName).ToLower() + " " + item.itemName;
					}
				} else {
					outputConcatenation += "I do not have " + GetNounDeterminer(action[1]).ToLower() + " " + action[1];
				}
			} else {
				outputConcatenation += "I have :\n";
				// Get all items in inventory
				foreach(GameObject item in player.inventory) {
					// Get all item names
					string itemName = item.GetComponent<TextAdventureItem>().itemName;
					outputConcatenation += "    " + GetNounDeterminer(itemName) + " " + itemName + "\n";
				}
				outputConcatenation += "\n";
			}
		} else {
			outputConcatenation += "I do not have anything.\n";
		}
	}

	/// <summary>
	/// Starts the conversation action.
	/// </summary>
	/// <param name="action">Action.</param>
	private void StartConversationAction(string[] action) {
		// Check if supplied a noun
		if (action.Length > 1) {
			for (int i = 0; i < action.Length; i++) {
				if (locPeople.Contains(action[i])) {
					conversationPartner = GetPersonByName(action[i]).GetComponent<TextAdventurePerson>();
					inConversation = true;
					return;
				}
			}
			outputConcatenation += "That person is not here.";
		} else {
			outputConcatenation += "I need to talk to someone.";
		}
	}

	private void UseItemAction(string[] action) {
		outputConcatenation += "Using item";
		// check inventory for item
		// get item type
		// new word library for action type?
	}

	private void MoveAction(string[] action) {
		if (action.Length > 1) {
			Directions moveDir = Directions.Stay;
			if (action[1] == "n" || action[1] == "north") {
				moveDir = Directions.North;
			} else if (action[1] == "e" || action[1] == "east") {
				moveDir = Directions.East;
			} else if (action[1] == "s" || action[1] == "south") {
				moveDir = Directions.South;
			} else if (action[1] == "w" || action[1] == "west") {
				moveDir = Directions.West;
			} else {
				outputConcatenation += "That is not a valid direction.";
			}

			// Send move direction to animator
			locations.SetInteger("Direction", (int)moveDir);
		} else {
			outputConcatenation += "Need to supply a valid direction.";
		}
	}

	/// <summary>
	/// Gets the location information from the Location Animator.
	/// </summary>
	private void GetLocationInfo() {
		// Get current location hash
		currLocation = locations.GetCurrentAnimatorStateInfo(0).shortNameHash;

		// Get currently enabled location manager behaviour
		for (int i = 0; i < allLocInfo.Length; i++) {
			if (allLocInfo[i].isEnabled) {
				locInfo = allLocInfo[i];
			}
		}

		// Checks location hash against previous hash
		if (currLocation != prevLocation) {
			// Immediately display location description
			output.text += locInfo.description + "\n";

			// Update items in room list
			if (locItems.Count > 0) {
				locItems.Clear();
			}
			if (locInfo.items.Count > 0) {
				if (GetNounDeterminer(locInfo.items[0]) == "") {
					output.text += "There are";
				} else {
					output.text += "There is ";
				}

				string showItems = "";
				foreach(string s in locInfo.items) {
					if (GetItemByName(s) != null) {
						showItems += GetNounDeterminer(s).ToLower() + " " + s + ", ";
						locItems.Add(s.ToLower());
					}
				}
					
				showItems = new System.Text.StringBuilder(showItems).Remove(showItems.LastIndexOf(", "), 2).ToString();

				if (locInfo.items.Count == 2) {
					var stringBldr = new System.Text.StringBuilder(showItems);
					stringBldr.Insert(showItems.LastIndexOf(", ") + 2, " and ");
					stringBldr.Remove(showItems.LastIndexOf(", "), 2);
					showItems = stringBldr.ToString();
				} else if (locInfo.items.Count > 2) {
					var stringBldr = new System.Text.StringBuilder(showItems);
					stringBldr.Insert(showItems.LastIndexOf(", ") + 2, "and ");
					showItems = stringBldr.ToString();
				}
				output.text += showItems + " here.";
			}

			output.text += "\n";

			// Update people in room list
			if (locPeople.Count > 0) {
				locPeople.Clear();
			}
			if (locInfo.people.Count > 0) {
				output.text += "You see ";

				string showPeople = "";
				foreach(string s in locInfo.people) {
					if (GetPersonByName(s) != null) {
						showPeople += s + ", ";
						locPeople.Add(s.ToLower());
					}
				}

				showPeople = new System.Text.StringBuilder(showPeople).Remove(showPeople.LastIndexOf(", "), 2).ToString();

				if (locInfo.people.Count == 2) {
					var stringBldr = new System.Text.StringBuilder(showPeople);
					stringBldr.Insert(showPeople.LastIndexOf(", ") + 2, " and ");
					stringBldr.Remove(showPeople.LastIndexOf(", "), 2);
					showPeople = stringBldr.ToString();
				} else if (locInfo.people.Count > 2) {
					var stringBldr = new System.Text.StringBuilder(showPeople);
					stringBldr.Insert(showPeople.LastIndexOf(", ") + 2, "and ");
					showPeople = stringBldr.ToString();
				}
				output.text += showPeople + " in the room.";
			}
		}

		// Store location for comparison
		prevLocation = currLocation;
	}


	// Conversation Interpreter
	private void InterpretConversationText(string[] toInterpret) {
		if (conversationPartner != null) {
			DialoguePath convoPath = ConversationStringToEnum(string.Join(" ", toInterpret));

			if (convoPath == DialoguePath.End) {
				outputConcatenation += "<i><b>" + conversationPartner.personName + "</b></i>\n" + "Bye.";
				inConversation = false;
				convoPath = DialoguePath.Stay;
			} else if (convoPath == DialoguePath.Stay) {
				outputConcatenation += "Not valid conversation item.";
			}

			conversationPartner.conversationTree.SetInteger("Path", (int)convoPath);
		} else {
			inConversation = false;
		}
	}

	/// <summary>
	/// Gets the dialogue information from the Dialogue Animator.
	/// </summary>
	private void GetDialogueInfo() {
		if (conversationPartner != null) {
			// Get current node hash
			currNode = conversationPartner.conversationTree.GetCurrentAnimatorStateInfo(0).shortNameHash;

			// Get currently enabled dialogue node behaviour
			for (int i = 0; i < conversationPartner.allDialogues.Length; i++) {
				if (conversationPartner.allDialogues[i].isEnabled) {
					dialogue = conversationPartner.allDialogues[i];
				}
			}

			// Checks node hash against previous hash
			if (currNode != prevNode) {
				output.text += "<i><b>" + conversationPartner.personName + "</b></i>\n" + dialogue.dialogue + "\n";
			}
					
			// Store node for comparison
			prevNode = currNode;
		}
	}


	// Developer Interpreter

	/// <summary>
	/// Interprets the text based on the Developer Commands.
	/// </summary>
	/// <param name="toInterpret">Input to be interpreted.</param>
	private void InterpretDevText(string[] toInterpret) {
		if (toInterpret[0][0] == '/') {
			if (toInterpret[0] == "/additem") {
				GameObject CheckItem = GetItemByName(toInterpret[1]);
				if (CheckItem != null) {
					player.inventory.Add(CheckItem);
				} else {
					outputConcatenation += toInterpret[1] + " is not an item in this game.";
				}

			} else if (toInterpret[0] == "/clear") {
				output.text = "";
			} else if (toInterpret[0] == "/exit") {
				inDevMode = false;
				outputConcatenation += "Dev Mode Deactivated";
			} else {
				outputConcatenation += "Invalid Command";
			}
		} else {
			outputConcatenation += "Commands must start with \"/\"";
		}
	}


	// Object Checks

	/// <summary>
	/// Evaluates if an item GameObject exists by its itemName.
	/// </summary>
	/// <returns>The item by name.</returns>
	/// <param name="itemName">Item name to be evaluated.</param>
	private GameObject GetItemByName(string itemName) {
		foreach(Transform item in allItemsParent) {
			if (itemName.ToLower() == item.gameObject.GetComponent<TextAdventureItem>().itemName.ToLower()) {
				return item.gameObject;
			}
		}
		return null;
	}

	/// <summary>
	/// Evaluates if a person GameObject exists by its personName.
	/// </summary>
	/// <returns>A person by name.</returns>
	/// <param name="personName">Person name to be evaluated.</param>
	private GameObject GetPersonByName(string personName) {
		foreach(Transform person in allPeopleParent) {
			if (personName.ToLower() == person.gameObject.GetComponent<TextAdventurePerson>().personName.ToLower()) {
				return person.gameObject;
			}
		}
		return null;
	}

	/// <summary>
	/// Determines if a player has an item in his/her inventory.
	/// </summary>
	/// <returns><c>true</c>, if player has item, <c>false</c> otherwise.</returns>
	/// <param name="itemName">Item name to be evaluated.</param>
	private bool DoesPlayerHaveItem(string itemName) {
		if (player.inventory.Count > 0) {
			foreach(GameObject item in player.inventory) {
				if (itemName.ToLower() == item.GetComponent<TextAdventureItem>().itemName.ToLower()) {
					return true;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// Wait the specified time t.
	/// </summary>
	/// <param name="t">Time in seconds to wait.</param>
	IEnumerator Wait(float t) {
		yield return new WaitForSeconds(t);
	}
}