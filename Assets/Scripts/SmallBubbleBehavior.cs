using Enums;
using Managers;
using UnityEngine;
using CameraState = Enums.CameraState;
using Vector3 = UnityEngine.Vector3;

public class SmallBubbleBehavior : MonoBehaviour {

#region Dependencies
	//
#endregion

#region Attributes
	//
#endregion

#region Components
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private CircleCollider2D _collider;
#endregion

#region Data
	private Vector3 _startingPosition, _startingScale;
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
		if (GameManager.Instance.GameState == GameState.Start) { return; }
		
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			Debug.Log(other.name + " has landed on the bubble.");
			player.EnterBubble();
			BubbleManager.Instance.SmallBubblePlayerIsOn = this;
			OxygenManager.Instance.InBubble();
			CameraManager.Instance.Switch(CameraState.Bubble, _transform);
			player.JetpackOff();
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }
		
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			Debug.Log(other.name + " has exited the bubble.");
			var direction = (player.transform.position - _transform.position).normalized;
			player.Eject(direction);
			BubbleManager.Instance.SmallBubblePlayerIsOn = null;
			OxygenManager.Instance.OutOfBubble();
			CameraManager.Instance.Switch(CameraState.Ocean);
		}
	}

#endregion

#region Custom
	//
#endregion

}

