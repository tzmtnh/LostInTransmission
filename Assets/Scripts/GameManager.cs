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

	bool _isPlayingIntro = false;
	AudioSource _music;

	void switchMusic(string name, bool loop) {
		if (_music != null && _music.isPlaying) {
			AudioManager.inst.stopSound(_music);
		}
		_music = AudioManager.inst.playSound(name, loop: loop);
	}

	void Awake()
    {
        trackObject = GameObject.Find("Track");
        hudObject = GameObject.Find("UI");
		Player.onPlayerDied += onPlayerDeid;
	}

    void Start()
    {
        Player.instance.SetVisible(false);
        trackObject.SetActive(false);
        hudObject.SetActive(false);
		switchMusic("Main Menu Music", true);
	}

    void Update () {
		if (gameOverUI.activeSelf && Input.GetKeyDown(KeyCode.Space)) {
			NewGame();
		}

        if (startUI.activeSelf && Input.GetKeyDown(KeyCode.Space)) {
            StartGame();
        }

		if (_isPlayingIntro && _music.isPlaying == false) {
			_isPlayingIntro = false;
			switchMusic("Game Music Loop", true);
		}
	}

	void onPlayerDeid() {
		gameOverScoreText.text = "Distance: " + (int)Player.instance.distance;
		gameOverUI.SetActive(true);
		hudObject.SetActive(false);
		switchMusic("Main Menu Music", true);
	}

    public void StartGame()
    {
        Player.instance.SetVisible(true);
        Player.instance.Reset();

		trackObject.SetActive(true);
        hudObject.SetActive(true);

        startUI.SetActive(false);

		_isPlayingIntro = true;
		switchMusic("Game Music Intro", false);
	}

    public void NewGame()
    {
        gameOverUI.SetActive(false);
        hudObject.SetActive(true);
        Player.instance.Reset();
		Track.inst.reset();
    }
}
