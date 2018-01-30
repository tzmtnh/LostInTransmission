using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixelate : MonoBehaviour {

	public int numHeightPixels = 100;
	public int secondaryHeightPixels = 20;
	public float blendMultiplier = 0.1f;

	public Shader combineShader;

	RenderTexture _resampleRT;
	RenderTexture _secondaryResampleRT;

	Material _combineMaterial;
	int _blendID;
	int _lightSpeedID;

	Vector2Int _screenSize = new Vector2Int();

	void rebuildRT() {
		_screenSize.x = Screen.width;
		_screenSize.y = Screen.height;
		float aspect = Camera.main.aspect;

		if (_resampleRT != null)
			_resampleRT.Release();
		int w = Mathf.CeilToInt(aspect * numHeightPixels);
		w = Mathf.Max(1, w);
		_resampleRT = new RenderTexture(w, numHeightPixels, 0);
		_resampleRT.filterMode = FilterMode.Point;

		if (_secondaryResampleRT != null)
			_secondaryResampleRT.Release();
		int w2 = Mathf.CeilToInt(aspect * secondaryHeightPixels);
		w2 = Mathf.Max(1, w2);
		_secondaryResampleRT = new RenderTexture(w2, secondaryHeightPixels, 0);
		_secondaryResampleRT.filterMode = FilterMode.Point;
		_combineMaterial.SetTexture("_SecondaryTex", _secondaryResampleRT);
	}

	void Awake() {
		_combineMaterial = new Material(combineShader);

		_blendID = Shader.PropertyToID("_Blend");
		_lightSpeedID = Shader.PropertyToID("_LightSpeed");

		rebuildRT();
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (Screen.width != _screenSize.x || Screen.height != _screenSize.y)
			rebuildRT();

		float blend = Player.instance.delay * blendMultiplier;
		_combineMaterial.SetFloat(_blendID, blend);

		float lightSpeed = Player.instance.lightSpeedParam;
		_combineMaterial.SetFloat(_lightSpeedID, lightSpeed);

		Graphics.Blit(source, _resampleRT);
		if (Time.frameCount % 3 == 0)
			Graphics.Blit(source, _secondaryResampleRT);
		Graphics.Blit(_resampleRT, destination, _combineMaterial);
	}
}
