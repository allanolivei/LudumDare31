using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpeak : MonoBehaviour {

	public Sprite[] msg;
	private float cooldown = 4;
	private float delay = 3;

	private float _previousTimeMessage;
	private Player _player;
	private SpriteRenderer _sprite;

	void Start()
	{
		_player = FindObjectOfType<Player> ();
		_sprite = GetComponent<SpriteRenderer> ();
		_previousTimeMessage = Time.time - (cooldown + delay);
	}

	void Update()
	{
		transform.rotation = Quaternion.identity;

		Color c = _sprite.color;
		c.a = (Time.time - _previousTimeMessage <= delay ? 1 : 0);
		_sprite.color = c;

		if (Time.time - _previousTimeMessage <= cooldown + delay) return;

		if (_player.abstinence > 0)
			ShowMessage ( _player.currentState.Equals(Player.STATES.SIT) ? 1 : Random.Range(0,2) );
		else if( _player.abstinence < 0 && _player.currentState.Equals(Player.STATES.SIT) )
			ShowMessage (2);
	}

	void ShowMessage(int m)
	{
		_sprite.sprite = msg[m];
		_previousTimeMessage = Time.time;
	}

}
