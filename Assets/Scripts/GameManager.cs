using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public enum GameState { Start, InGame, GameOver, Leaderboar }

    public static GameManager inst;

	public Color color1;
	public Color color2;

	public GameObject startUI;
    public GameObject gameOverUI;
    public GameObject leaderboardUI;
    public GameObject hudUI;

	public Text gameOverScoreText;

	public Text[] initials;

	public Text leaderboardIndexs;
	public Text leaderboardInitials;
	public Text leaderboardScores;

	GameState _state = GameState.Start;
	public GameState state { get { return _state; } }

	bool _isPlayingIntro = false;
	AudioSource _music;

	const string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	int _currentCharacter = 0;
	int _currentLetter = 0;

	dreamloLeaderBoard _leaderboard;
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

	void findCharacterIndex() {
		char c = initials[_currentLetter].text[0];
		for (int i = 0; i < CHARACTERS.Length; i++) {
			if (CHARACTERS[i] == c) {
				_currentCharacter = i;
				return;
			}
		}
		_currentCharacter = 0;
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
				} else if (InputManager.inst.leftClick) {
					if (_currentLetter > 0) {
						_currentLetter--;
						findCharacterIndex();
					}
				} else if (InputManager.inst.rightClick) {
					if (_currentLetter < 2) {
						_currentLetter++;
						findCharacterIndex();
					}
				} else if (InputManager.inst.upClick) {
					_currentCharacter++;
					if (_currentCharacter == CHARACTERS.Length)
						_currentCharacter = 0;
				} else if (InputManager.inst.downClick) {
					_currentCharacter--;
					if (_currentCharacter < 0)
						_currentCharacter = CHARACTERS.Length - 1;
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

		if (_state == GameState.GameOver) {
			bool blink = (Time.time % 1) < 0.5f;
			for (int i = 0; i < initials.Length; i++) {
				if (i == _currentLetter) {
					initials[i].text = CHARACTERS[_currentCharacter].ToString();
					initials[i].color = blink ? color1 : color2;
				} else {
					initials[i].color = color1;
				}
			}
		}

		if (_isPlayingIntro && _music.isPlaying == false) {
			_isPlayingIntro = false;
			switchMusic("Game Music Loop", true);
		}
	}

	int _finalScore = 0;
	void onPlayerDeid() {
		_finalScore = (int)Player.instance.distance;
		_currentLetter = 0;
		_currentCharacter = 0;

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

	string _playerUniqueName;
	public void Submit() {
		string playerInitials = "" + initials[0] + initials[1] + initials[2];
		_playerUniqueName = playerInitials + Random.Range(1, 999999);
		_leaderboard.AddScore(_playerUniqueName, _finalScore);
		StartCoroutine(showLeaderboard());
	}

	IEnumerator showLeaderboard() {
		_scoreList = null;

		leaderboardIndexs.text = "\n\n\nLoading...";
		leaderboardInitials.text = "";
		leaderboardScores.text = "";

		setState(GameState.Leaderboar);

		const float TIME_OUT = 10;
		float time = 0;
		while (_scoreList == null || _scoreList.Count == 0) {
			yield return null;
			_scoreList = _leaderboard.ToListHighToLow();
			time += Time.deltaTime;
			if (time > TIME_OUT) {
				Debug.LogWarning("Time out!");
				break;
			}
		}

		if (_scoreList.Count == 0) {
			leaderboardIndexs.text = "";
			leaderboardInitials.text = "";
			leaderboardScores.text = "";
			yield break;
		}

		const int NUM_ENTRIES = 10;
		int place = -1;
		for (int i = 0; i < _scoreList.Count; i++) {
			dreamloLeaderBoard.Score item = _scoreList[i];
			if (item.playerName.Equals(_playerUniqueName)) {
				place = i;
				break;
			}
		}

		string indexes = "";
		string initials = "";
		string scores = "";

		const string PREFIX = "<color=#00C2A2FF>";
		const string SUFFIX = "</color>";

		int n = Mathf.Min(NUM_ENTRIES, _scoreList.Count);
		if (place >= NUM_ENTRIES) {
			n -= 3;
		}

		for (int i = 0; i < n; i++) {
			dreamloLeaderBoard.Score item = _scoreList[i];

			string playerName = item.playerName;
			if (playerName.Length > 3)
				playerName = playerName.Substring(0, 3);

			int score = item.score;

			string prefix = place == i ? PREFIX : "";
			string suffix = place == i ? SUFFIX : "";

			indexes +=	prefix + (i + 1) +		suffix + "\n";
			initials +=	prefix + playerName +	suffix + "\n";
			scores +=	prefix + score +		suffix + "\n";
		}

		if (place >= NUM_ENTRIES) {
			for (int i = place - 1; i < place + 2; i++) {
				dreamloLeaderBoard.Score item = _scoreList[i];

				string playerName = item.playerName;
				if (playerName.Length > 3)
					playerName = playerName.Substring(0, 3);

				int score = item.score;

				string prefix = place == i ? PREFIX : "";
				string suffix = place == i ? SUFFIX : "";

				indexes +=	prefix + (i + 1) +		suffix + "\n";
				initials += prefix + playerName +	suffix + "\n";
				scores +=	prefix + score +		suffix + "\n";
			}
		}

		leaderboardIndexs.text = indexes;
		leaderboardInitials.text = initials;
		leaderboardScores.text = scores;
	}

	bool _isPaused = false;
	void pause() {
		_isPaused = !_isPaused;
		Time.timeScale = _isPaused ? 0 : 1;
	}
}
