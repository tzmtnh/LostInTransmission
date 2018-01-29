using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InputManager : MonoBehaviour {

	public static InputManager inst;

	public bool start { get; private set; }
	public bool left { get; private set; }
	public bool right { get; private set; }

	float _touchMove = 0;

	void Awake() {
		Assert.IsNull(inst, "There can be only one!");
		inst = this;
	}

	void Update() {
		start = false;
		left = false;
		right = false;

		// handle keyboard keys
		if (Input.GetKeyDown(KeyCode.Space))
			start = true;
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			left = true;
		if (Input.GetKeyDown(KeyCode.RightArrow))
			right = true;

		// handle mobile touch
		if (Input.touchCount > 0) {
			Touch touch0 = Input.touches[0];
			const float THRESH = 10;

			switch (touch0.phase) {
				case TouchPhase.Began:
					_touchMove = 0;
					break;
				case TouchPhase.Moved:
					_touchMove += touch0.deltaPosition.x;
					break;
				case TouchPhase.Ended:
					start = Mathf.Abs(_touchMove) < THRESH;
					left = _touchMove < -THRESH;
					right = _touchMove > THRESH;
					break;
			}
		}
	}
}
