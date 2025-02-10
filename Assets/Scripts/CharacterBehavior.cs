using DG.Tweening;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {

#region Dependencies
	public float Acceleration = 2f;
	public float Deceleration = 2f;
	public float MaxSpeed = 5f;
	public float JetpackMaxSpeed = 10f;
	public float EjectForce = 20f;
	public float JetpackForce = 2f;

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
				_timeSinceLastInput = 0f;
			}
		}
	}

	public Vector2 PreviousNotZeroInputVector { get; set; }
	public Vector2 PreviousInputVector { get; set; }
	public bool Spinning { get; private set; }
	public bool Stunned { get; private set; }
#endregion

#region Components
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private AttackBehavior _attackBehavior;
	private SpriteRenderer _spriteRenderer;
	private ParticleSystem _particleSystem;
#endregion

#region Data
	private Vector2 _inputVector; 
	private bool _dashCooldown;
	private Vector3 _startingPosition;
	private float _startingAcceleration, _startingDeceleration, _startingMaxSpeed, _startingJetpackForce;
	private Vector2 _velocity;
	private bool _jetpacking;
	private float _timeSinceLastInput = 10f;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_startingPosition = _transform.position;
		_animator = GetComponent<Animator>();
		_attackBehavior = GetComponentInChildren<AttackBehavior>();
		_particleSystem = GetComponentInChildren<ParticleSystem>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

		_startingAcceleration = Acceleration;
		_startingDeceleration = Deceleration;
		_startingMaxSpeed = MaxSpeed;
		_startingJetpackForce = JetpackForce;
    }

    private void Update() {
	    _timeSinceLastInput += Time.deltaTime;
    }
	
	private void FixedUpdate() {
		if (Stunned) {return;}
		
		var bubble = GameManager.Instance.Bubble;
		var onBubble = bubble.OnBubble(this);
		
		if (_jetpacking) {
			var goalSpeed = PreviousNotZeroInputVector * JetpackMaxSpeed;
			var acc = Acceleration / 2f;
			_velocity = Vector2.Lerp(_velocity, goalSpeed, Time.fixedDeltaTime * acc);
			OxygenManager.Instance.AddOxygen(-0.002f);
		} else {
			if (InputVector.magnitude > 0f) {
				var goalSpeed = InputVector * MaxSpeed;
				var acc = (onBubble) ? Acceleration : Acceleration / 4f;
				_velocity = Vector2.Lerp(_velocity, goalSpeed, Time.fixedDeltaTime * acc);
			} else if (!Stunned) {
				var goalSpeed = Vector2.zero;
				var dec = (onBubble) ? Deceleration : Deceleration / 4f;
				_velocity = Vector2.Lerp(_velocity, goalSpeed, Time.fixedDeltaTime * dec);
			}
		}
		
		if (_animator) {
			if (tag.Equals("Player")) {
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
				
				_animator.SetBool("idle", _velocity.sqrMagnitude < 0.05f && _timeSinceLastInput > 2f);
			}

			_animator.SetFloat("speed", Mathf.Sqrt(_velocity.sqrMagnitude) / 5f + 0.2f);
		}

		if (!Spinning) { // Don't clamp if spinning or dashing
			var maxSpeed = _jetpacking ? JetpackMaxSpeed : MaxSpeed;
			_rigidbody.linearVelocity = Vector2.ClampMagnitude(_velocity, maxSpeed);
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

#endregion

#region Custom
	public void UpgradeJetpack() {
		JetpackForce += 0.4f;
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
		Debug.Log("Unstun");
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
		JetpackForce = _startingJetpackForce;

		if (_attackBehavior) {
			_attackBehavior.ResetWeapon();	
		}
	}
	public void Init() {
		ResetStats();

		_velocity = Vector2.zero;
		_transform.position = _startingPosition;
		_transform.eulerAngles = Vector3.zero;
		_jetpacking = false;
		TimeOnBubble = 0f;
		_timeSinceLastInput = 10f;
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

	public void Attack() {
		if (Stunned || _jetpacking) { return; }

		if (!_attackBehavior.OnCooldown) {
			DOVirtual.DelayedCall(0.1f, () => AudioManager.Instance.PlaySfx(_attackSound));
			if (_animator) {
				_animator.SetTrigger("attack");
				_timeSinceLastInput = 0f;
			}
		}
		
		_attackBehavior.Activate(DirectionFacing);
	}
	
	public void JetpackOn() {
		if (Stunned) { return; }

		var onBubble = GameManager.Instance.Bubble.OnBubble(this);
		
		if (onBubble) { return; }

		_particleSystem.Play();
		_jetpacking = true;
		
		if (_animator) {
			_animator.SetBool("jetpack", _jetpacking);
		}

		_velocity += PreviousNotZeroInputVector * JetpackForce;
		OxygenManager.Instance.AddOxygen(-0.005f);
		_timeSinceLastInput = 0f;
		AudioManager.Instance.PlaySfx(_dashSound);	
	}
	
	public void JetpackOff() {
		_particleSystem.Stop();
		_jetpacking = false;
		
		if (_animator) {
			_animator.SetBool("jetpack", _jetpacking);
		}
	}
	
#endregion

}

