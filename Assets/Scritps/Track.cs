using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Track : MonoBehaviour {

	public float speed = 1;

	Transform _lane1;
	Transform _lane2;
	float _laneLength;

	float traveledDistance = 0;

	private void Awake() {
		_lane1 = transform.Find("Lanes");
		Assert.IsNotNull(_lane1);
		_lane1.name = "Lanes 1";
		_laneLength = _lane1.localScale.y;

		_lane2 = Instantiate(_lane1, transform);
		_lane2.name = "Lanes 2";
		_lane2.localPosition += new Vector3(0, 0, _laneLength);
	}

	private void Update() {
		float dt = Time.deltaTime;
		traveledDistance += speed * dt;

		Vector3 pos1 = _lane1.localPosition;
		pos1.z = - (traveledDistance % _laneLength);
		//Debug.Log(pos1.z + " -> " + traveledDistance);
		if (-pos1.z > _laneLength) {
			//swap lanes
			Transform temp = _lane1;
			_lane1 = _lane2;
			_lane2 = temp;
			pos1.z += _laneLength;
		}

		_lane1.localPosition = pos1;

		Vector3 pos2 = pos1;
		pos2.z += _laneLength;
		_lane2.localPosition = pos2;
	}

}
