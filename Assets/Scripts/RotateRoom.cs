using UnityEngine;
using System.Collections;

public class RotateRoom : MonoBehaviour {

	public Transform targetRoom;
	public int initRotation = 0;

	private bool playerOn = false;

	void Awake()
	{
		targetRoom.rotation = Quaternion.Euler (0, 0, initRotation * -90);
	}

	void Start()
	{
		MiniGame.current.gameCompleteEvent += MiniGameComplete;
	}

	void MiniGameComplete()
	{
		if ( playerOn )
		{
			Rotate ();
		}
	}

	void Update()
	{
		if ( playerOn && Input.GetKeyDown (KeyCode.P) && transform.eulerAngles.z == 0 ) 
		{
			Rotate ();
		}
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if (other.tag == CRef.TAG_PLAYER)
			playerOn = true;
	}

	void OnTriggerExit2D( Collider2D other )
	{
		if (other.tag == CRef.TAG_PLAYER)
			playerOn = false;
	}

	void Rotate()
	{
		initRotation = (initRotation + 1) % 4;
		targetRoom.rotation = Quaternion.Euler (0, 0, initRotation * -90);
		//StartCoroutine("AnimateRotation");
	}

	/*IEnumerator AnimateRotation()
	{
		float initTime = Time.time;
		while(true)
		{
			targetRoom.Rotate(0, 0, 9900.8f * Time.deltaTime);
			yield return null;
		}
	}*/
}
