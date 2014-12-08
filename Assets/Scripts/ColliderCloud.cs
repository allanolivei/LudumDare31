using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ColliderCloud : MonoBehaviour {
	
	static private List<Collider2D> clouds;
	
	public LayerMask mask = -1;
	public Transform _transform;
	public Rigidbody2D _rigidbody2D;

	[HideInInspector]
	public Bounds footer;
	[HideInInspector]
	public Collider2D[] colliders;
	
	private Vector3 previousPosition;
	private Collider2D previousColliderEnter;
	private Collider2D previousColliderExit;
	private float previousEnterTime;
	private float previousExitTime;
	
	void Awake()
	{
		_transform = GetComponent<Transform>();
	}

	void OnLevelWasLoaded( int level ) 
	{
		clouds = null;
	}
	
	void Start()
	{
		colliders = GetComponents<Collider2D>();
		if( colliders.Length == 0 )
		{
			this.enabled = false;
			return;
		}
		_rigidbody2D = GetComponent<Rigidbody2D>();
		
		IgnoreAllClouds();
		
		CheckBase();
		
		previousPosition = _transform.position;
	}
	
	void OnEnable()
	{
		previousPosition = _transform.position;
	}
	
	void FixedUpdate()
	{
		Vector3 currentPosition = _transform.position;
		float space = previousPosition.y - currentPosition.y;//(currentPosition - previousTransform).magnitude;
		//Collider2D enterCollider = null;
		
		if( space >= 0.00f && 
		   (_rigidbody2D != null && !_rigidbody2D.isKinematic) && 
		   !colliders[0].isTrigger ) 
		{
			//previousPosition = currentPosition;
			
			int raysAmount = 3;
			
			Vector3 size = footer.size;
			Vector2 init = _transform.position + footer.center - size * 0.5f;
			Vector2 end = _transform.position + footer.center + size * 0.5f;
			float distance = Mathf.Max( 0.1f, space + 0.05f );
			
			for( int i = 0 ; i < raysAmount ; i++ )
			{
				Vector2 v = Vector2.Lerp( init, end, i/(float)(raysAmount - 1) );
				RaycastHit2D hit;
				//Debug.DrawLine( v, v - Vector2.up * distance, v.Raycast( -Vector2.up, distance, out hit, gameObject, mask  ) ? Color.red : Color.blue );
				//if( v.Raycast( -Vector2.up, distance, out hit, gameObject, mask ) && 
				if( v.Raycast( -Vector2.up, distance, out hit, gameObject, mask ) && 
				   hit.collider.tag == CRef.TAG_CLOUD &&
				   !v.OverlapPoint( gameObject, mask ) &&
				   !v.OverlapPoint( gameObject, mask ) &&
				   Physics2D.GetIgnoreCollision( hit.collider, colliders[0] ) &&
				   (Time.time - previousExitTime > 0.04f || previousColliderExit != hit.collider) )
				{
					IgnoreCollision( hit.collider, false );
					if( previousColliderEnter == hit.collider ) StopCoroutine("CancelCollision");
					else previousColliderEnter = hit.collider;
					//se ele demorar para collidir com o objeto, dai é cancelado a colisao
					StartCoroutine("CancelCollision", previousColliderEnter);
					break;
				}
			}
		}
		
		previousPosition = currentPosition;
		
	}
	
	IEnumerator CancelCollision( Collider2D cloud )
	{
		yield return new WaitForSeconds(0.1f);
		IgnoreCollision( cloud, true );
		if( cloud == previousColliderEnter ) previousColliderEnter = null;
	}
	
	void CheckBase()
	{
		footer.size = Vector2.zero;
		footer.center = Vector2.up * int.MaxValue;
		foreach( Collider2D col in GetComponents<Collider2D>() )
		{
			Vector2 footerCenter = col.bounds.center - new Vector3( 0, col.bounds.size.y * 0.5f, 0 );
			if( footerCenter.y < footer.center.y ) footer.center = footerCenter;
			if( col.bounds.size.x > footer.size.x ) footer.size = new Vector2(col.bounds.size.x - 0.05f, 0);
		}
		
		footer.center -= _transform.position;
	}
	
	void OnCollisionEnter2D( Collision2D other )
	{
		if( other.collider == previousColliderEnter )
		{
			StopCoroutine( "CancelCollision" );
		}
	}
	
	void OnCollisionStay2D( Collision2D other )
	{
		if( other.collider == previousColliderEnter )
		{
			StopCoroutine( "CancelCollision" );
		}
	}
	
	void OnCollisionExit2D( Collision2D other ) 
	{ 
		if( other.gameObject.tag == CRef.TAG_CLOUD ) 
		{
			IgnoreCollision( other.collider, true ); 
			previousExitTime = Time.time;
			previousColliderExit = other.collider;
			if( previousColliderEnter == previousColliderExit ) previousColliderEnter = null;
		}
	}
	
	void IgnoreAllClouds()
	{
		if( clouds == null ) clouds = GameObject.FindGameObjectsWithTag( CRef.TAG_CLOUD ).Select( go => go.collider2D ).ToList<Collider2D>();
		
		int total = clouds.Count;
		
		for ( int i = total - 1 ; i >= 0 ; i-- )
		{
			if( clouds[i] == null ) clouds.RemoveAt(i);
			else IgnoreCollision( clouds[i], true );
		}
	}
	
	void IgnoreCollision( Collider2D cloud, bool ignore )
	{
		foreach( Collider2D col in colliders )
			Physics2D.IgnoreCollision( col, cloud, ignore );
	}
	
}