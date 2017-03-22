using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Project4GameManager : TextAdventureEnums {

	// Handles text referencing
	private WordLibrary library;

	[Header("Handles the Display")]
	public Canvas canvasUI;

	// Handles player input
	private InputField playerInput;
	private InputField.SubmitEvent se;

	// Tracks command mode
	private bool inDevMode = false;

	// Handles display output
	private Text output;
	private string outputConcatenation;

	// Handles location
	private Animator locations;

	// Location information
	private LocationManager[] allLocInfo;
	private LocationManager locInfo;

	// Tracks location content
	private List<string> locItems = new List<string>();
	private List<string> locItemDisplay = new List<string>();
	private List<string> locPeople = new List<string>();
	private List<string> locPeopleDisplay = new List<string>();

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
		// Get Word Library
		library = gameObject.GetComponent<WordLibrary>();

		// Initialize Word Library
		library.Init();

		// Get player
		player = gameObject.GetComponent<TextAdventurePlayer>();

		// Get Canvas fields
		playerInput = canvasUI.GetComponentInChildren<InputField>();
		output = canvasUI.GetComponentInChildren<ScrollRect>().gameObject.GetComponentInChildren<Text>();

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

		// Allow player to continue inputting text
		playerInput.ActivateInputField();
	}

	private void CheckInput(string inText) {
		// Check if input exists
		if (inText != "") {

			// Split input into string array
			string[] inputArr = inText.Split(' ');
			for (int i = 0; i < inputArr.Length; i++) {
				inputArr[i] = library.RemoveSpecialCharacters(inputArr[i]);
			}

			// Check if input starts with space
			if (inputArr[0] != "") {

				// Check if in conversation
				if (conversationPartner != null) {
					string toInterpret = string.Join(" ", inputArr);
					InterpretConversationText(toInterpret);
				
				} else {
					// get action word
					string action = inputArr[0];

					// get action commands
					string toInterpret = library.RemoveFirstWord(string.Join(" ", inputArr));

					// Check if in Command Mode
					if (inDevMode) {
						// Interpret command action
						InterpretDevText(inputArr);

					} else {
						// Interpret player action
						// Check input
						if (inputArr[0][0] != '/') {
							// Get Verb Action Type
							ActionType currAction = library.ActionStringToEnum(action);

							// Check what type of action was used
							switch(currAction) {
							case ActionType.Examine:
								if (inputArr.Length > 1) {
									ExamineAction(toInterpret);
								} else {
									outputConcatenation += "That is not here.";
								}

								break;
							case ActionType.PickUp:
								if (inputArr.Length > 1) {
									PickupAction(toInterpret);
								} else {
									outputConcatenation += "Must supply an item to pick up.";
								}

								break;
							case ActionType.Converse:
								if (inputArr.Length > 1) {
									StartConversationAction(toInterpret);
								} else {
									outputConcatenation += "I need to talk to someone.";
								}

								break;
							case ActionType.Use:
								if (inputArr.Length > 1) {
									//UseItemAction(inputArr);
									outputConcatenation += "I would like to use an item, but that is not implemented yet.";
								} else {
									outputConcatenation += "I need an item to " + action + ".";
								}

								break;
							case ActionType.Move:
								if (inputArr.Length > 1) {
									MoveAction(toInterpret);
								} else {
									outputConcatenation += "Need to supply a valid direction.";
								}

								break;
							default:
								outputConcatenation += "\"" + action + "\" is not something I know how to do.";
								break;
							}
						} else {
							if (inputArr[0] == "/getmethedev") {
								inDevMode = true;
								outputConcatenation += "Dev Mode Activated";
							} else {
								outputConcatenation += action + " command not found";
							}
						}
					}
				}
			} else {
				outputConcatenation += "You cannot start with a space.";
			}
		} else {
			outputConcatenation += "You need to actually type something.";
		}
	}

	private void PickupAction(string getItem) {
		for (int i = 0; i < locItems.Count; i++) {
			// Check if item is in scene
			if (getItem.Contains(locItems[i])) {
				if (GetItemByName(locItems[i]).GetComponent<TextAdventureItem>().type != ItemType.NotItem) {
					// Add item to inventory
					player.inventory.Add(GetItemByName(locItems[i]));

					// Display picked up item
					outputConcatenation += "I picked up the " + library.FirstLetterToUpper(locItems[i]);

					// Remove item reference from Animator + current location info
					locInfo.items.Remove(library.FirstLetterToUpper(locItems[i]));
					locItems.RemoveAt(i);
					locItemDisplay.RemoveAt(i);

				} else {
					outputConcatenation += "I cannot get " + library.FirstLetterToUpper(locItems[i]);
				}
				return;
			}
		}
		outputConcatenation += "That is not here.";
	}

	private void ExamineAction(string examineTarget) {
		for (int i = 0; i < locItems.Count; i++) {
			if (examineTarget.Contains(locItems[i])) {
				outputConcatenation += GetItemByName(locItems[i]).GetComponent<TextAdventureItem>().description;
				return;
			}
		}

		for (int i = 0; i < player.inventory.Count; i++) {
			if (examineTarget.Contains(player.inventory[i].GetComponent<TextAdventureItem>().itemName)) {
				outputConcatenation += player.inventory[i].GetComponent<TextAdventureItem>().description;
				return;
			}
		}

		for (int i = 0; i < locPeople.Count; i++) {
			if (examineTarget.Contains(locPeople[i])) {
				outputConcatenation += GetPersonByName(locPeople[i]).GetComponent<TextAdventurePerson>().description;
				return;
			}
		}

		foreach(string s in library.locationCheck) {
			if (examineTarget.Contains(s)) {
				outputConcatenation += "You are at the " + locInfo.locationName + "\n" + locInfo.description + "\n\n";

				if (locItemDisplay.Count > 0) {
					if (library.GetNounDeterminer(locItemDisplay[0]) == "") {
						outputConcatenation += "There are";
					} else {
						outputConcatenation += "There is ";
					}

					outputConcatenation += library.FormatCommaList(locItemDisplay) + " here.\n\n";
				}

				if (locInfo.people.Count > 0) {
					outputConcatenation += "You see " + library.FormatCommaList(locPeopleDisplay) + " in the room.\n\n";
				}
				return;
			}
		}

		foreach(string s in library.inventoryCheck) {
			if (examineTarget.Contains(s)) {
				// Check if inventory is empty
				if (player.inventory.Count > 0) {
					outputConcatenation += "I have :\n";
					// Get all items in inventory
					foreach(GameObject item in player.inventory) {
						// Get all item names
						string itemName = item.GetComponent<TextAdventureItem>().itemName;
						outputConcatenation += "    " + library.GetNounDeterminer(itemName) + " " + itemName + "\n";
					}
					outputConcatenation += "\n";
				} else {
					outputConcatenation += "I do not have anything.\n";
				}
				return;
			}
		}

		outputConcatenation += "I do not know what you are trying to examine.";
	}

	private void StartConversationAction(string getPerson) {
		for (int i = 0; i < locPeople.Count; i++) {
			if (getPerson.Contains(locPeople[i].ToLower())) {
				conversationPartner = GetPersonByName(locPeople[i]).GetComponent<TextAdventurePerson>();
				return;
			}
		}
		outputConcatenation += "That person is not here.";
	}

	private void UseItemAction(string[] actionInterpet) {
		UseAction useType = library.UseActionStringToEnum(actionInterpet[0]);
		switch(useType) {
		case UseAction.Attack:
			if (actionInterpet.Length == 4) {
				if (GetItemByName(actionInterpet[1]).GetComponent<TextAdventureItem>().type == ItemType.Weapon) {
				} else {
					outputConcatenation += "I cannot attack with " + library.GetNounDeterminer(actionInterpet[1]) + " " + actionInterpet[1];
				}
			} else {
				outputConcatenation += "Need to type action as <i>action noun preposition noun</i>";
			}

			break;
		case UseAction.Combine:
			break;
		case UseAction.Drink:
			break;
		case UseAction.Eat:
			break;
		case UseAction.Listen:
			break;
		case UseAction.Place:
			break;
		case UseAction.Smell:
			break;
		case UseAction.Toggle:
			break;
		case UseAction.Touch:
			break;
		default:
			break;
		}
	}

	private void MoveAction(string getDirection) {
		Directions moveDir = Directions.Stay;
		if (getDirection.Contains("n") || getDirection.Contains("north")) {
			moveDir = Directions.North;
		} else if (getDirection.Contains("e") || getDirection.Contains("east")) {
			moveDir = Directions.East;
		} else if (getDirection.Contains("s") || getDirection.Contains("south")) {
			moveDir = Directions.South;
		} else if (getDirection.Contains("w") || getDirection.Contains("west")) {
			moveDir = Directions.West;
		} else {
			outputConcatenation += "That is not a valid direction.";
		}

		// Send move direction to animator
		locations.SetInteger("Direction", (int)moveDir);
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
			output.text += locInfo.description + "\n\n";

			// Update items in room list
			if (locItems.Count > 0) {
				locItems.Clear();
				locItemDisplay.Clear();
			}
			if (locInfo.items.Count > 0) {
				foreach(string s in locInfo.items) {
					if (GetItemByName(s) != null) {
						locItems.Add(s.ToLower());
						if (GetItemByName(s).GetComponent<TextAdventureItem>().displayInRoomInfo) {
							locItemDisplay.Add(s);
						} else {
							locItemDisplay.Add("");
						}
					}
				}

				if (locItemDisplay.Count > 0) {
					if (library.GetNounDeterminer(locItemDisplay[0]) == "") {
						output.text += "There are";
					} else {
						output.text += "There is ";
					}

					output.text += library.FormatCommaList(locItemDisplay) + " here.\n\n";
				}
			}

			// Update people in room list
			if (locPeople.Count > 0) {
				locPeople.Clear();
				locPeopleDisplay.Clear();
			}
			if (locInfo.people.Count > 0) {
				foreach(string s in locInfo.people) {
					if (GetPersonByName(s) != null) {
						locPeople.Add(s.ToLower());
						locPeopleDisplay.Add(s);
					}
				}

				if (locPeople.Count > 0) {
					output.text += "You see " + library.FormatCommaList(locPeopleDisplay) + " in the room.\n\n";
				}
			}
		}

		// Store location for comparison
		prevLocation = currLocation;
	}


	// Conversation Interpreter
	private void InterpretConversationText(string toInterpret) {
		if (conversationPartner != null) {
			DialoguePath convoPath = library.ConversationStringToEnum(toInterpret);

			if (convoPath == DialoguePath.End) {
				outputConcatenation += "<i><b>" + conversationPartner.personName + "</b></i>\n" + "Bye.";
				conversationPartner = null;
				prevNode = 0;
				convoPath = DialoguePath.Stay;
				return;
			} else if (convoPath == DialoguePath.Stay) {
				outputConcatenation += "<i><b>" + conversationPartner.personName + "</b></i>\n" + "I don't understand.";
			}

			conversationPartner.conversationTree.SetInteger("Path", (int)convoPath);
		} else {
			conversationPartner = null;
			prevNode = 0;
			return;
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

			if (dialogue.isEndNode) {
				conversationPartner = null;
				prevNode = 0;
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
			if (toInterpret[0] == "/list" || toInterpret[0] == "/commands") {
				outputConcatenation += "List of Commands\n";
				outputConcatenation += "----------------\n";
				outputConcatenation += "/getmethedev\n  Enter Dev Mode\n";
				outputConcatenation += "/additem [item name]\n  Add item to inventory\n";
				outputConcatenation += "/forcetalk [person name]\n  Enter conversation with NPC\n";
				outputConcatenation += "/volume [percentage]\n  Sets the volume percentage of the music\n";
				outputConcatenation += "/clear\n  Clear text\n";
				outputConcatenation += "/exit\n  Exit Dev Mode\n";
			} else if (toInterpret[0] == "/additem") {
				GameObject CheckItem = GetItemByName(toInterpret[1]);
				if (CheckItem != null) {
					player.inventory.Add(CheckItem);
					outputConcatenation += "Adding " + toInterpret[1] + " to inventory.";
				} else {
					outputConcatenation += toInterpret[1] + " is not an item in this game.";
				}
			} else if (toInterpret[0] == "/forcetalk") {
				GameObject CheckPerson = GetPersonByName(toInterpret[1]);
				if (CheckPerson != null) {
					conversationPartner = CheckPerson.GetComponent<TextAdventurePerson>();
					inDevMode = false;
					outputConcatenation += "Talking to " + toInterpret[1];
				} else {
					outputConcatenation += toInterpret[1] + " is not an NPC in game.";
				}
			} else if (toInterpret[0] == "/volume") {
				int volPercentage = System.Int32.Parse(toInterpret[1]);
				if (volPercentage >= 0 && volPercentage <= 100) {
					Camera.main.gameObject.GetComponent<AudioSource>().volume = ((float)volPercentage / 100.0f);
				} else {
					outputConcatenation += "Volume percentage must be between 0 and 100 (inclusive)";
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
}