using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Trail : MonoBehaviour {

	public int historySize = 10;

	Transform _transform;
	LineRenderer _renderer;
	Vector3[] _points;

	void Awake() {
		_transform = transform;
		_points = new Vector3[historySize];

		_renderer = GetComponent<LineRenderer>();
		_renderer.positionCount = historySize;
		_renderer.useWorldSpace = true;

		for (int i = 0; i < historySize; i++) {
			_points[i] = _transform.position;
		}
	}

	void Update() {
		Vector3 offset = new Vector3(0, 0, Time.deltaTime * Player.instance.speed * Random.Range(1f, 2f));
		for (int i = historySize - 1; i >= 2; i--) {
			_points[i] = _points[i - 1] - offset;
		}

		Vector3 pos = _transform.position;
		_points[0] = pos;
		_points[1] = pos - offset;

		_renderer.SetPositions(_points);
	}
}
