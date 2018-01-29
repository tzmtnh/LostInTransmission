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

	void Awake() {
		int w = Mathf.CeilToInt((float) Screen.width / Screen.height * numHeightPixels);
		_resampleRT = new RenderTexture(w, numHeightPixels, 0);
		_resampleRT.filterMode = FilterMode.Point;

		int w2 = Mathf.CeilToInt((float)Screen.width / Screen.height * secondaryHeightPixels);
		_secondaryResampleRT = new RenderTexture(w2, secondaryHeightPixels, 0);
		_secondaryResampleRT.filterMode = FilterMode.Point;

		_combineMaterial = new Material(combineShader);
		_combineMaterial.SetTexture("_SecondaryTex", _secondaryResampleRT);

		_blendID = Shader.PropertyToID("_Blend");
		_lightSpeedID = Shader.PropertyToID("_LightSpeed");
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		float blend = Player.instance.delay * blendMultiplier;
		_combineMaterial.SetFloat(_blendID, blend);

		float lightSpeed = Player.instance.lightSpeedParam;
		_combineMaterial.SetFloat(_lightSpeedID, lightSpeed);

		Graphics.Blit(source, _resampleRT);
		Graphics.Blit(source, _secondaryResampleRT);
		Graphics.Blit(_resampleRT, destination, _combineMaterial);
	}
}
