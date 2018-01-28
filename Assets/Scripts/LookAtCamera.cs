using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	Transform _camera;
	Transform _transform;

	void Start () {
		_camera = Camera.main.transform;
		_transform = transform;
	}
	
	void Update () {
		_transform.rotation = Quaternion.LookRotation(_camera.position - _transform.position, _camera.up);
	}
}
