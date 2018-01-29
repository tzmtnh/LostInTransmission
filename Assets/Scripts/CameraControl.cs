using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour {

	public static CameraControl inst;

	private GameObject _warning;
	private GameObject _critical;

	Transform _transform;
	Camera _camera;

	Vector3 _origin;
	float _defaultFOV;

	Coroutine _co = null;

	public void shake(float duration) {
		if (_co != null) {
			StopCoroutine(_co);
		}
		_co = StartCoroutine(shakeCo(duration));
	}

	IEnumerator shakeCo(float duration) {
		const float strength = 0.2f;
		float timer = 0;
		while (timer < duration) {
			float t = timer / duration;

			Vector3 pos = _origin;
			float s = strength * (1f - Mathf.Cos(Mathf.PI * t)) / 2f;
			pos += Random.insideUnitSphere * s;
			_transform.localPosition = pos;

			yield return null;
			timer += Time.deltaTime;
		}

		_transform.localPosition = _origin;
		_co = null;
	}

	void Awake() {
		Assert.IsNull(inst, "There can be only one!");
		inst = this;
		_transform = transform;
		_camera = GetComponent<Camera>();

		_origin = _transform.localPosition;
		_defaultFOV = _camera.fieldOfView;

        _warning = GameObject.Find("Warning");
        _critical = GameObject.Find("Critical");
	}

	void updateFOV() {
		const float maxDist = 4f;
		float speed = Player.instance.normalizedSpeed;
		float offset = Mathf.Clamp((speed - 1f) / 1.3f, 0, maxDist);
		float alpha = Mathf.Deg2Rad * _defaultFOV / 2f;
		float tanBeta = maxDist * Mathf.Tan(alpha) / (maxDist - offset);
		float beta = Mathf.Atan(tanBeta);

		_transform.localPosition = _origin + _transform.forward * (offset * 1.2f);
		_camera.fieldOfView = beta * Mathf.Rad2Deg * 2f;
	}

	void updateWarnings() {
		_warning.SetActive(false);
		_critical.SetActive(false);

		if (Player.instance.isAlive == false) return;
		int health = Player.instance.currentHealth;
		if (health == Player.instance.maxHealth) return;

		if (health == 1) {
			const float FREQUENCY = 0.5f;
			_critical.SetActive(Time.time / FREQUENCY % 1f > 0.5f); 
		} else {
			const float FREQUENCY = 1f;
			_warning.SetActive(Time.time / FREQUENCY % 1f > 0.5f);
		}
	}

	void Update() {
		updateFOV();
		updateWarnings();
	}
}
