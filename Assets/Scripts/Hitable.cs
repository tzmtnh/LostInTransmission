using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Hitable : MonoBehaviour {

	public static event Action<HitableType> onHitableHit;

	public enum HitableType { Astroid, Jump, Repair, Amplify }

	public HitableType type;

	public bool canDamage { get { return type == HitableType.Astroid; } }

	bool _destroyed = false;
	public bool destroyed { get { return _destroyed; } }

	Renderer _renderer;

	void playSound() {
		switch (type) {
			case HitableType.Astroid:
				if (Player.instance.isAlive) {
					AudioManager.inst.playSound("Astroid Explode");
                    var explosion = GameObject.Instantiate(Track.inst.asteroidExplosionPrefab);
                    explosion.transform.SetParent(gameObject.transform);
                    explosion.transform.localPosition = new Vector3(0, 0, 0);
                    gameObject.GetComponent<Renderer>().enabled = false;
				} else {
					AudioManager.inst.playSound("Ship Explode");
				}
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
		_renderer.material.color = Color.yellow;

		if (onHitableHit != null)
			onHitableHit(type);

		playSound();
	}

	private void Awake() {
		_renderer = GetComponent<Renderer>();
	}
}
