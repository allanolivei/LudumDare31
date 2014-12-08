using UnityEngine;
using System.Collections;

public class RemoveRotation : MonoBehaviour {
	
	void Update () {
		transform.rotation = Quaternion.Euler(Vector3.zero);
	}
}
