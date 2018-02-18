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

	[System.NonSerialized] public GameObject warning;
	[System.NonSerialized] public GameObject critical;

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

	int _touchBaseCharacter;
	public void getLetterFromTouch() {
		InputManager input = InputManager.inst;
		if (input.touchBegan == false) return;

		if (input.touchStartPos.x > 0.6f)
			_currentLetter = 2;
		else if (input.touchStartPos.x > 0.4f)
			_currentLetter = 1;
		else
			_currentLetter = 0;

		findCharacterIndex();
		_touchBaseCharacter = _currentCharacter;
	}

	public void updateCharacterFromTouch() {
		InputManager input = InputManager.inst;
		if (input.touchInProgress == false) return;

		const float SPEED = 20;
		Vector2 delta = input.touchPos - input.touchStartPos;
		_currentCharacter = _touchBaseCharacter;
		updateCharacter(Mathf.FloorToInt(delta.y * SPEED));
	}

	public void updateCharacter(int delta) {
		_currentCharacter += delta;
		_currentCharacter = _currentCharacter % CHARACTERS.Length;
		if (_currentCharacter < 0)
			_currentCharacter += CHARACTERS.Length;
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

	public bool showLeaderboard(string playerUniqueName, string playerInitials, int finalScore, int place, List<dreamloLeaderBoard.Score> scoreList) {
		if (scoreList.Count == 0) {
			_leaderboardIndexs.text = "";
			_leaderboardInitials.text = "";
			_leaderboardScores.text = "";
			return false;
		}

		string indexes = "";
		string initials = "";
		string scores = "";

		const int NUM_ENTRIES = 10;
		string PREFIX = "<color=#" + ColorUtility.ToHtmlStringRGBA(color2) + ">";
		const string SUFFIX = "</color>";

		bool scoreUpdated = true;
		int j = -1;
		for (int i = 0; i < scoreList.Count; i++) {
			dreamloLeaderBoard.Score item = scoreList[i];
			if (GameManager.inst.leaderboardFilter.Length > 0 && item.playerName.StartsWith(GameManager.inst.leaderboardFilter) == false)
				continue;

			j++;
			if (j == NUM_ENTRIES)
				break;

			if (place >= NUM_ENTRIES && j == NUM_ENTRIES - 1) {
				j = place;
				item = scoreList[place];
			}

			string playerName = item.shortText;
			int score = item.score;
			// we make sure we show the new result
			if (j == place && score <= finalScore) {
				scoreUpdated = false;
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

		return scoreUpdated;
	}

	void setX(RectTransform t, float x) {
		Vector2 pos = t.localPosition;
		pos.x = x;
		t.localPosition = pos;
	}

	Vector2 _lastScreenSize;
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

		// crop view so we see black bars on the sides
		if (Application.isMobilePlatform == false) {
			_camera.rect = new Rect(x / w, 0, width / w, 1);
			_uiCamera.rect = _camera.rect;
		}

		const float PAD = 0.08f;
		float scale = 1f / Mathf.Max(0.001f, _canvas.scaleFactor);

		setX(commandBox, -width * scale * (1f - PAD) / 2f);
		setX(receiver, width * scale * (1f - PAD) / 2f);
		setX(distance, -width * scale * (1f - PAD) / 2f);

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

		warning = hudUI.transform.Find("Warning").gameObject;
		critical = hudUI.transform.Find("Critical").gameObject;

		personalBest = gameOverUI.transform.Find("Personal Best").gameObject;
		_gameOverText = gameOverUI.transform.Find("GameOver") as RectTransform;
		_gameOverSize = _gameOverText.sizeDelta;
		gameOverScoreText = gameOverUI.transform.Find("Score").GetComponent<Text>();

		string lastPlayerInitials = PlayerPrefs.GetString("PlayerInitials", "AAA");
		initials = new Text[3];
		initials[0] = gameOverUI.transform.Find("A").GetComponent<Text>();
		initials[1] = gameOverUI.transform.Find("B").GetComponent<Text>();
		initials[2] = gameOverUI.transform.Find("C").GetComponent<Text>();

		for (int i = 0; i < initials.Length; i++) {
			initials[i].text = lastPlayerInitials[i].ToString();
		}

		_leaderboardIndexs = leaderboardUI.transform.Find("Indexes").GetComponent<Text>();
		_leaderboardInitials = leaderboardUI.transform.Find("Initials").GetComponent<Text>();
		_leaderboardScores = leaderboardUI.transform.Find("Scores").GetComponent<Text>();
	}

	void LateUpdate() {
		updatePositions();
		updateInitials();
	}
}
