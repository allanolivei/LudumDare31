using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameExtensions
{
	
	private static RaycastHit2D hitNull;

	static public Vector3 FromString( this Vector3 vector, string value )
	{
		string[] temp = value.Substring(1,value.Length -2 ).Replace(" ", "").Split(',');
		vector.x = float.Parse(temp[0]);
		vector.y = float.Parse(temp[1]);
		vector.z = float.Parse(temp[2]);
		
		return vector;
	}

	static public Vector3 ToVec3( this Vector2 vec2, float z = 0 ) {
		return new Vector3( vec2.x, vec2.y, z );
	}
	
	static public Vector2 ToVec2( this Vector3 vec3 ) {
		return new Vector2( vec3.x, vec3.y );
	}

	static public Vector2 RotateVector (this Vector2 vec, float angle)
	{
		float rad = Mathf.Deg2Rad * angle;
		
		float cs = Mathf.Cos (rad);
		float sn = Mathf.Sin (rad);
		
		float _x = vec.x * cs - vec.y * sn;
		float _y = vec.x * sn + vec.y * cs;
		
		return new Vector2 (_x, _y);
	}
	
	static public Vector2 GetNormal (this Vector2 a)
	{//90 graus
		return new Vector2 (-a.y, a.x);
	}
	
	static public float GetAngle180 (this Vector2 a, Vector2 b)
	{
		return Vector2.Angle (a, b) * 
			(Vector2.Dot (a.GetNormal (), b) > 0 ? 1 : -1);
	}
	
	static public float GetAngle360 (this Vector2 a, Vector2 b)
	{
		float angle = a.GetAngle180 (b);
		if (angle < 0)
			angle = 360 + angle;
		
		return angle;
	}
	
	
	
	static public bool OverlapPoint ( this Vector2 origin, GameObject ignore, LayerMask layerMask, bool hitTrigger = false )
	{
		Collider2D[] cols = Physics2D.OverlapPointAll( origin, layerMask );
		for( int i = 0 ; i < cols.Length ; i++ )
		{
			if( cols[i].isTrigger == hitTrigger && cols[i].gameObject != ignore ) return true;
		}
		
		return false;
	}
	
	static public bool OverlapPoint ( this Vector2 origin, LayerMask layerMask, bool hitTrigger = false )
	{
		Collider2D[] cols = Physics2D.OverlapPointAll( origin, layerMask );
		for( int i = 0 ; i < cols.Length ; i++ )
		{
			if( cols[i].isTrigger == hitTrigger ) return true;
		}
		
		return false;
	}
	
	static public bool OverlapPoint ( this Vector2 origin, LayerMask layerMask, string tag )
	{
		Collider2D[] cols = Physics2D.OverlapPointAll( origin, layerMask );
		for( int i = 0 ; i < cols.Length ; i++ )
			if( cols[i].tag == tag ) return true;
		
		return false;
	}
	
	static public bool OverlapPoint ( this Vector2 origin, out Collider2D hit, LayerMask layerMask, string tag )
	{
		Collider2D[] cols = Physics2D.OverlapPointAll( origin, layerMask );
		for( int i = 0 ; i < cols.Length ; i++ )
		{
			if( cols[i].tag == tag )
			{
				hit = cols[i];
				return true;
			}
		}
		hit = null;
		
		return false;
	}
	
	static public bool OverlapPoint ( this Vector2 origin, out Collider2D hit, LayerMask layerMask, string tag1, string tag2, bool hitTrigger = false )
	{
		Collider2D[] cols = Physics2D.OverlapPointAll( origin, layerMask );
		for( int i = 0 ; i < cols.Length ; i++ )
		{
			if( (cols[i].tag == tag1 || cols[i].tag == tag2) && cols[i].isTrigger == hitTrigger )
			{
				hit = cols[i];
				return true;
			}
		}
		hit = null;
		
		return false;
	}
	
	static public bool Raycast (this Vector2 origin, Vector2 direction, float distance, out RaycastHit2D hit, LayerMask layerMask, string tag)
	{
		//Physics2D.LinecastNonAlloc( origin, direction, hits, distance, layerMask );
		RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, distance, layerMask);
		//hits[0] = Physics2D.Raycast( origin, direction, distance, layerMask);
		
		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider != null && hits [i].collider.tag == tag) {
				hit = hits [i];
				return true;
			}
		}
		hit = hitNull;
		return false;
	}
	
	static public bool Raycast (this Vector2 origin, Vector2 direction, float distance, out RaycastHit2D hit, string tag)
	{
		//Physics2D.LinecastNonAlloc( origin, direction, hits, distance, layerMask );
		RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, distance);
		//hits[0] = Physics2D.Raycast( origin, direction, distance, layerMask);
		
		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider != null && hits [i].collider.tag == tag) {
				hit = hits [i];
				return true;
			}
		}
		hit = hitNull;
		return false;
	}
	
	static public bool Raycast (this Vector2 origin, Vector2 direction, float distance, out RaycastHit2D hit, GameObject ignoreObject, LayerMask layerMask, bool hitTrigger = false)
	{
		//Physics2D.LinecastNonAlloc( origin, direction, hits, distance, layerMask );
		RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, distance, layerMask);
		//hits[0] = Physics2D.Raycast( origin, direction, distance, layerMask);
		
		for (int i = 0; i < hits.Length; i++) {
			if ( hits [i].collider != null && hits [i].collider.isTrigger == hitTrigger && hits[i].collider.gameObject != ignoreObject ) {
				hit = hits [i];
				return true;
			}
		}
		hit = hitNull;
		return false;
	}
	
	static public bool Raycast (this Vector2 origin, Vector2 direction, float distance, out RaycastHit2D hit, LayerMask layerMask, bool hitTrigger = false)
	{
		//Physics2D.LinecastNonAlloc( origin, direction, hits, distance, layerMask );
		RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, distance, layerMask);
		//hits[0] = Physics2D.Raycast( origin, direction, distance, layerMask);
		
		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider != null && hits [i].collider.isTrigger == hitTrigger) {
				hit = hits [i];
				return true;
			}
		}
		hit = hitNull;
		return false;
	}
	
	static public bool Raycast (this Vector2 origin, Vector2 direction, float distance, out RaycastHit2D hit, LayerMask layerMask, bool hitTrigger, string ignoreTag)
	{
		//Physics2D.LinecastNonAlloc( origin, direction, hits, distance, layerMask );
		RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, distance, layerMask);
		//hits[0] = Physics2D.Raycast( origin, direction, distance, layerMask);
		
		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider != null && hits [i].collider.isTrigger == hitTrigger && hits [i].collider.tag != ignoreTag) {
				hit = hits [i];
				return true;
			}
		}
		hit = hitNull;
		return false;
	}
	
	static public bool Raycast (this Vector2 origin, Vector2 direction, float distance, LayerMask layerMask, bool hitTrigger = false)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, distance, layerMask);
		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider != null && hits [i].collider.isTrigger == hitTrigger)
				return true;
		}
		return false;
	}
	
	static public Vector2 GetCornerNormal (this Vector2 prev, Vector2 next)
	{
		//if( prev.x == next.x || prev.y == next.y ) return prev.normalized;
		
		prev.Normalize ();
		next.Normalize ();
		
		if (Vector2.Dot (prev, next) > 0.99f)//0.95f)
			return prev;
		
		prev = new Vector2 (-prev.y, prev.x);
		next = new Vector2 (next.y, -next.x);
		
		Vector2 norm = (prev + next) / 2.0f;
		norm.Normalize ();
		
		return norm;
	}
	
	//recebe point no mundo e retorna ponto local
	static public Vector2 GetClosestVertex (this Collider2D collider, Vector2 pointInWorld)
	{
		Transform tr = collider.transform;
		Vector2 pos = tr.position.ToVec2 ();
		Vector2 scale = tr.localScale.ToVec2 ();
		List<Vector2> vert = new List<Vector2> ();
		if (collider as BoxCollider2D) {
			BoxCollider2D col = collider as BoxCollider2D;
			vert.Add (pos + col.center + new Vector2 (col.size.x * scale.x, col.size.y * scale.y) * 0.5f);
			vert.Add (pos + col.center + new Vector2 (-col.size.x * scale.x, col.size.y * scale.y) * 0.5f);
			vert.Add (pos + col.center + new Vector2 (-col.size.x * scale.x, -col.size.y * scale.y) * 0.5f);
			vert.Add (pos + col.center + new Vector2 (col.size.x * scale.x, -col.size.y * scale.y) * 0.5f);
		} else if (collider as PolygonCollider2D) {
			PolygonCollider2D colPol = collider as PolygonCollider2D;
			for (int i = 0; i < colPol.points.Length; i++) 
				vert.Add (pos + new Vector2 (colPol.points [i].x * scale.x, colPol.points [i].y * scale.y));
		}
		
		float minDistance = int.MaxValue;
		Vector2 holdPoint = Vector2.zero;
		for (int i = 0; i < vert.Count; i++) {
			if ((vert [i] - pointInWorld).sqrMagnitude < minDistance) {
				holdPoint = vert [i] - pos;
				minDistance = (vert [i] - pointInWorld).sqrMagnitude;
			}
		}
		
		return holdPoint;
	}
	
}