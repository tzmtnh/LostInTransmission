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
	public bool debug = false;

	[Header("UI roots")]
	public GameObject startUI;
    public GameObject gameOverUI;
    public GameObject leaderboardUI;
    public GameObject hudUI;
    public GameObject howToPlayUI;
    public GameObject debugUI;

	[System.NonSerialized] public RectTransform commandBox;
	[System.NonSerialized] public RectTransform receiver;
	[System.NonSerialized] public RectTransform distance;
	[System.NonSerialized] public GameObject pause;

	[System.NonSerialized] public Text gameOverScoreText;
	[System.NonSerialized] public GameObject personalBest;
	[System.NonSerialized] public Text[] initials;

	[System.NonSerialized] public GameObject warning;
	[System.NonSerialized] public GameObject critical;
	[System.NonSerialized] public Text powerupText;

	Camera _camera;
	Camera _uiCamera;
	Canvas _canvas;

	Text _leaderboardIndexs;
	Text _leaderboardInitials;
	Text _leaderboardScores;

	RectTransform _logo;
	Vector2 _logoSize;

    RectTransform _howToPlayTouch;
    Vector2 _howToPlayTouchSize;

    RectTransform _howToPlayKeyboard;
    Vector2 _howToPlayKeyboardSize;

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
		leaderboardUI.SetActive(state == GameManager.GameState.Leaderboard);
        howToPlayUI.SetActive(state == GameManager.GameState.HowToPlay);

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
        const float SWIPE_THRESHOLD = .025f;
		Vector2 delta = input.touchPos - input.touchStartPos;
        if (Mathf.Abs(delta.y) > SWIPE_THRESHOLD)
        {
            _currentCharacter = _touchBaseCharacter;
            var swipeDistanceModifier = (delta.y / Mathf.Abs(delta.y)) * SWIPE_THRESHOLD;
            updateCharacter(Mathf.FloorToInt((delta.y - swipeDistanceModifier) * SPEED));
        }
	}

	public void updateCharacter(int delta) {
		_currentCharacter += delta;
		_currentCharacter = _currentCharacter % CHARACTERS.Length;
		if (_currentCharacter < 0)
			_currentCharacter += CHARACTERS.Length;
	}

	public void setLetter(char c) {
		initials[_currentLetter].text = c.ToString();
		_currentLetter = (_currentLetter + 1) % initials.Length;
		findCharacterIndex();
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

	public bool showLeaderboard(string playerUniqueName, string playerInitials, int finalScore, List<dreamloLeaderBoard.Score> scoreList) {
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

		int place = GameManager.inst.findPlayerPlace();
		bool scoreUpdated = place >= 0;
		int n = Mathf.Min(NUM_ENTRIES, scoreList.Count);
		for (int i = 0; i < n; i++) {
			dreamloLeaderBoard.Score item = scoreList[i];

			int j = i;
			if (j == NUM_ENTRIES - 1 && place >= NUM_ENTRIES) {
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

        _howToPlayTouch.sizeDelta = _howToPlayTouchSize * Mathf.Min(1, w / wantedWidth);
        _howToPlayKeyboard.sizeDelta = _howToPlayKeyboardSize * Mathf.Min(1, w / wantedWidth);
    }

	public void showPowerup(string poweupName) {
		powerupText.text = poweupName;
		StartCoroutine(showPowerupCo());
	}

	IEnumerator showPowerupCo() {
		powerupText.gameObject.SetActive(true);
		const float duration = 2;
		float timer = 0;
		Color c0 = Color.white;
		Color c1 = new Color(1, 0.5f, 0);
		while (timer < duration) {
			float t = timer / duration;
			Color c = Color.Lerp(c0, c1, t);
			c.a = 1f - t;
			powerupText.color = c;

			timer += Time.deltaTime;
			yield return null;
		}
		powerupText.gameObject.SetActive(false);
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
		powerupText = hudUI.transform.Find("Powerup").GetComponent<Text>();

		personalBest = gameOverUI.transform.Find("Personal Best").gameObject;
		gameOverScoreText = gameOverUI.transform.Find("Score").GetComponent<Text>();
        
        _howToPlayTouch = howToPlayUI.transform.Find("Touch") as RectTransform;
        _howToPlayTouchSize = _howToPlayTouch.sizeDelta;
        
        _howToPlayKeyboard = howToPlayUI.transform.Find("Keyboard") as RectTransform;
        _howToPlayKeyboardSize = _howToPlayKeyboard.sizeDelta;

        _howToPlayTouch.gameObject.SetActive(Application.isMobilePlatform);
        _howToPlayKeyboard.gameObject.SetActive(!Application.isMobilePlatform);

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

		debugUI.SetActive(debug);
	}

	void LateUpdate() {
		updatePositions();
		updateInitials();
	}

	void OnValidate() {
		debugUI.SetActive(debug);
	}
}
