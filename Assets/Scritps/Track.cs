using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

	public float speed = 1;

	public Material lanesMaterial;

	float traveledDistance = 0;

	private void Update() {
		if (lanesMaterial == null)
			return;

		float dt = Time.deltaTime;
		traveledDistance += speed * dt;

		lanesMaterial.mainTextureOffset = new Vector2(0, -traveledDistance);
	}

}
