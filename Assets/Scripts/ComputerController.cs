using UnityEngine;
using System.Collections;

public class ComputerController : MonoBehaviour {

	public Sprite[] gamePieces;

	void TurnOnComputer() 
	{
		GetComponent<Animator> ().SetBool ("TurnOn", true);
		MiniGame.current.SetPieces(gamePieces, transform.position + Vector3.up * 0.9f);
	}

	void TurnOffComputer() 
	{
		GetComponent<Animator> ().SetBool ("TurnOn", false);
		MiniGame.current.SetPieces(null, Vector3.zero);
	}

}
