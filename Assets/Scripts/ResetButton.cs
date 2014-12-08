using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	public void ApplyReset()
	{
		Application.LoadLevel( Application.loadedLevel );
	}
}
