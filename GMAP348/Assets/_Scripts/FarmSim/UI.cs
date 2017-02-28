using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class UI : MonoBehaviour {
    Project3Player playerScript;
    FirstPersonController fpScript;
    public bool alive;
	// Use this for initialization
	void Start () {
        alive = true;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Project3Player>();
        fpScript = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (alive)
            gameObject.GetComponentsInChildren<Text>()[0].text = "Cash: " + playerScript.inventory.cash;
    }

    public void endScreen()
    {
        fpScript.enabled = false;
        gameObject.GetComponentsInChildren<Text>()[1].text = "You earned " + playerScript.inventory.cash + " money";
        Invoke("reload", 4);
    }

    void reload()
    {
        SceneManager.LoadScene(0);
    }
}
