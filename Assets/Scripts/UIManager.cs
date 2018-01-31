using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager inst;

	[Header("General")]
	public float aspect = 1.6f;
	public float commandPadding = 0.1f;
	public float distPadding = 0.2f;

	public Color color1;
	public Color color2;

	[Header("UI roots")]
	public GameObject startUI;
    public GameObject gameOverUI;
    public GameObject leaderboardUI;
    public GameObject hudUI;

	[System.NonSerialized] public RectTransform commandBox;
	[System.NonSerialized] public RectTransform receiver;
	[System.NonSerialized] public RectTransform distance;
	[System.NonSerialized] public GameObject pause;

	[System.NonSerialized] public Text gameOverScoreText;
	[System.NonSerialized] public GameObject personalBest;
	[System.NonSerialized] public Text[] initials;

	Camera _camera;
	Camera _uiCamera;
	Canvas _canvas;

	Text _leaderboardIndexs;
	Text _leaderboardInitials;
	Text _leaderboardScores;

	RectTransform _logo;
	Vector2 _logoSize;

	RectTransform _gameOverText;
	Vector2 _gameOverSize;

	const string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	int _currentCharacter = 0;
	int _currentLetter = 0;

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

	public void updateUIVisibility() {
		GameManager.GameState state = GameManager.inst.state;
		startUI.SetActive(state == GameManager.GameState.Start);
		hudUI.SetActive(state == GameManager.GameState.InGame);
		gameOverUI.SetActive(state == GameManager.GameState.GameOver);
		leaderboardUI.SetActive(state == GameManager.GameState.Leaderboar);

		if (gameOverUI.activeSelf) {
			_currentLetter = 0;
			findCharacterIndex();
		}
	}

	public void updateLetter(int delta) {
		_currentLetter += delta;
		_currentLetter = Mathf.Clamp(_currentLetter, 0, initials.Length - 1);
		findCharacterIndex();
	}

	public void updateCharacter(int delta) {
		_currentCharacter += delta;
		if (_currentCharacter == CHARACTERS.Length)
			_currentCharacter = 0;
		else if (_currentCharacter < 0)
			_currentCharacter = CHARACTERS.Length - 1;
	}

	public void updateInitials() {
		if (GameManager.inst.state != GameManager.GameState.GameOver)
			return;

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

	public string getInitials() {
		return initials[0].text + initials[1].text + initials[2].text;
	}

	public void showLoadingLeaderboards() {
		_leaderboardIndexs.text = "\n\n\nLoading...";
		_leaderboardInitials.text = "";
		_leaderboardScores.text = "";
	}

	public void showLeaderboard(string playerUniqueName, string playerInitials, int finalScore, int place, List<dreamloLeaderBoard.Score> scoreList) {
		if (scoreList.Count == 0) {
			_leaderboardIndexs.text = "";
			_leaderboardInitials.text = "";
			_leaderboardScores.text = "";
			return;
		}

		string indexes = "";
		string initials = "";
		string scores = "";

		const int NUM_ENTRIES = 10;
		string PREFIX = "<color=#" + ColorUtility.ToHtmlStringRGBA(color2) + ">";
		const string SUFFIX = "</color>";

		int n = Mathf.Min(NUM_ENTRIES, scoreList.Count);
		for (int i = 0; i < n; i++) {
			int j = i;
			if (place >= NUM_ENTRIES && i == NUM_ENTRIES - 1) {
				j = place;
			}

			dreamloLeaderBoard.Score item = scoreList[j];

			string playerName = item.shortText;
			int score = item.score;
			// we make sure we show the new result
			if (j == place && score < finalScore) {
				score = finalScore;
				playerName = playerInitials;
			}

			string prefix = place == j ? PREFIX : "";
			string suffix = place == j ? SUFFIX : "";

			indexes += prefix + (j + 1) + suffix + "\n";
			initials += prefix + playerName + suffix + "\n";
			scores += prefix + score + suffix + "\n";
		}

		_leaderboardIndexs.text = indexes;
		_leaderboardInitials.text = initials;
		_leaderboardScores.text = scores;
	}

	void setX(RectTransform t, float x) {
		Vector2 pos = t.localPosition;
		pos.x = x;
		t.localPosition = pos;
	}

	Vector2 _lastScreenSize;
	float _commandBoxX;
	float _receiverX;
	float _distanceX;
	void updatePositions() {
		float w = Screen.width;
		float h = Screen.height;
		if (_lastScreenSize.x == w && _lastScreenSize.y == h)
			return;
		_lastScreenSize = new Vector2(w, h);

		float a = h / w;
		float wantedWidth = w * a / aspect;
		float width = Mathf.Min(w, wantedWidth);
		float x = (w - width) / 2f;

		if (Application.isMobilePlatform == false) {
			_camera.rect = new Rect(x / w, 0, width / w, 1);
			_uiCamera.rect = _camera.rect;
		}

		float scale = 1f / _canvas.scaleFactor;
		setX(commandBox, width * scale * _commandBoxX / wantedWidth);
		setX(receiver, width * scale * _receiverX / wantedWidth);
		setX(distance, width * scale * _distanceX / wantedWidth);

		_logo.sizeDelta = _logoSize * Mathf.Min(1, w / wantedWidth);
		_gameOverText.sizeDelta = new Vector2(_gameOverSize.x * Mathf.Min(1, w / wantedWidth), _gameOverSize.y);
	}

	void Awake() {
		Assert.IsNull(inst, "There can be only one!");
		inst = this;

		_camera = Camera.main;
		_uiCamera = _camera.transform.parent.Find("UI Camera").GetComponent<Camera>();
		_canvas = GetComponent<Canvas>();

		_logo = startUI.transform.Find("Logo") as RectTransform;
		_logoSize = _logo.sizeDelta;

		commandBox = hudUI.transform.Find("CommandBox") as RectTransform;
		receiver = hudUI.transform.Find("Receiver") as RectTransform;
		distance = hudUI.transform.Find("Distance") as RectTransform;
		pause = hudUI.transform.Find("Pause").gameObject;

		_commandBoxX = commandBox.localPosition.x;
		_receiverX = receiver.localPosition.x;
		_distanceX = distance.localPosition.x;

		personalBest = gameOverUI.transform.Find("Personal Best").gameObject;
		_gameOverText = gameOverUI.transform.Find("GameOver") as RectTransform;
		_gameOverSize = _gameOverText.sizeDelta;
		gameOverScoreText = gameOverUI.transform.Find("Score").GetComponent<Text>();

		initials = new Text[3];
		initials[0] = gameOverUI.transform.Find("A").GetComponent<Text>();
		initials[1] = gameOverUI.transform.Find("B").GetComponent<Text>();
		initials[2] = gameOverUI.transform.Find("C").GetComponent<Text>();

		_leaderboardIndexs = leaderboardUI.transform.Find("Indexes").GetComponent<Text>();
		_leaderboardInitials = leaderboardUI.transform.Find("Initials").GetComponent<Text>();
		_leaderboardScores = leaderboardUI.transform.Find("Scores").GetComponent<Text>();
	}

	void Update() {
		updatePositions();
		updateInitials();
	}
}
