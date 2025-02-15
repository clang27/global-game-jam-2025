using System;
using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;
using CameraState = Enums.CameraState;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LargeBubbleBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private Vector2 goalVelocity;
	[SerializeField] private string sceneName = "Ocean";
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
	private Vector3 _startingPosition, _startingScale;
	private Tween _accelerationTween;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponentInChildren<CircleCollider2D>();
		_startingPosition = _transform.position;
		_startingScale = _transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other) {
	    if (GameManager.Instance.GameState == GameState.Playing) {
		    if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			    Debug.Log(other.name + " has landed on the bubble.");
			    player.EnterBubble();
			    BubbleManager.Instance.LargeBubblePlayerIsOn = this;
		    
			    OxygenManager.Instance.InBubble();
			    CameraManager.Instance.Switch(CameraState.Bubble, _transform, false);
			    player.JetpackOff();
			    
			    GameManager.Instance.StartLargeBubbleTransition(sceneName);
		    }
	    } else if (other.TryGetComponent<PlayerBehavior>(out var player)) {
		    Debug.Log(other.name + " has landed on the bubble from a scene transition.");
		    BubbleManager.Instance.LargeBubblePlayerIsOn = this;
		    
		    CameraManager.Instance.Switch(CameraState.Bubble, _transform, true);
		    DOVirtual.DelayedCall(2f, StopMoving);
	    }
    }
	
    private void OnTriggerExit2D(Collider2D other) {
	    if (GameManager.Instance.GameState is GameState.Start or GameState.BubbleTransition) { return; }
		
	    if (other.TryGetComponent<PlayerBehavior>(out var player)) {
		    Debug.Log(other.name + " has exited the bubble.");
		    var direction = (player.transform.position - _transform.position).normalized;
		    player.Eject(direction);
		    BubbleManager.Instance.LargeBubblePlayerIsOn = null;
		    OxygenManager.Instance.OutOfBubble();
		    CameraManager.Instance.Switch(CameraState.Ocean);
	    }
    }

    private void OnDestroy() {
	    _accelerationTween.Kill();
    }

    #endregion

#region Custom
	
	public void StartMovingInstantly() {
		_rigidbody.linearVelocity = goalVelocity;
	}
	
	public void StartMovingSlowly() {
		_accelerationTween = 
			DOVirtual.Vector2(Vector2.zero, goalVelocity, 1f, 
				(v) => _rigidbody.linearVelocity = v);
	}
	
	public void StopMoving() {
		_rigidbody.linearVelocity = Vector2.zero;
	}
	
#endregion

}

