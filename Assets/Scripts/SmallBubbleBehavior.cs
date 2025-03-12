using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;
using CameraState = Enums.CameraState;
using Vector3 = UnityEngine.Vector3;

public class SmallBubbleBehavior : MonoBehaviour {

#region Components
	private Transform _transform;
	private Animator _animator;
#endregion

#region Data
	private const float JetpackDelayTime = 1f;
	private bool _jetpackDelay;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_animator = GetComponent<Animator>();
    }

	private void OnTriggerEnter2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }
		
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			Debug.Log(other.name + " has landed on the bubble.");

			BubbleManager.Instance.SmallBubblePlayerIsOn = this;
			OxygenManager.Instance.InBubble();
			SaveManager.Instance.Save(this);
			player.EnterBubble();
			
			if (!player.Jetpacking) {
				player.JetpackOff();
				CameraManager.Instance.Switch(CameraState.Bubble, _transform);
			} else if (!_jetpackDelay) {
				_jetpackDelay = true;
				
				var dir = (other.transform.position - _transform.position).normalized;
				var lookRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: dir);
				DOVirtual.DelayedCall(JetpackDelayTime, () => _jetpackDelay = false);
				_transform.DORotate(lookRotation.eulerAngles + new Vector3(0f, 0f, 90f), 0.15f).SetEase(Ease.InOutCirc);
				_animator.SetTrigger("jetpack");
				player.BubbleBoost();
			}
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }
		
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			Debug.Log(other.name + " has exited the bubble.");
			
			BubbleManager.Instance.SmallBubblePlayerIsOn = null;
			OxygenManager.Instance.OutOfBubble();
			CameraManager.Instance.Switch(CameraState.Ocean);
		}
	}

#endregion

}

