using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour {

	public static CameraControl inst;

	Transform _transform;
	Camera _camera;

	Vector3 _origin;
	float _defaultFOV;

	Coroutine _co = null;

	public void shake(float duration) {
		if (_co != null) {
			StopCoroutine(_co);
		}
		StartCoroutine(shakeCo(duration));
	}

	IEnumerator shakeCo(float duration) {
		const float strength = 0.1f;
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
	}

	void Update() {
		const float maxDist = 4f;
		float speed = Player.instance.normalizedSpeed;
		float offset = Mathf.Clamp((speed - 1f) / 1.3f, 0, maxDist);
		Vector3 pos = _origin + _transform.forward * offset;
		float alpha = Mathf.Deg2Rad * _defaultFOV / 2f;
		float tanBeta = maxDist * Mathf.Tan(alpha) / (maxDist - offset);
		float beta = Mathf.Atan(tanBeta);
		_transform.localPosition = pos;
		_camera.fieldOfView = beta * Mathf.Rad2Deg * 2f;
	}
}
