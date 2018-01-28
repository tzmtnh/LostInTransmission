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

	private void Awake() {
		_renderer = GetComponent<Renderer>();
	}

	public void destroy() {
		Assert.IsFalse(_destroyed);
		_destroyed = true;
		_renderer.material.color = Color.red;

		if (onHitableHit != null)
			onHitableHit(type);
	}
}
