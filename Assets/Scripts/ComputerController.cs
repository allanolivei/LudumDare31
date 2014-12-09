using UnityEngine;
using System.Collections;

public class ComputerController : MonoBehaviour {

	public Sprite[] gamePieces;
    public int rotateTarget;

	void TurnOnComputer() 
	{
		GetComponent<Animator> ().SetBool ("TurnOn", true);
		//MiniGame.current.SetPieces(gamePieces, transform.position + Vector3.up * 0.9f);
        UIRotateRoom.current.SetComputer(this);
	}

	void TurnOffComputer() 
	{
		GetComponent<Animator> ().SetBool ("TurnOn", false);
        UIRotateRoom.current.SetComputer(null);
		//MiniGame.current.SetPieces(null, Vector3.zero);
	}

}
