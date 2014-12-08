using UnityEngine;
using System.Collections;

public class TriggerPlayAnimation : MonoBehaviour {

	public Animator target;
	public string boolVarName;

	void OnTriggerEnter2D( Collider2D other )
	{
		target.SetBool( boolVarName, true );
	}
}
