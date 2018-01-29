using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InputManager : MonoBehaviour {

	public static InputManager inst;

	public bool middleClick { get; private set; }
	public bool leftClick { get; private set; }
	public bool rightClick { get; private set; }

	void Awake() {
		Assert.IsNull(inst, "There can be only one!");
		inst = this;
	}

	void Update() {
		middleClick = false;
		leftClick = false;
		rightClick = false;

		// handle keyboard keys
		if (Input.GetKeyDown(KeyCode.Space))
			middleClick = true;
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			leftClick = true;
		if (Input.GetKeyDown(KeyCode.RightArrow))
			rightClick = true;

		// handle mobile touch
		if (Input.touchCount > 0) {
			Touch touch0 = Input.touches[0];

			if (touch0.phase == TouchPhase.Began) {
				float x = touch0.position.x / Screen.width;
				const float SIDE_THRESH = 0.3f;

				leftClick = x < SIDE_THRESH;
				rightClick = 1f - x < SIDE_THRESH;
				middleClick = !leftClick && !rightClick;
			}
		}
	}
}
