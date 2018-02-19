using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager inst;

	public enum GameState { Start, InGame, GameOver, Leaderboar }
	GameState _state = GameState.Start;
	public GameState state { get { return _state; } }

	[Tooltip("Leave empty to not have a filter")]
	public string leaderboardFilter = "";

	AudioSource _music = null;
	Coroutine _playLoopCo = null;

	dreamloLeaderBoard _leaderboard;
	List<dreamloLeaderBoard.Score> _scoreList;

    void switchMusic(string name, bool loop) {
		if (_music != null) {
			AudioManager.inst.stopSound(_music);
		}
		_music = AudioManager.inst.playSound(name, loop: loop);
	}

	void setState(GameState newState) {
		_state = newState;
		UIManager.inst.updateUIVisibility();
	}

	void handleInput() {
		InputManager input = InputManager.inst;
		UIManager ui = UIManager.inst;

		switch (_state) {
			case GameState.Start:
				if (input.select) {
					StartGame();
				} else if (input.back) {
					Application.Quit();
				}
				break;

			case GameState.InGame:
				if (input.select) {
					pause();
				} else if (input.back) {
					if (_isPaused)
						mainMenu();
					else
						pause();
				}

				if (_isPaused) {
					if (input.left || input.right) {
						pause();
					}
				}

				break;

			case GameState.GameOver:
				if (Application.isMobilePlatform) {
					if (input.touchBegan) {
						ui.getLetterFromTouch();
					} else if (input.touchInProgress) {
						ui.updateCharacterFromTouch();
					}
				} else {
					if (input.select) {
						Submit();
					} else if (input.left) {
						ui.updateLetter(-1);
					} else if (input.right) {
						ui.updateLetter(1);
					} else if (input.up) {
						ui.updateCharacter(1);
					} else if (input.down) {
						ui.updateCharacter(-1);
					} else if (input.back) {
						mainMenu();
					}
				}
				break;

			case GameState.Leaderboar:
				if (input.select) {
					StartGame();
				} else if (input.back) {
					mainMenu();
				}
				break;

			default:
				Debug.LogWarning("Unhandled game state " + _state);
				break;
		}
	}

	void reset() {
		if (_isPaused)
			pause();

		Player.instance.Reset();
		Track.inst.reset();

		if (_playLoopCo != null) {
			StopCoroutine(_playLoopCo);
			AudioManager.inst.stopSound(_music);
			_music = null;
		}
	}

	void mainMenu() {
		reset();
		switchMusic("Main Menu Music", true);
		setState(GameState.Start);
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
		switchMusic("Main Menu Music", true);
	}

    public void StartGame() {
		setState(GameState.InGame);
		reset();
		_playLoopCo = StartCoroutine(playGameMusicCo());
	}

	IEnumerator playGameMusicCo() {
		switchMusic("Game Music Intro", false);
		yield return new WaitForSeconds(_music.clip.length);
		switchMusic("Game Music Loop", true);
		_playLoopCo = null;
	}

	string _playerUniqueName;
	string _playerInitials;
	public void Submit() {
		_playerInitials = UIManager.inst.getInitials();
		PlayerPrefs.SetString("PlayerInitials", _playerInitials);

		//_playerUniqueName = SystemInfo.deviceUniqueIdentifier;
		// this doesn't work in some case
		// for example, in WebGL it generates NA as the identifier
		// this is why we do this
		if (PlayerPrefs.HasKey("UniqueIdentifier")) {
			_playerUniqueName = PlayerPrefs.GetString("UniqueIdentifier");
		} else {
			_playerUniqueName = System.Guid.NewGuid().ToString();
			PlayerPrefs.SetString("UniqueIdentifier", _playerUniqueName);
		}

		if (leaderboardFilter.Length > 0) {
			_playerUniqueName = leaderboardFilter + "_" + _playerInitials + "_" + _playerUniqueName;
		}
		_leaderboard.AddScore(_playerUniqueName, _finalScore, _finalDuraton, _playerInitials);

		UIManager.inst.showLoadingLeaderboards();
		setState(GameState.Leaderboar);

		StartCoroutine(updateLeaderboardCo());
	}

	IEnumerator updateLeaderboardCo() {
		_scoreList = null;

		WaitForSeconds waitForSec = new WaitForSeconds(0.1f);
		bool scoreUpdated = false;
		while (state == GameState.Leaderboar && scoreUpdated == false) {
			_scoreList = _leaderboard.ToListHighToLow();

			if (_scoreList != null) {
				if (leaderboardFilter.Length > 0) {
					string filter = leaderboardFilter + "_";
					for (int i = _scoreList.Count - 1; i >= 0; i--) {
						if (_scoreList[i].playerName.StartsWith(filter) == false) {
							_scoreList.RemoveAt(i);
						}
					}
				}

				scoreUpdated = UIManager.inst.showLeaderboard(
					_playerUniqueName,
					_playerInitials,
					_finalScore,
					_scoreList);
			}

			yield return waitForSec;
		}
	}

	public int findPlayerPlace() {
		for (int i = 0; i < _scoreList.Count; i++) {
			dreamloLeaderBoard.Score item = _scoreList[i];
			if (item.playerName.Equals(_playerUniqueName)) {
				return i;
			}
		}
		return -1;
	}

	AudioSource _damageSource = null;
	void updateWarnings() {
		bool isDamaged = true;
		int health = Player.instance.currentHealth;

		if (Player.instance.isAlive == false)
			isDamaged = false;
		else if (health == Player.instance.maxHealth)
			isDamaged = false;

		bool showWarning = false;
		bool showCritical = false;

		if (isDamaged) {
			if (health == 1) {
				const float FREQUENCY = 0.5f;
				showCritical = Time.time / FREQUENCY % 1f > 0.5f;
			} else {
				const float FREQUENCY = 1f;
				showWarning = Time.time / FREQUENCY % 1f > 0.5f;
			}

			if (showCritical && _damageSource == null) {
				_damageSource = AudioManager.inst.playSound("Damage", loop: true);
				_damageSource.volume = 0.3f;
				_damageSource.pitch = 2;
			}
		}

		if (showCritical == false && _damageSource != null) {
			AudioManager.inst.stopSound(_damageSource);
			_damageSource = null;
		}

		if (UIManager.inst.warning.activeSelf != showWarning)
			UIManager.inst.warning.SetActive(showWarning);
		if (UIManager.inst.critical.activeSelf != showCritical)
			UIManager.inst.critical.SetActive(showCritical);
	}

	bool _isPaused = false;
	void pause() {
		_isPaused = !_isPaused;
		AudioManager.inst.pauseAllSounds(_isPaused);
		UIManager.inst.pause.gameObject.SetActive(_isPaused);
		Time.timeScale = _isPaused ? 0 : 1;
	}

	void Awake() {
		UnityEngine.Assertions.Assert.IsNull(inst, "There can be only one!");
		inst = this;

		_leaderboard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		Player.onPlayerDied += onPlayerDeid;
	}

	void Start() {
		mainMenu();
	}

	void Update() {
		handleInput();
		updateWarnings();
	}
}
