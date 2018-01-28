using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject gameOverUI;
    public Text gameOverScoreText;
	
	void Update () {
		if (Player.instance.isAlive == false)
        {
            gameOverScoreText.text = "Distance: " + (int)Player.instance.distance;
            gameOverUI.SetActive(true);
        }
	}

    public void NewGame()
    {
        gameOverUI.SetActive(false);
        Player.instance.Reset();
    }
}
