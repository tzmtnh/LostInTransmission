using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject startUI;

    public GameObject gameOverUI;
    public Text gameOverScoreText;
    
    private GameObject trackObject;
    private GameObject hudObject;

    void Awake()
    {
        trackObject = GameObject.Find("Track");
        hudObject = GameObject.Find("UI");
    }

    void Start()
    {
        Player.instance.SetVisible(false);
        trackObject.SetActive(false);
        hudObject.SetActive(false);
    }

    void Update () {
		if (Player.instance.isAlive == false)
        {
            gameOverScoreText.text = "Distance: " + (int)Player.instance.distance;
            gameOverUI.SetActive(true);

			if (Input.GetKeyDown(KeyCode.Space)) {
				NewGame();
			}
        }

        if (startUI.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
	}

    public void StartGame()
    {
        Player.instance.SetVisible(true);
        Player.instance.Reset();

        trackObject.SetActive(true);
        hudObject.SetActive(true);

        startUI.SetActive(false);
    }

    public void NewGame()
    {
        gameOverUI.SetActive(false);
        Player.instance.Reset();
		Track.inst.reset();
    }
}
