using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Camera _camera;

	void Awake()
	{
		_camera = GetComponent<Camera> ();
	}

	void Update () {
		_camera.orthographicSize = Screen.height / 100 / 2;
		//normal resolution 800x600
		//sprite has 100 pixels per units

		//1 unit == 100 px

		//600 px - 6 units

	}
}
