using System.Collections.Generic;
using UnityEngine;

public class Project4GameManager : TextAdventureClass {

	public enum Location {
		Office,
		Admiral,
		Club,
		Slums
	}

	public bool inConversation = false;

	// Use this for initialization
	void Start() {
		Init();
	}
	
	// Update is called once per frame
	void Update() {

	}
}