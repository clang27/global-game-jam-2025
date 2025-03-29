using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Managers;
using Scriptable;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

#region Dependencies
	[Header("Stats")]
	[SerializeField] private float acceleration = 4f;
	[SerializeField] private float deceleration = 4f;
	[SerializeField] private float maxSpeed = 5f;
	
	[Header("Jetpack")]
	[SerializeField] private float jetpackMaxSpeed = 14f;
	[SerializeField] private float jetpackForce = 2f;
	
	[Header("Sounds")]
	[SerializeField] private AudioClip _dashSound;
	[SerializeField] private AudioClip _bumpSound;
	[SerializeField] private AudioClip _hurtSound;
	[SerializeField] private AudioClip _attackSound;
	
	[Header("Sprites")]
	[SerializeField] private SpriteRenderer _shockSprite;
	[SerializeField] private SpriteRenderer _jetpackSprite;
	[SerializeField] private SpriteRenderer weaponSpriteRenderer;

	[Header("Weapons")] 
	[SerializeField] private Weapon hookWeapon;
	[SerializeField] private Weapon swordWeapon;
#endregion

#region Attributes
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
	public bool Stunned { get; private set; }
	public bool InCutScene { get; private set; }
	public bool HasJetpack { get; private set; }
	public bool HasWeapon => _attackBehavior.EquippedWeapon != null;
	public bool Jetpacking => _jetpacking;
	private bool Boosting { get; set; }
	public List<Vector3> ExternalForces { get; set; } = new();
#endregion

#region Components
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private AttackBehavior _attackBehavior;
	private SpriteRenderer _spriteRenderer;
	private ParticleSystem _particleSystem;
	private GameObject _jetpack;
#endregion

#region Data
	private Vector2 _inputVector; 
	private bool _dashCooldown;
	private Vector3 _startingPosition;
	private Vector2 _velocity;
	private bool _jetpacking, _jetpackCooldown, _jetpackReleased;
	private float _timeSinceLastInput = 10f;
	private Tween _boostTween;
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
		_jetpack = _transform.GetChild(2).gameObject;
    }

    private void Update() {
	    _timeSinceLastInput += Time.deltaTime;
    }
	
	private void FixedUpdate() {
		var onBubble = BubbleManager.Instance.PlayerIsOnBubble;
		var acc = deceleration;
		var goalSpeed = Vector2.zero;

		if (!Stunned) {
			if (Boosting) {
				goalSpeed = InputVector * maxSpeed;
				acc = acceleration * 10f;
			} else if (Jetpacking) {
				if (_jetpackReleased) {
					_jetpackReleased = false;
					JetpackOff(true);
				} else {
					goalSpeed = PreviousNotZeroInputVector * jetpackMaxSpeed;
					acc = acceleration * 2f;
				}
			} else {
				if (InputVector.magnitude > 0f) {
					goalSpeed = InputVector * maxSpeed;
					acc = (onBubble) ? acceleration * 2 : acceleration;
				} else {
					goalSpeed = Vector2.zero;
					acc = (onBubble) ? deceleration * 2 : deceleration;
				}
			}
		}
		
		foreach (var force in ExternalForces) {
			goalSpeed += (Vector2) force;
		}
		_velocity = Vector2.Lerp(_velocity, goalSpeed, Stunned ? 1f : Time.fixedDeltaTime * acc);
		
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
				
				_animator.SetBool("idle", _velocity.sqrMagnitude < 0.05f && _timeSinceLastInput > 2f && !InCutScene);
			}

			_animator.SetFloat("speed", Mathf.Min(Mathf.Sqrt(_velocity.sqrMagnitude) / 15f + 0.2f, 3f));
		}
		
		_rigidbody.linearVelocity = _velocity;
		_rigidbody.linearVelocity += BubbleManager.Instance.BubbleVelocity;	
	}

	private void ClearMovementFlags() {
		_animator.SetBool("up", false);
		_animator.SetBool("down", false);
		_animator.SetBool("right", false);
		_animator.SetBool("left", false);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Floor"))) return;
		if (_velocity.sqrMagnitude < 1000f) return;
		
		Debug.Log($"Slowing down!!! {_velocity.sqrMagnitude}");
		_velocity.x /= 4f;
		_velocity.y /= 4f;
		_rigidbody.linearVelocity = _velocity;
		Boosting = false;
	}
#endregion

#region Custom
	public void StopMoving() {
		JetpackOff(false);
		_inputVector = Vector2.zero;
		_velocity = Vector2.zero;
		_timeSinceLastInput = 10f;
	}

	public void CutScene(float f) {
		InCutScene = true;
		DOVirtual.DelayedCall(f, () => InCutScene = false);
		StopMoving();
	}
	
	public void Hurt(Weapon weapon, Vector2 direction) {
		if (Stunned) { return; }

		if (weapon.Shock) {
			_shockSprite.enabled = true;
		}
		
		AudioManager.Instance.PlaySfx(_hurtSound);

		JetpackOff(true);
		
		Boosting = false;
		Stunned = true;
		
		ExternalForces.Add(direction * weapon.Knockback);
		OxygenManager.Instance.Hurt(weapon.Damage);
		
		_spriteRenderer.DOFade(0.1f, 0.05f).SetLoops(-1, LoopType.Yoyo);
		_jetpackSprite.DOFade(0.1f, 0.05f).SetLoops(-1, LoopType.Yoyo);
            
		DOVirtual.DelayedCall(weapon.StunTime, () => {
			Stunned = false;    
			ExternalForces.Remove(direction * weapon.Knockback);
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
		ExternalForces.Clear();
		
		_jetpacking = false;
		_jetpackCooldown = false;
		_jetpackReleased = false;
			
		Stunned = false;
		Boosting = false;
		
		_timeSinceLastInput = 10f;
		
		_attackBehavior.Init();
		HasJetpack = SaveManager.Instance.HasJetpack();
		_jetpack.SetActive(HasJetpack);

		if (SaveManager.Instance.HasHook()) {
			_attackBehavior.EquipWeapon(hookWeapon);
		} else if (SaveManager.Instance.HasSword()) {
			_attackBehavior.EquipWeapon(swordWeapon);
		}
		
		if (_animator) {
			_animator.SetBool("up", true);
			_animator.SetBool("down", false);
			_animator.SetBool("right", false);
			_animator.SetBool("left", false);
		}
	}
	
	public void InitWithBubble(Vector2 v) {
		Stunned = false;
		Boosting = false;

		_spriteRenderer.DOKill();
		_spriteRenderer.DOFade(1f, 0f);

		_jetpackSprite.DOKill();
		_jetpackSprite.DOFade(1f, 0f);
		
		_inputVector = Vector2.zero;
		InputVector = Vector2.zero;
		PreviousInputVector = Vector2.zero;
		PreviousNotZeroInputVector = Vector2.zero;
		
		_attackBehavior.Init();
		HasJetpack = SaveManager.Instance.HasJetpack();
		_jetpack.SetActive(HasJetpack);
		
		if (SaveManager.Instance.HasHook()) {
			_attackBehavior.EquipWeapon(hookWeapon);
		} else if (SaveManager.Instance.HasSword()) {
			_attackBehavior.EquipWeapon(swordWeapon);
		}
		
		MoveToBubble(v);
	}

	public void MoveToBubble(Vector2 v) {
		_velocity = Vector2.zero;
		_transform.position = v;
		_transform.eulerAngles = Vector3.zero;
		_jetpacking = false;
		_timeSinceLastInput = 10f;
		ExternalForces.Clear();
		
		if (_animator) {
			_animator.SetBool("up", true);
			_animator.SetBool("down", false);
			_animator.SetBool("right", false);
			_animator.SetBool("left", false);
		}
	}
	
	public Vector3 BubbleBoost(float boostMaxSpeed, float boostTime) {
		Debug.Log("Boost time!");
		
		_boostTween?.Kill();
		
		var force = PreviousNotZeroInputVector * boostMaxSpeed;
		Boosting = true;
		ExternalForces.Add(force);
        
		_boostTween = DOVirtual.DelayedCall(boostTime, () => {
			Boosting = false;
			ExternalForces.Remove(force);
		}).OnKill(() => ExternalForces.Remove(force));

		return force.normalized;
	}

	public void Attack() {
		if (Stunned || _jetpacking || !HasWeapon || Boosting) { return; }

		if (!_attackBehavior.OnCooldown) {
			DOVirtual.DelayedCall(0.1f, () => AudioManager.Instance.PlaySfx(_attackSound));
			if (_animator) {
				_animator.SetTrigger("attack");
				_timeSinceLastInput = 0f;
			}
		}
		
		_attackBehavior.Activate();
	}

	public void Interact() {
		Debug.Log("Interacting!");
		if (Stunned) { return; }
		if (KeyManager.TouchedDoor && KeyManager.TouchedDoor.CanOpen) { 	
			_velocity = Vector2.zero;
			_transform.position = KeyManager.TouchedDoor.TeleportLocation;
			_transform.eulerAngles = Vector3.zero;
			_jetpacking = false;
			_timeSinceLastInput = 10f;
		}

		if (KeyManager.TouchedChest) {
			KeyManager.TouchedChest.Open();
		}
	}

	public void AddJetpack(ItemId id) {
		_jetpack.SetActive(true);
		HasJetpack = true;
	}
	
	public void AddWeapon(ItemId id) {
		if (id.type != ItemType.Weapon) { return; }

		switch (id.number) {
			case 0:
				_attackBehavior.EquipWeapon(hookWeapon);
				break;
			case 1:
				_attackBehavior.EquipWeapon(swordWeapon);
				break;
		}
	}
	
	public void JetpackOn() {
		if (!HasJetpack) { return; }
		if (Stunned) { return; }
		if (Boosting) { _jetpackReleased = false; return; }
		if (BubbleManager.Instance.PlayerIsOnBubble) { return; }
		if (_jetpackCooldown) { return; }

		_jetpackCooldown = true;
		DOVirtual.DelayedCall(0.5f, () => _jetpackCooldown = false);
		
		_particleSystem.Play();
		_jetpacking = true;
		OxygenManager.Instance.ToggleJetpack(true);
		CameraManager.Instance.Switch(CameraState.Jetpack, name);
		
		if (_animator) {
			_animator.SetBool("jetpack", true);
		}

		_velocity += PreviousNotZeroInputVector * jetpackForce;
		_timeSinceLastInput = 0f;
		AudioManager.Instance.PlaySfx(_dashSound);	
	}
	
	public void JetpackOff(bool changeCamera) {
		if (Boosting) { _jetpackReleased = true; return; }
		
		_particleSystem.Stop();
		_jetpacking = false;
		OxygenManager.Instance.ToggleJetpack(false);
		if (changeCamera) {
			CameraManager.Instance.Switch(BubbleManager.Instance.PlayerIsOnBubble ? CameraState.Bubble : CameraState.Ocean, name);	
		}
		
		if (_animator) {
			_animator.SetBool("jetpack", false);
		}
	}
	
#endregion

}

