using DG.Tweening;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {

#region Dependencies
	public float Acceleration = 2f;
	public float Deceleration = 2f;
	public float MaxSpeed = 5f;
	public float EjectForce = 20f;
	public float DashSpeed = 10f;
	public float DashForce = 0.8f;

	[SerializeField] private AudioClip _leaveBubbleSound;
	[SerializeField] private AudioClip _enterBubbleSound;
	[SerializeField] private AudioClip _dashSound;
	[SerializeField] private AudioClip _bumpSound;
	[SerializeField] private AudioClip _hurtSound;
	[SerializeField] private AudioClip _attackSound;
	[SerializeField] private SpriteRenderer _shockSprite;
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
	public bool Stunned { get; private set; }
#endregion

#region Components
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private AttackBehavior _attackBehavior;
	private SpriteRenderer _spriteRenderer;
#endregion

#region Data
	private Vector2 _inputVector; 
	private bool _dashCooldown;
	private Vector3 _startingPosition;
	private float _startingAcceleration, _startingDeceleration, _startingMaxSpeed, _startingDashSpeed, _startingDashForce;
	private Vector2 _velocity;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_startingPosition = _transform.position;
		_animator = GetComponent<Animator>();
		_attackBehavior = GetComponentInChildren<AttackBehavior>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

		_startingAcceleration = Acceleration;
		_startingDeceleration = Deceleration;
		_startingMaxSpeed = MaxSpeed;
		_startingDashSpeed = DashSpeed;
		_startingDashForce = DashForce;
    }
	
	private void FixedUpdate() {
		if (Stunned) {return;}
		
		var bubble = GameManager.Instance.Bubble;
		var onBubble = bubble.OnBubble(this);
		
		if (InputVector.magnitude > 0f && Controllable) {
			var goalSpeed = InputVector * MaxSpeed;
			var acc = (onBubble) ? Acceleration : Acceleration / 4f;
			_velocity = Vector2.Lerp(_velocity, goalSpeed, Time.fixedDeltaTime * acc);
		} else if (!Stunned) {
			var goalSpeed = Vector2.zero;
			var dec = (onBubble) ? Deceleration : Deceleration / 4f;
			_velocity = Vector2.Lerp(_velocity, goalSpeed, Time.fixedDeltaTime * dec);
		}
		
		if (_animator) {
			if (Controllable && tag.Equals("Player")) {
				if (InputVector.x > 0f) {
					DirectionFacing = Direction.Right;
					ClearMovementFlags();
					_animator.SetBool("right", true);
				} else if (InputVector.x < 0f) {
					DirectionFacing = Direction.Left;
					ClearMovementFlags();
					_animator.SetBool("left", true);
				} else if (InputVector.y > 0f) {
					DirectionFacing = Direction.Up;
					ClearMovementFlags();
					_animator.SetBool("up", true);
				} else if (InputVector.y < 0f) {
					DirectionFacing = Direction.Down;
					ClearMovementFlags();
					_animator.SetBool("down", true);
				} 
			}

			_animator.SetFloat("speed", Mathf.Sqrt(_velocity.sqrMagnitude) / 5f + 0.2f);
		}

		if (!Spinning && !Dashing) { // Don't clamp if spinning or dashing
			_rigidbody.linearVelocity = Vector2.ClampMagnitude(_velocity, MaxSpeed);
			if (onBubble) {
				_rigidbody.linearVelocity += bubble.Velocity;
			}
		}
	}

	private void ClearMovementFlags() {
		_animator.SetBool("up", false);
		_animator.SetBool("down", false);
		_animator.SetBool("right", false);
		_animator.SetBool("left", false);
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.TryGetComponent<CharacterBehavior>(out var player)) {
			if (Dashing) {
				AudioManager.Instance.PlaySfx(_bumpSound);
				Debug.Log("Collided!");
				player.Knockback((other.transform.position - _transform.position).normalized, DashForce);	
				player.Stunned = true;
				DOVirtual.DelayedCall(0.2f, () => player.Stunned = false);
			}
		}
	}

#endregion

#region Custom
	public void UpgradeDash() {
		DashSpeed += 2f;
		DashForce += 0.4f;
	}

	public void UpgradeWeapon() {
		_attackBehavior.EquippedWeapon.StunTime += _attackBehavior.EquippedWeapon.StunTime * .1f;
		_attackBehavior.EquippedWeapon.Knockback += _attackBehavior.EquippedWeapon.Knockback * .1f;
	}
	
	public void Stun(bool zapped) {
		if (tag.Equals("Player") && Stunned) {
			return;
		}

		if (_shockSprite && zapped) {
			_shockSprite.enabled = true;
		}
		
		AudioManager.Instance.PlaySfx(_hurtSound);
		
		Stunned = true;
		_spriteRenderer.DOFade(0.1f, 0.05f).SetLoops(-1, LoopType.Yoyo);
	}
	
	public void Unstun() {
		Stunned = false;
		_spriteRenderer.DOKill();
		_spriteRenderer.DOFade(1f, 0f);

		if (_shockSprite) {
			_shockSprite.enabled = false;
		}
	}

	public void ResetStats() {
		Acceleration = _startingAcceleration;
		Deceleration = _startingDeceleration;
		MaxSpeed = _startingMaxSpeed;
		DashSpeed = _startingDashSpeed;
		DashForce = _startingDashForce;

		if (_attackBehavior) {
			_attackBehavior.ResetWeapon();	
		}
	}
	public void Init() {
		ResetStats();

		_velocity = Vector2.zero;
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
		TimeOnBubble = 0f;
		Knockback(direction, EjectForce);
		AudioManager.Instance.PlaySfx(_leaveBubbleSound);

		var duration = tag.Equals("Enemy") ? 0.5f : 1f;
		_rigidbody.DORotate(360f, duration).OnComplete(() => {
			_rigidbody.rotation = 0f;
			Spinning = false;
			if (tag.Equals("Enemy")) {
				EnemyManager.Instance.DespawnEnemy(GetComponent<AiController>());
			}
		});
	}

	public void EnterBubble() {
		TimeOnBubble = 0f;
		AudioManager.Instance.PlaySfx(_enterBubbleSound);
	}
	
	public void Knockback(Vector2 direction, float force) {
		_rigidbody.AddForce(direction * (force * _rigidbody.mass), ForceMode2D.Impulse);
	}

	public void Dash() {
		if (_dashCooldown || Stunned) { return; }
		
		if (_animator) {
			_animator.SetTrigger("dash");
		}
		
		AudioManager.Instance.PlaySfx(_dashSound);
		_rigidbody.AddForce(PreviousNotZeroInputVector * DashSpeed * _rigidbody.mass, ForceMode2D.Impulse);
		_dashCooldown = true;
		Dashing = true;
		DOVirtual.DelayedCall(GameManager.Instance.Bubble.OnBubble(this) ? 0.3f : 0.6f, () => Dashing = false);
		DOVirtual.DelayedCall(1f, () => _dashCooldown = false);
	}

	public void Attack() {
		if (Dashing || Stunned) { return; }

		if (!_attackBehavior.OnCooldown) {
			DOVirtual.DelayedCall(0.1f, () => AudioManager.Instance.PlaySfx(_attackSound));
			if (_animator) {
				_animator.SetTrigger("attack");
			}
		}

		
		_attackBehavior.Activate(DirectionFacing);
	}
	
#endregion

}

