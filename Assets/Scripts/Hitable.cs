using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Hitable : MonoBehaviour {

	public static event Action<HitableType> onHitableHit;

	public enum HitableType { Astroid, Jump, Repair, Amplify }

	public HitableType type;

	bool _destroyed = false;
	public bool destroyed { get { return _destroyed; } }

	Renderer _renderer;

	void playSound() {
		switch (type) {
			case HitableType.Astroid:
				AudioManager.inst.playSound("Astroid Explode");
				break;
			case HitableType.Jump:
				AudioManager.inst.playSound("Jump");
				break;
			case HitableType.Repair:
				AudioManager.inst.playSound("Repair");
				break;
			case HitableType.Amplify:
				AudioManager.inst.playSound("Amplify");
				break;
			default:
				Debug.LogWarning("Unimplemented hitable sfx");
				break;
		}
	}

	public void destroy() {
		Assert.IsFalse(_destroyed);
		_destroyed = true;
		_renderer.material.color = Color.red;

		playSound();

		if (onHitableHit != null)
			onHitableHit(type);
	}

	private void Awake() {
		_renderer = GetComponent<Renderer>();
	}
}
