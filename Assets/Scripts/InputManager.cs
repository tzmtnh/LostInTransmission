using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InputManager : MonoBehaviour {

	public static InputManager inst;

	public bool start { get; private set; }
	public bool left { get; private set; }
	public bool right { get; private set; }

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

			if (touch0.phase == TouchPhase.Began) {
				float posX = touch0.position.x / Screen.width;
				start = true;
				left = posX < 0.5f;
				right = !left;
			}
		}
	}
}
