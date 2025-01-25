using System;
using Unity.Cinemachine;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private float shrinkSpeed = 0.0001f;
	[SerializeField] private CinemachineCamera inBubbleCamera, outBubbleCamera;
#endregion

#region Attributes
	public Vector2 Velocity => _rigidbody.linearVelocity;
	public float Radius => _collider.bounds.extents.x;
#endregion

#region Components
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private CircleCollider2D _collider;
#endregion

#region Data
	private Vector3 _shrinkRate = Vector3.zero;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponentInChildren<CircleCollider2D>();
		Debug.Log(Radius);
    }

    private void Start() {
	    _rigidbody.linearVelocityY = 2f;
    }

    private void Update() {
        
    }
	
	private void FixedUpdate() {
		_transform.localScale -= _shrinkRate;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(other.name + " has landed on the bubble.");
		if (other.TryGetComponent<CharacterBehavior>(out var player)) {
			player.OnBubble = true;
			if (player.IsPlayer) {
				inBubbleCamera.enabled = true;
				outBubbleCamera.enabled = false;
			}
			else {
				_shrinkRate += Vector3.one * shrinkSpeed;
			}
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		Debug.Log(other.name + " has exited the bubble.");
		if (other.TryGetComponent<CharacterBehavior>(out var player)) {
			player.OnBubble = false;
			var direction = (player.transform.position - _transform.position).normalized;
			player.Eject(direction);
			
			if (player.IsPlayer) {
				inBubbleCamera.enabled = false;
				outBubbleCamera.enabled = true;
			} else {
				_shrinkRate -= Vector3.one * shrinkSpeed;
			}
		}
	}

	#endregion

#region Custom
	private void DoFunction() {
		
	}
#endregion

}

