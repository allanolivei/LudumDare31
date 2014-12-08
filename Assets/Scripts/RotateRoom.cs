using UnityEngine;
using System.Collections;

public class RotateRoom : MonoBehaviour {

	public Transform targetRoom;
	public int initRotation = 0;

    private Quaternion targetRotation;
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
        //StopAllCoroutines();
        //StartCoroutine( "AnimateRotation", Quaternion.Euler(0, 0, initRotation * -90) );
	}

	IEnumerator AnimateRotation( Quaternion targetRotation )
	{
		float initTime = Time.time;
        float duration = 1;
        Quaternion initRotation = transform.rotation;
		while(true)
		{
            float percent = duration == 0 ? 1 : Mathf.Min(1, (Time.time - initTime) / duration);
            transform.rotation = Quaternion.Lerp(initRotation, targetRotation, percent);
			targetRoom.Rotate(0, 0, 9900.8f * Time.deltaTime);
            if (percent == 1)
                break;
            else
			    yield return null;
		}
	}
}
