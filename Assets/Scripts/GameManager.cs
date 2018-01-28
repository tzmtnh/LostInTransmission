using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject startUI;

    public GameObject gameOverUI;
    public Text gameOverScoreText;
    
    public GameObject trackObject;
    public GameObject hudObject;

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
    }
}
