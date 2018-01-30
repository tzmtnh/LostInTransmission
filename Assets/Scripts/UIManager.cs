using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour {

	public static UIManager inst;

	public float aspect = 1.6f;
	public float commandPadding = 0.1f;
	public float distPadding = 0.2f;

	public RectTransform logo;
	public RectTransform commandBox;
	public RectTransform receiver;
	public RectTransform distance;

	Camera _camera;
	Vector2 _logoSize;

	void setX(RectTransform t, float x) {
		Vector2 pos = t.position;
		pos.x = x;
		t.position = pos;
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

		if (Application.isMobilePlatform == false)
			_camera.rect = new Rect(x / w, 0, width / w, 1);

		setX(commandBox, x + width * commandPadding);
		setX(receiver, x + width - width * commandPadding);
		setX(distance, x + width * distPadding);

		logo.sizeDelta = _logoSize * Mathf.Min(1, w / wantedWidth);
	}

	void Awake() {
		Assert.IsNull(inst, "There can be only one!");
		inst = this;

		_camera = Camera.main;
		_logoSize = logo.sizeDelta;
	}

	void Update() {
		updatePositions();
	}
}
