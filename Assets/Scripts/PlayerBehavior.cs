using DG.Tweening;
using Managers;
using Scriptable;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

#region Dependencies
	[Header("Stats")]
	[SerializeField] private float acceleration = 4f;
	[SerializeField] private float deceleration = 4f;
	[SerializeField] private float maxSpeed = 5f;
	[SerializeField] private float jetpackMaxSpeed = 14f;
	[SerializeField] private float ejectForce = 6f;
	[SerializeField] private float jetpackForce = 2f;

	[Header("Sounds")]
	[SerializeField] private AudioClip _leaveBubbleSound;
	[SerializeField] private AudioClip _enterBubbleSound;
	[SerializeField] private AudioClip _dashSound;
	[SerializeField] private AudioClip _bumpSound;
	[SerializeField] private AudioClip _hurtSound;
	[SerializeField] private AudioClip _attackSound;
	
	[Header("Sprites")]
	[SerializeField] private SpriteRenderer _shockSprite;
	[SerializeField] private SpriteRenderer _jetpackSprite;
#endregion

#region Attributes
	public float TimeOnBubble { get; set; }
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
    }

    private void Update() {
	    _timeSinceLastInput += Time.deltaTime;
    }
	
	private void FixedUpdate() {
		if (Stunned) {return;}

		var onBubble = BubbleManager.Instance.PlayerIsOnBubble;
		
		if (_jetpacking) {
			var goalSpeed = PreviousNotZeroInputVector * jetpackMaxSpeed;
			var acc = acceleration / 2f;
			_velocity = Vector2.Lerp(_velocity, goalSpeed, Time.fixedDeltaTime * acc);
		} else {
			if (InputVector.magnitude > 0f) {
				var goalSpeed = InputVector * maxSpeed;
				var acc = (onBubble) ? acceleration : acceleration / 4f;
				_velocity = Vector2.Lerp(_velocity, goalSpeed, Time.fixedDeltaTime * acc);
			} else if (!Stunned) {
				var goalSpeed = Vector2.zero;
				var dec = (onBubble) ? deceleration : deceleration / 4f;
				_velocity = Vector2.Lerp(_velocity, goalSpeed, Time.fixedDeltaTime * dec);
			}
		}
		
		if (_animator) {
			if (tag.Equals("Player")) {
				if (InputVector.x > 0f) {
					ClearMovementFlags();
					_animator.SetBool("right", true);
				} else if (InputVector.x < 0f) {
					ClearMovementFlags();
					_animator.SetBool("left", true);
				} else if (InputVector.y > 0f) {
					ClearMovementFlags();
					_animator.SetBool("up", true);
				} else if (InputVector.y < 0f) {
					ClearMovementFlags();
					_animator.SetBool("down", true);
				}
				
				_animator.SetBool("idle", _velocity.sqrMagnitude < 0.05f && _timeSinceLastInput > 2f);
			}

			_animator.SetFloat("speed", Mathf.Sqrt(_velocity.sqrMagnitude) / 5f + 0.2f);
		}

		if (!Spinning) { // Don't clamp if spinning or dashing
			var m = _jetpacking ? jetpackMaxSpeed : maxSpeed;
			_rigidbody.linearVelocity = Vector2.ClampMagnitude(_velocity, m);
			_rigidbody.linearVelocity += BubbleManager.Instance.BubbleVelocity;
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
	public void StopMoving() {
		JetpackOff();
		_inputVector = Vector2.zero;
		_velocity = Vector2.zero;
		_timeSinceLastInput = 10f;
	}
	
	public void Hurt(Weapon weapon, Vector2 sourcePosition) {
		if (Stunned) { return; }

		if (weapon.Shock) {
			_shockSprite.enabled = true;
		}
		
		AudioManager.Instance.PlaySfx(_hurtSound);
            
		Stunned = true;
		var direction = ((Vector2) _transform.position - sourcePosition).normalized;
		_rigidbody.AddForce(direction * weapon.Knockback, ForceMode2D.Impulse);
		OxygenManager.Instance.Hurt(weapon.Damage);
		
		_spriteRenderer.DOFade(0.1f, 0.05f).SetLoops(-1, LoopType.Yoyo);
		_jetpackSprite.DOFade(0.1f, 0.05f).SetLoops(-1, LoopType.Yoyo);
            
		DOVirtual.DelayedCall(weapon.StunTime, () => {
			Stunned = false;    
			_shockSprite.enabled = false;
			
			_spriteRenderer.DOKill();
			_spriteRenderer.DOFade(1f, 0f);
			
			_jetpackSprite.DOKill();
			_jetpackSprite.DOFade(1f, 0f);
		});
	}
	public void Init() {
		_velocity = Vector2.zero;
		_transform.position = _startingPosition;
		_transform.eulerAngles = Vector3.zero;
		_jetpacking = false;
		TimeOnBubble = 0f;
		_timeSinceLastInput = 10f;
		_attackBehavior.Init();
		
		if (_animator) {
			_animator.SetBool("up", true);
			_animator.SetBool("down", false);
			_animator.SetBool("right", false);
			_animator.SetBool("left", false);
		}
	}
	
	public void InitWithBubble(Vector2 v) {
		_attackBehavior.Init();
		
		MoveToBubble(v);
	}

	public void MoveToBubble(Vector2 v) {
		_velocity = Vector2.zero;
		_transform.position = v;
		_transform.eulerAngles = Vector3.zero;
		_jetpacking = false;
		TimeOnBubble = 0f;
		_timeSinceLastInput = 10f;
		
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
		Knockback(direction, ejectForce);
		AudioManager.Instance.PlaySfx(_leaveBubbleSound);

		var duration = tag.Equals("Enemy") ? 0.5f : 1f;
		_rigidbody.DORotate(360f, duration).OnComplete(() => {
			_rigidbody.rotation = 0f;
			Spinning = false;
			if (tag.Equals("Enemy")) {
				//EnemyManager.Instance.DespawnEnemy(GetComponent<AiController>());
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
		
		_attackBehavior.Activate();
	}
	
	public void JetpackOn() {
		if (Stunned) { return; }
		if (BubbleManager.Instance.PlayerIsOnBubble) { return; }

		_particleSystem.Play();
		_jetpacking = true;
		OxygenManager.Instance.ToggleJetpack(true);
		
		if (_animator) {
			_animator.SetBool("jetpack", true);
		}

		_velocity += PreviousNotZeroInputVector * jetpackForce;
		_timeSinceLastInput = 0f;
		AudioManager.Instance.PlaySfx(_dashSound);	
	}
	
	public void JetpackOff() {
		_particleSystem.Stop();
		_jetpacking = false;
		OxygenManager.Instance.ToggleJetpack(false);
		
		if (_animator) {
			_animator.SetBool("jetpack", false);
		}
	}
	
#endregion

}

