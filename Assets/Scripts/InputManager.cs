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

			if (touch0.phase == TouchPhase.Began) {
				float x = touch0.position.x / Screen.width;
				const float SIDE_THRESH = 0.3f;

				left = x < SIDE_THRESH;
				right = 1f - x < SIDE_THRESH;
				select = !left && !right;
			}
		}
	}
}
