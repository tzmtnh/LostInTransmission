using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InputManager : MonoBehaviour {

	public static InputManager inst;

	public bool select { get; private set; }
	public bool left { get; private set; }
	public bool right { get; private set; }
	public bool up { get; private set; }
	public bool down { get; private set; }
	public bool back { get; private set; }

	public Vector2 touchStartPos { get; private set; }
	public Vector2 touchPos { get; private set; }
	public bool touchBegan { get; private set; }
	public bool touchInProgress { get; private set; }
	public bool touchEnded { get; private set; }

	void Awake() {
		Assert.IsNull(inst, "There can be only one!");
		inst = this;
	}

	void Update() {
		select = false;
		left = false;
		right = false;
		up = false;
		down = false;
		back = false;

		touchBegan = false;
		touchEnded = false;

		// handle keyboard keys
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
			select = true;
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			left = true;
		if (Input.GetKeyDown(KeyCode.RightArrow))
			right = true;
		if (Input.GetKeyDown(KeyCode.UpArrow))
			up = true;
		if (Input.GetKeyDown(KeyCode.DownArrow))
			down = true;
		if (Input.GetKeyDown(KeyCode.Escape))
			back = true;

		// handle mobile touch
		if (Input.touchCount > 0) {
			Touch touch0 = Input.touches[0];
			Vector2 pos = touch0.position;
			pos.x /= Screen.width;
			pos.y /= Screen.height;
			touchPos = pos;

			switch (touch0.phase) {
				case TouchPhase.Began:
					float x = touch0.position.x / Screen.width;
					const float SIDE_THRESH = 0.3f;

					left = x < SIDE_THRESH;
					right = 1f - x < SIDE_THRESH;
					select = !left && !right;

					touchBegan = true;
					touchInProgress = true;
					touchStartPos = pos;
					break;

				case TouchPhase.Moved:
					break;

				case TouchPhase.Ended:
					touchEnded = true;
					touchInProgress = false;
					break;
			}
		}
	}
}
