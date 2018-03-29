using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class QualityControl : MonoBehaviour {

	public enum Quality { Low, High }

	public float fpsSmoothTime = 0.1f;
	public float fpsThresh = 40;
	public float fpsHighThresh = 50;

	PostProcessingBehaviour[] _postProcessing;

	float _fps = 60;
	float _fpsVelocity;

	Quality _quality = Quality.High;
	public Quality quality {
		get { return _quality; }

		set {
			if (_quality == value) return;
			_quality = value;
			qualityChanged();
		}
	}

	Text _debugFPS;
	Text _debugQuality;

	void qualityChanged() {
		foreach (var pp in _postProcessing) {
			pp.enabled = quality == Quality.High;
		}
	}

	void Awake () {
		_postProcessing = FindObjectsOfType<PostProcessingBehaviour>();
	}

	void Start() {
		_debugFPS = UIManager.inst.debugUI.transform.Find("FPS").GetComponent<Text>();
		_debugQuality = UIManager.inst.debugUI.transform.Find("Quality").GetComponent<Text>();
	}

	void Update () {
		float fps = 1f / Time.deltaTime;
		_fps = Mathf.SmoothDamp(_fps, fps, ref _fpsVelocity, fpsSmoothTime);

		if (Time.time > 1 && quality == Quality.High && _fps < fpsThresh) {
			quality = Quality.Low;
		}

		if (UIManager.inst.debug) {
			_debugFPS.text = "FPS: " + Mathf.RoundToInt(_fps);
			_debugQuality.text = "Quality: " + quality.ToString();
		}
	}
}
