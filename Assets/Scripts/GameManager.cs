using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager inst;

	public enum GameState { Start, InGame, GameOver, Leaderboar }
	GameState _state = GameState.Start;
	public GameState state { get { return _state; } }

	bool _isPlayingIntro = false;
	AudioSource _music;

	dreamloLeaderBoard _leaderboard;
	List<dreamloLeaderBoard.Score> _scoreList;

    void switchMusic(string name, bool loop) {
		if (_music != null && _music.isPlaying) {
			AudioManager.inst.stopSound(_music);
		}
		_music = AudioManager.inst.playSound(name, loop: loop);
	}

	void setState(GameState newState) {
		_state = newState;
		UIManager.inst.updateUIVisibility();
	}

	void handleInput() {
		switch (_state) {
			case GameState.Start:
				if (InputManager.inst.select) {
					StartGame();
				} else if (InputManager.inst.back) {
					Application.Quit();
				}
				break;

			case GameState.InGame:
				if (InputManager.inst.select) {
					pause();
				} else if (InputManager.inst.back) {
					mainMenu();
				}
				break;

			case GameState.GameOver:
				if (InputManager.inst.select) {
					Submit();
				} else if (InputManager.inst.left) {
					UIManager.inst.updateLetter(-1);
				} else if (InputManager.inst.right) {
					UIManager.inst.updateLetter(1);
				} else if (InputManager.inst.up) {
					UIManager.inst.updateCharacter(1);
				} else if (InputManager.inst.down) {
					UIManager.inst.updateCharacter(-1);
				} else if (InputManager.inst.back) {
					mainMenu();
				}
				break;

			case GameState.Leaderboar:
				if (InputManager.inst.select) {
					StartGame();
				} else if (InputManager.inst.back) {
					mainMenu();
				}
				break;

			default:
				Debug.LogWarning("Unhandled game state " + _state);
				break;
		}
	}

	void reset() {
		Player.instance.Reset();
		Track.inst.reset();
		_isPaused = false;
	}

	void mainMenu() {
		reset();

		if (_music == null || state == GameState.InGame) {
			_isPlayingIntro = false;
			switchMusic("Main Menu Music", true);
		}

		setState(GameState.Start);
	}

	void Awake()
    {
		UnityEngine.Assertions.Assert.IsNull(inst, "There can be only one!");
		inst = this;

		_leaderboard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		Player.onPlayerDied += onPlayerDeid;
	}

    void Start()
    {
		mainMenu();
	}

    void Update () {
		handleInput();

		if (_isPaused == false && _isPlayingIntro && _music.isPlaying == false) {
			_isPlayingIntro = false;
			switchMusic("Game Music Loop", true);
		}
	}

	int _finalScore = 0;
	int _finalDuraton = 0;
	bool isPersonalBest = false;
	void onPlayerDeid() {
		_finalScore = (int)Player.instance.distance;
		_finalDuraton = (int)Player.instance.duration;

		int bestScore = PlayerPrefs.GetInt("Best_Score", 0);
		isPersonalBest = bestScore < _finalScore;
		UIManager.inst.personalBest.SetActive(isPersonalBest);
		if (isPersonalBest) {
			PlayerPrefs.SetInt("Best_Score", _finalScore);
		}
		Debug.LogFormat("Final score:{0}, best:{1}", _finalScore, bestScore);

		UIManager.inst.gameOverScoreText.text = "Distance: " + _finalScore;
		setState(GameState.GameOver);
		AudioManager.inst.stopAllSounds();
		switchMusic("Main Menu Music", true);
	}

    public void StartGame()
    {
		setState(GameState.InGame);
		reset();
		
		_isPlayingIntro = true;
		switchMusic("Game Music Intro", false);
	}

	string _playerUniqueName;
	string _playerInitials;
	int _playerPlace = -1;
	public void Submit() {
		_playerInitials = UIManager.inst.getInitials();
		_playerUniqueName = SystemInfo.deviceUniqueIdentifier;
		_leaderboard.AddScore(_playerUniqueName, _finalScore, _finalDuraton, _playerInitials);

		UIManager.inst.showLoadingLeaderboards();
		setState(GameState.Leaderboar);

		StartCoroutine(downloadLeaderboard());
	}

	IEnumerator downloadLeaderboard() {
		_scoreList = null;

		const float TIME_OUT = 10;
		float time = 0;
		while (_scoreList == null || _scoreList.Count == 0) {
			_scoreList = _leaderboard.ToListHighToLow();
			time += Time.deltaTime;
			if (time > TIME_OUT) {
				Debug.LogWarning("Time out!");
				break;
			}
			yield return null;
		}

		_playerPlace = findPlayerPlace();
		Debug.LogFormat("Player placed {0} out of {1}", _playerPlace + 1, _scoreList.Count);

		UIManager.inst.showLeaderboard(
			_playerUniqueName,
			_playerInitials,
			_finalScore,
			_playerPlace, 
			_scoreList);
	}

	int findPlayerPlace() {
		for (int i = 0; i < _scoreList.Count; i++) {
			dreamloLeaderBoard.Score item = _scoreList[i];
			if (item.playerName.Equals(_playerUniqueName)) {
				return i;
			}
		}
		return -1;
	}

	bool _isPaused = false;
	void pause() {
		_isPaused = !_isPaused;
		AudioManager.inst.pauseAllSounds(_isPaused);
		UIManager.inst.pause.gameObject.SetActive(_isPaused);
		Time.timeScale = _isPaused ? 0 : 1;
	}
}
