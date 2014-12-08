using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MiniGame : MonoBehaviour {

	static public MiniGame current;

	//public delegate System.Action MiniGameComplete();
	//public MiniGame
	public Action gameCompleteEvent;

	public Transform[] pieces;
	public Transform targetPosition;

	private int[] game;
	private const float pieceDistance = 32;
	private const int numCols = 3;
	private const int nullPiece = 2;
	private Vector3 offset = new Vector3( -32, -32, 0 );//pieceDistance * -1.5f, pieceDistance * 0.5f, 0 );

	void Awake()
	{
		current = this;

		game = new int[ numCols * numCols ];
		for( int i = 0 ; i < game.Length ; i++ ) game[i] = i;

		//gameObject.SetActive(false);
	}

	public void SetPieces( Sprite[] sp, Vector3 pos )
	{
		if ( sp == null || sp.Length < pieces.Length ) 
		{
			GetComponent<Animator>().SetBool("ShowScreen", false);
			return;
		}

		for( int i = 0 ; i < pieces.Length ; i++ )
		{
			pieces[i].GetComponent<Image>().sprite = sp[i];
		}

		pieces[nullPiece].gameObject.SetActive(false);

		//Vector3 p = Camera.main.WorldToScreenPoint(pos) - new Vector3(Screen.width, Screen.height, 0) * 0.5f;
		//p.z = 0;
		transform.localPosition = Vector3.zero;

		Shuffle();

		GetComponent<Animator>().SetBool("ShowScreen", true);

	}

	public void SwapPiece( int pieceIndex )
	{
		//SwapPiece( piece%numCols, Mathf.FloorToInt(piece/(float)numCols) );
		int piece = System.Array.IndexOf(game, pieceIndex);
		SwapPiece( piece%numCols, Mathf.FloorToInt(piece/(float)numCols) );
	}

	public void SwapPiece( int co, int li )
	{
		int pieceIndex = li * numCols + co;
		int nullPosition = System.Array.IndexOf(game, nullPiece);

		if( nullPosition == pieceIndex || IsComplete() ) return ;//IsComplete();

		int nullCo = (int)(nullPosition%numCols);
		int	nullLi = Mathf.FloorToInt(nullPosition/(float)numCols);

		if( nullCo == co )
		{
			int dir = (int)Mathf.Sign(li-nullLi);
			for( int l = nullLi ; l != li ; l += dir )
			{
				game[ l * numCols + co ] = game[ (l+dir) * numCols + co ];
			}
			game[pieceIndex] = nullPiece;
		}

		if( nullLi == li )
		{
			int dir = (int)Mathf.Sign(co-nullCo);
			for( int c = nullCo ; c != co ; c += dir )
			{
				game[ li * numCols + c ] = game[ li * numCols + (c+dir) ];
			}
			game[pieceIndex] = nullPiece;
		}

		ApplyInView();
		//PrintGrid();
			
		if( IsComplete() && gameCompleteEvent != null ) 
		{
			pieces[nullPiece].gameObject.SetActive(true);
			gameCompleteEvent();
		}

		return;// IsComplete();
	}

	public void Shuffle()
	{
		game = Shuffle<int>(game);
		//game[2] = 1;
		//game[1] = 2;
		ApplyInView();
	}

	public bool IsComplete()
	{
		for( int i = 0 ; i < game.Length ; i++ ) 
		{
			if( game[i] != i ) return false;
		}
		return true;
	}

	public void ApplyInView()
	{
		//PrintGrid();

		for ( int i = 0 ; i < game.Length ; i++ )
		{
			int co = (int)(i%numCols);
			int li = Mathf.FloorToInt(i/(float)numCols);
			pieces[ game[i] ].localPosition = new Vector3( co * pieceDistance, li * pieceDistance, 0 ) + offset;
		}
	}

	void PrintGrid()
	{
		string s = "";
		for ( int i = 0 ; i < game.Length ; i++ )
			s += (game[i] == nullPiece ? "x" : game[i].ToString()) + ( i%numCols == numCols-1 ? "\n" : " " );
		Debug.Log(s);
	}

	public T[] Shuffle<T>(T[] array)
	{
		//Random.
		//var random = _random;
		for (int i = array.Length; i > 1; i--)
		{
			// Pick random element to swap.
			int j = UnityEngine.Random.Range(0, i);//random.Next(i); // 0 <= j <= i-1
			// Swap.
			T tmp = array[j];
			array[j] = array[i - 1];
			array[i - 1] = tmp;
		}
		return array;
	}

}
