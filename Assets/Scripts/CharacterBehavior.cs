using System;
using DG.Tweening;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private float acceleration = 2f;
	[SerializeField] private float deceleration = 2f;
	[SerializeField] private float maxSpeed = 5f;
	[SerializeField] private float ejectForce = 30f;
	[SerializeField] private float dashForce = 10f;
#endregion

#region Attributes
	public float TimeOnBubble { get; set; }
	public Direction DirectionFacing { get; private set; }
	public Vector2 InputVector {
		get => _inputVector;
		set {
			PreviousInputVector = _inputVector;
			_inputVector = value;
			if (_inputVector != Vector2.zero) {
				PreviousNotZeroInputVector = value;
			}
		}
	}

	public Vector2 PreviousNotZeroInputVector { get; set; }
	public Vector2 PreviousInputVector { get; set; }
	public bool Spinning { get; private set; }
	public bool Dashing { get; private set; }
	public bool Controllable { get; set; } = true;
	public bool Stunned { get; set; }
#endregion

#region Components
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private AttackBehavior _attackBehavior;
#endregion

#region Data
	private Vector2 _inputVector; 
	private bool _dashCooldown;
	private Vector3 _startingPosition;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_startingPosition = _transform.position;
		_animator = GetComponent<Animator>();
		_attackBehavior = GetComponentInChildren<AttackBehavior>();
    }
	
	private void FixedUpdate() {
		if (Stunned) {return;}
		
		var bubble = GameManager.Instance.Bubble;
		var onBubble = bubble.OnBubble(this);
		
		if (InputVector.magnitude > 0f && Controllable) {
			var goalSpeed = InputVector * maxSpeed;
			if (onBubble) {
				goalSpeed += bubble.Velocity;
			}
		
			var acc = (onBubble) ? acceleration : acceleration / 4f;
			_rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, goalSpeed, Time.fixedDeltaTime * acc);
		} else if (!Stunned) {
			var goalSpeed = (onBubble) ? bubble.Velocity : Vector2.zero;
			var dec = (onBubble) ? deceleration : deceleration / 4f;
			_rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, goalSpeed, Time.fixedDeltaTime * dec);
		}
		
		if (_animator) {
			if (Controllable) {
				_animator.SetBool("up", false);
				_animator.SetBool("down", false);
				_animator.SetBool("right", false);
				_animator.SetBool("left", false);
				
				if (InputVector.x > 0f) {
					DirectionFacing = Direction.Right;
					_animator.SetBool("right", true);
				} else if (InputVector.x < 0f) {
					DirectionFacing = Direction.Left;
					_animator.SetBool("left", true);
				} else if (InputVector.y > 0f) {
					DirectionFacing = Direction.Up;
					_animator.SetBool("up", true);
				} else if (InputVector.y < 0f) {
					DirectionFacing = Direction.Down;
					_animator.SetBool("down", true);
				} 
			}

			_animator.SetFloat("speed", Mathf.Sqrt(_rigidbody.linearVelocity.sqrMagnitude) / 10f + 0.3f);
		}

		if (!Spinning && !Dashing) { // Don't clamp if spinning
			_rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, maxSpeed);	
		}
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.TryGetComponent<CharacterBehavior>(out var player)) {
			if (Dashing) {
				Debug.Log("Collided!");
				player.Knockback((other.transform.position - _transform.position).normalized, 0.8f);	
				player.Stunned = true;
				DOVirtual.DelayedCall(0.2f, () => player.Stunned = false);
			}
		}
	}

#endregion

#region Custom
	public void Init() {
		_transform.position = _startingPosition;
		_transform.eulerAngles = Vector3.zero;
		Controllable = false;
		TimeOnBubble = 0f;
		_attackBehavior.Init();

		DirectionFacing = Direction.Up;
		if (_animator) {
			_animator.SetBool("up", true);
			_animator.SetBool("down", false);
			_animator.SetBool("right", false);
			_animator.SetBool("left", false);
		}
	}
	
	public void Eject(Vector2 direction) {
		Spinning = true;
		Knockback(direction, ejectForce);
		_rigidbody.DORotate(360f, 2f).OnComplete(() => {
			_rigidbody.rotation = 0f;
			Spinning = false;
			if (tag.Equals("Enemy")) {
				EnemyManager.Instance.DespawnEnemy(GetComponent<AiController>());
			}
		});
	}
	
	public void Knockback(Vector2 direction, float force) {
		_rigidbody.AddForce(direction * (force * _rigidbody.mass), ForceMode2D.Impulse);
	}

	public void Dash() {
		if (_dashCooldown) { return; }
		_rigidbody.AddForce(PreviousNotZeroInputVector * dashForce * _rigidbody.mass, ForceMode2D.Impulse);
		_dashCooldown = true;
		Dashing = true;
		DOVirtual.DelayedCall(0.5f, () => Dashing = false);
		DOVirtual.DelayedCall(1f, () => _dashCooldown = false);
	}

	public void Attack() {
		_attackBehavior.Activate(DirectionFacing);
	}
	
#endregion

}

