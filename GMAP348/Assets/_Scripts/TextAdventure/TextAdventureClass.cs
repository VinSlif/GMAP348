using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAdventureClass : WordLibrary {

	public enum Directions {
		North,
		East,
		South,
		West
	}

	[Header("Handles Input")]
	public InputField playerInput;
	public List<GameObject> inventory;

	private ActionType currAction;
	private InputField.SubmitEvent se;

	[Header("Handles Output")]
	public Text output;

	private string wordConstruction = "";

	// Handles location variables
	private Animator locations;
	private List<GameObject> locItems;

	/// <summary>
	/// Used to initialize variables and
	/// set up the input field listener.
	/// </summary>
	public void Init() {
		se = new InputField.SubmitEvent();
		se.AddListener(SubmitInput);
		playerInput.onEndEdit = se;

		locations = GetComponent<Animator>();
	}

	/// <summary>
	/// Used to get and interpret the input field.
	/// </summary>
	/// <param name="newText">Text submitted in input field.</param>
	void SubmitInput(string newText) {
		// Get input
		string playerText = newText.ToLower();
		InterpretText(playerText);
		playerInput.text = "";

		// Disply output
		DisplayOutput(newText, wordConstruction);
		playerInput.ActivateInputField();
	}


	void DisplayOutput(string playerInput, string toDis) {
		output.text += "<i><size=24>" + playerInput + "</size></i>\n" + toDis + "\n";
	}

	public void InterpretText(string toInterpret) {
		string[] pInput = toInterpret.Split(' ');

		wordConstruction = "";

		currAction = ActionStringToEnum(pInput[0]);

		if (currAction != ActionType.NotAction) {
			switch(currAction) {
			case ActionType.Examine:
				break;
			case ActionType.PickUp:
				break;
			case ActionType.Inventory:
				break;
			case ActionType.Converse:
				break;
			case ActionType.Attack:
				break;
			default:
				break;
			}
		} else {
			wordConstruction = "\"" + pInput[0] + "\" is not something I know how to do.";
		}
	}

	public void GetItems() {
		locItems = locations.GetComponent<LocationManager>().items;
	}
}