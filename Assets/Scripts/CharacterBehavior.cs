using DG.Tweening;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private bool isPlayer;
	[SerializeField] private float acceleration = 2f;
	[SerializeField] private float deceleration = 2f;
	[SerializeField] private float maxSpeed = 5f;
	private BubbleBehavior _bubble;
#endregion

#region Attributes
	public bool OnBubble { get; set; } = true;
	public bool IsPlayer => isPlayer;

	public Vector2 InputVector {
		get => _inputVector;
		set {
			_inputVector = value;
			if (_inputVector != Vector2.zero) {
				PreviousNotZeroInputVector = value;
			}
		}
	}

	public Vector2 PreviousNotZeroInputVector { get; set; }
	public bool Spinning { get; private set; }
	public bool Dashing { get; private set; }
#endregion

#region Components
	private Transform _transform;
	private Rigidbody2D _rigidbody;
#endregion

#region Data
	private Vector2 _inputVector; 
	private bool _dashCooldown;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_bubble = FindFirstObjectByType<BubbleBehavior>();
    }

    private void Start() {
	    _rigidbody.linearVelocityY = 1f;
    }

    private void Update() {
	    
    }
	
	private void FixedUpdate() {
		if (InputVector.magnitude > 0f) {
			var goalSpeed = InputVector * maxSpeed;
			if (OnBubble) {
				goalSpeed += _bubble.Velocity;
			}
			
			var acc = (OnBubble) ? acceleration : acceleration / 4f;
			_rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, goalSpeed, Time.fixedDeltaTime * acc);
		} else {
			var goalSpeed = (OnBubble) ? _bubble.Velocity : Vector2.zero;
			var dec = (OnBubble) ? deceleration : deceleration / 4f;
			_rigidbody.linearVelocity = Vector2.Lerp(_rigidbody.linearVelocity, goalSpeed, Time.fixedDeltaTime * dec);
		}

		if (!Spinning && !Dashing) { // Don't clamp if spinning
			_rigidbody.linearVelocity = Vector2.ClampMagnitude(_rigidbody.linearVelocity, maxSpeed);	
		}
	}
#endregion

#region Custom
	public void Eject(Vector2 direction) {
		Spinning = true;
		_rigidbody.AddForce(direction * 30f * _rigidbody.mass, ForceMode2D.Impulse);
		_rigidbody.DORotate(360f, 1f).OnComplete(() => {
			_rigidbody.rotation = 0f;
			Spinning = false;
		});
	}

	public void Dash() {
		if (_dashCooldown) { return; }
		_rigidbody.AddForce(PreviousNotZeroInputVector * 10f * _rigidbody.mass, ForceMode2D.Impulse);
		_dashCooldown = true;
		Dashing = true;
		DOVirtual.DelayedCall(0.5f, () => Dashing = false);
		DOVirtual.DelayedCall(1f, () => _dashCooldown = false);
	}
	
#endregion

}

