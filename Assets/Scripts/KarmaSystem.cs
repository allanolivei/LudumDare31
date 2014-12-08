using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Player))]
public class KarmaSystem : MonoBehaviour {

	public Vector3 offset;

	private float minRadius = 3.8f;
	private float maxRadius = 8.0f;
	private float radius = 0;
	private float plus = -0.1f;
	private Player _player;
	private SpriteRenderer[] _rend;
	private Transform _transform;

	void Start()
	{
		_player = GetComponent<Player> ();
		_transform = GetComponent<Transform> ();
		_rend = FindObjectsOfType<SpriteRenderer> ();
		List<SpriteRenderer> rendList = new List<SpriteRenderer> ();
		foreach (SpriteRenderer r in _rend)
			if( r.tag != CRef.TAG_PLAYER && r.tag != CRef.TAG_COMPUTER ) rendList.Add( r );
		_rend = rendList.ToArray ();
	}

	void Update()
	{
		AnalysisKarma ();

		foreach (SpriteRenderer r in _rend) 
		{
			if( (r.transform.position - _transform.position + offset).sqrMagnitude < radius * radius )
			{
				Color c = r.color;
				c.a = Mathf.Clamp(c.a + plus * Time.deltaTime, 0.0f, 1.0f);
				r.color = c;
			}
		}
	}

	void AnalysisKarma()
	{
		radius = minRadius + ( _player.abstinence > 0 ? _player.abstinence * (maxRadius-minRadius) : _player.abstinence * (maxRadius-minRadius) * 0.4f );//maxRadius * (_player.abstinence / 100.0f);
		plus = -0.2f * _player.abstinence;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position + offset, maxRadius);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position + offset, radius);
	}

}
