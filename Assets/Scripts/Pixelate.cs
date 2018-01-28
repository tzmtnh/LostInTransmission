using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixelate : MonoBehaviour {

	public int numHeightPixels = 100;
	RenderTexture _resampleRT;

	void Awake() {
		int w = Mathf.CeilToInt((float) Screen.width / Screen.height * numHeightPixels);
		_resampleRT = new RenderTexture(w, numHeightPixels, 0);
		_resampleRT.filterMode = FilterMode.Point;
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit(source, _resampleRT);
		Graphics.Blit(_resampleRT, destination);
	}
}
