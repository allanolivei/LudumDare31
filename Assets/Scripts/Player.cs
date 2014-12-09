#define DEBUG_MODE

using UnityEngine;
using System.Collections;

public class Player : StateMachine {

	public enum STATES { GROUND, AIR, SIT }

	public float abstinence = 0;

	public float maxSpeed = 0.02f;
	public float forceH = 50;
	public float jumpForce = 8;
	public float jumpDelay = 0.3f;
	public LayerMask groundMask = -1;
	public GameObject keyHelp;
	public GameObject ab_bar;

	[HideInInspector, System.NonSerialized]
	public Transform currentComputer;
	[HideInInspector, System.NonSerialized]
	public bool isGround = false;

	private Rigidbody2D _rigidBody;
	private Animator _animator;
	private Transform _transform;
	private float timerInGround = 0;


	void Start () {
		_transform = GetComponent<Transform> ();

		_rigidBody = GetComponent<Rigidbody2D> ();
		_rigidBody.fixedAngle = true;

		_animator = GetComponent<Animator> ();

		AddState( STATES.GROUND, null, State_Ground_Update, null );
		AddState( STATES.AIR, null, State_Air_Update, null );
		AddState( STATES.SIT, State_Sit_Enter, State_Sit_Update, State_Sit_Exit );

		running = true;
		this.updateLocal = UpdateLocal.FixedUpdate;
		currentState = STATES.GROUND;
	}

	public override void Update() 
	{
		if( _transform.position.y > 2.3f )
		{
			ab_bar.SetActive(false);
			abstinence = -1;
		}

		//abstinence = Mathf.Clamp ( abstinence + (currentState.Equals(STATES.SIT) ? -0.25f : +0.06f) * Time.deltaTime, -1, 1 );
        abstinence = Mathf.Clamp(abstinence + (currentState.Equals(STATES.SIT) ? -0.25f : +0.08f) * Time.deltaTime, -1, 1);
		CheckIsGround ();
		base.Update ();
	}
	
	#region STATES
	void State_Ground_Update()
	{
		//State transition
		if (!isGround) {
			currentState = STATES.AIR;
			return;
		}

		float direction = Input.GetAxisRaw ("Horizontal");
		Move (direction, forceH);

		CheckJump ();


		if ( Input.GetButtonDown ("Action") && currentComputer )
		{
			currentState = STATES.SIT;
		}
	}

	void State_Air_Update() {
		//State transition
		if ( isGround ) {
			currentState = STATES.GROUND;
			return;
		}

		float direction = Input.GetAxisRaw ("Horizontal");
		Move (direction, forceH);
	}

	void State_Sit_Enter()
	{
		currentComputer.SendMessage("TurnOnComputer", SendMessageOptions.DontRequireReceiver);
		keyHelp.SetActive(false);
		abstinence = 0;
	}

	void State_Sit_Update()
	{
		if( !MoveToPosition( currentComputer.position, forceH ) )
		{
			_animator.SetBool ("Sit", false);
			return;
		}
		else
		{
			_animator.SetBool ("Sit", true);

		}

		if ( Time.time - timeEnterState > 1.0f && 
		     (Input.GetButtonDown ("Action") || Input.GetAxisRaw("Horizontal") != 0 || Input.GetKeyDown(KeyCode.Escape)) ) 
		{
			_animator.SetBool ("Sit", false);
			currentState = STATES.GROUND;
			PauseWaitingTime(0.4f);
		}
	}

	void State_Sit_Exit()
	{
		currentComputer.SendMessage("TurnOffComputer", SendMessageOptions.DontRequireReceiver);
		keyHelp.SetActive(true);
	}

	void Move( float direction, float force )
	{
		_rigidBody.AddForce ( Vector2.right * direction * forceH, ForceMode2D.Force);
		
		if ( Mathf.Abs(_rigidBody.velocity.x) > maxSpeed)
		{
			Vector2 vel = _rigidBody.velocity;
			vel.x = Mathf.Sign(_rigidBody.velocity.x) * maxSpeed;
			_rigidBody.velocity = vel;
		}
		SetEyeDirection (direction);
		_animator.SetFloat ("H_Speed", Mathf.Abs (direction));
	}

	bool MoveToPosition( Vector3 position, float force )
	{
		float diff = Mathf.Abs(position.x - _transform.position.x);
		float direction = Mathf.Sign(position.x - _transform.position.x);
		_animator.SetFloat ("H_Speed", Mathf.Abs (direction));
		SetEyeDirection (direction);
		float result = (_rigidBody.velocity.x + forceH * Time.fixedDeltaTime) * Time.fixedDeltaTime;
		if ( result > diff)
		{
			Vector3 p = _transform.position;
			p.x = _transform.position.x;
			_rigidBody.MovePosition(p);
			return true;
		}
		else
		{
			Move (direction, forceH);
			return false;
		}
	}
	
	#endregion States

	void CheckJump()
	{
		timerInGround += Time.fixedDeltaTime;
		if( timerInGround > jumpDelay && Input.GetButton ("Jump") )
		{
			timerInGround = 0;
			isGround = false;
			_rigidBody.AddForce( Vector2.up * jumpForce, ForceMode2D.Impulse );
		}
	}

	void CheckIsGround()
	{
		Vector2 center = _rigidBody.collider2D.bounds.center;
		Vector2 size = _rigidBody.collider2D.bounds.size;
		Vector2 bottomLeft = center + new Vector2 (-size.x, -size.y) * 0.5f;
		Vector2 bottomRight = center + new Vector2 (size.x, -size.y) * 0.5f;
		isGround = Physics2D.Raycast (bottomLeft, -Vector2.up, 0.3f, groundMask) || Physics2D.Raycast (bottomRight, -Vector2.up, 0.3f, groundMask);

#if DEBUG_MODE
		Debug.DrawRay (bottomLeft, -Vector2.up * 0.1f, Color.red);
		Debug.DrawRay (bottomRight, -Vector2.up * 0.1f, Color.red);
#endif
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if ( other.tag == CRef.TAG_COMPUTER && Mathf.Abs(other.transform.eulerAngles.z) < 1.0f ) 
		{
			currentComputer = other.transform;
			keyHelp.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if ( other.tag == CRef.TAG_COMPUTER && other.transform == currentComputer )
		{
			currentComputer = null;
			keyHelp.SetActive(false);
		}
	}

	void SetEyeDirection( float direction )
	{
		if (direction != 0) 
			_transform.rotation = Quaternion.Euler (Vector3.up * Mathf.Max(0, Mathf.Sign (direction) * 180));
	}
}
