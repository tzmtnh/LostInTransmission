using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public enum GameState { Start, InGame, GameOver, Leaderboar }

    public static GameManager inst;

    public GameObject startUI;
    public GameObject gameOverUI;
    public GameObject leaderboardUI;
    public GameObject hudUI;

	public Text gameOverScoreText;

	public Text leaderboardIndexs;
	public Text leaderboardInitials;
	public Text leaderboardScores;

	GameState _state = GameState.Start;
	public GameState state { get { return _state; } }

	bool _isPlayingIntro = false;
	AudioSource _music;

	dreamloLeaderBoard _leaderboard;
	string _playerInitials = "XYZ";
	List<dreamloLeaderBoard.Score> _scoreList;

	public bool isOnTitleScreen { get { return startUI.activeSelf; } }

    void switchMusic(string name, bool loop) {
		if (_music != null && _music.isPlaying) {
			AudioManager.inst.stopSound(_music);
		}
		_music = AudioManager.inst.playSound(name, loop: loop);
	}

	void setState(GameState newState) {
		_state = newState;
		startUI.SetActive(_state == GameState.Start);
		hudUI.SetActive(_state == GameState.InGame);
		gameOverUI.SetActive(_state == GameState.GameOver);
		leaderboardUI.SetActive(_state == GameState.Leaderboar);
	}

	void handleInput() {
		switch (_state) {
			case GameState.Start:
				if (InputManager.inst.middleClick) {
					StartGame();
				}
				break;

			case GameState.InGame:
				if (InputManager.inst.middleClick) {
					pause();
				}
				break;

			case GameState.GameOver:
				if (InputManager.inst.middleClick) {
					Submit();
				}
				break;

			case GameState.Leaderboar:
				if (InputManager.inst.middleClick) {
					NewGame();
				}
				break;
			default:
				break;
		}
	}

	void Awake()
    {
        inst = this;

		_leaderboard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		_scoreList = _leaderboard.ToListHighToLow();

		Player.onPlayerDied += onPlayerDeid;
	}

    void Start()
    {
        Track.inst.spawnEnabled = false;
		switchMusic("Main Menu Music", true);
		setState(GameState.Start);
	}

    void Update () {
		handleInput();

		if (_isPlayingIntro && _music.isPlaying == false) {
			_isPlayingIntro = false;
			switchMusic("Game Music Loop", true);
		}
	}

	int _finalScore = 0;
	void onPlayerDeid() {
		_finalScore = (int)Player.instance.distance;
		_scoreList = _leaderboard.ToListHighToLow();

		gameOverScoreText.text = "Distance: " + _finalScore;
		setState(GameState.GameOver);
		switchMusic("Main Menu Music", true);
	}

    public void StartGame()
    {
		setState(GameState.InGame);

		Player.instance.Reset();
        Track.inst.spawnEnabled = true;

		_isPlayingIntro = true;
		switchMusic("Game Music Intro", false);
	}

    public void NewGame()
    {
		setState(GameState.InGame);

        Player.instance.Reset();
		Track.inst.reset();

		switchMusic("Game Music Intro", false);
	}

	public void Submit() {
		string playerName = _playerInitials + Random.Range(1, 999999);
		_leaderboard.AddScore(playerName, _finalScore);

		StartCoroutine(showLeaderboard());
	}

	IEnumerator showLeaderboard() {
		_scoreList = _leaderboard.ToListHighToLow();

		while (_scoreList == null) {
			yield return null;
		}

		string indexes = "";
		string initials = "";
		string scores = "";

		int n = Mathf.Min(10, _scoreList.Count);
		for (int i = 0; i < n; i++) {
			dreamloLeaderBoard.Score item = _scoreList[i];

			string playerName = item.playerName;
			if (playerName.Length > 3)
				playerName = playerName.Substring(0, 3);

			int score = item.score;

			indexes += (i + 1) + "\n";
			initials += playerName + "\n";
			scores += score + "\n";
		}

		leaderboardIndexs.text = indexes;
		leaderboardInitials.text = initials;
		leaderboardScores.text = scores;

		setState(GameState.Leaderboar);
	}

	bool _isPaused = false;
	void pause() {
		_isPaused = !_isPaused;
		Time.timeScale = _isPaused ? 0 : 1;
	}
}
