using UnityEngine;
using System.Collections;

public class InitTip : MonoBehaviour {


	private KeyCode[] unblockKeys = 
		new KeyCode[]{
		KeyCode.A, 
		KeyCode.S,
		KeyCode.W,
		KeyCode.D,
		KeyCode.Escape,
		KeyCode.Space,
		KeyCode.End,
		KeyCode.KeypadEnter
	};

	void Start()
	{
		Time.timeScale = 0;
	}

	void OnGUI()
	{
		if ( Event.current.isKey )
		{
			if ( System.Array.IndexOf( unblockKeys, Event.current.keyCode ) != -1 )
			{
				Time.timeScale = 1;
				GameObject.Destroy(gameObject);

			}
		}
	}
}
