using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;
using CameraState = Enums.CameraState;
using Vector3 = UnityEngine.Vector3;

public class BoostBubbleBehavior : MonoBehaviour {
	
#region Dependencies
	[Header("Boost")]
	[SerializeField] private float boostMaxSpeed = 40f;
	[SerializeField][Range(0f, 1f)] private float boostTime = 0.5f;
	[Header("SFX")]
	[SerializeField] private AudioClip leaveBubbleSound;
	[SerializeField] private AudioClip enterBubbleSound;
#endregion

#region Components
	private Transform _transform;
	private Animator _animator;
	private Transform _arrowTransform;
#endregion

#region Data
	private const float JetpackDelayTime = 0.25f;
	private bool _jetpackDelay;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_animator = GetComponent<Animator>();
		_arrowTransform = _transform.GetChild(1).GetChild(0);
    }

    private void FixedUpdate() {
	    _arrowTransform.right = PlayerManager.Player.PreviousNotZeroInputVector;
    }

    private void OnTriggerEnter2D(Collider2D other) {
		if (GameManager.Instance.GameState is GameState.Start or GameState.GameOver) { return; }
		
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			if (_jetpackDelay) return;
			
			//Debug.Log(other.name + " has landed on the boost bubble.");
				
			_jetpackDelay = true;
			OxygenManager.Instance.InBubble();
			AudioManager.Instance.PlaySfx(enterBubbleSound);
				
			var dir = player.BubbleBoost(boostMaxSpeed, boostTime);
			var lookRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: dir);
			DOVirtual.DelayedCall(JetpackDelayTime, () => _jetpackDelay = false);
			_transform.DORotate(lookRotation.eulerAngles + new Vector3(0f, 0f, 90f), 0.15f).SetEase(Ease.InOutCirc);
			_animator.SetTrigger("enterWithJetpack");
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (GameManager.Instance.GameState is GameState.Start or GameState.GameOver) { return; }
		
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			//Debug.Log(other.name + " has exited the bubble.");
			
			AudioManager.Instance.PlaySfx(leaveBubbleSound);
			OxygenManager.Instance.OutOfBubble();
			CameraManager.Instance.Switch(CameraState.Ocean, name);
		}
	}

#endregion

}

