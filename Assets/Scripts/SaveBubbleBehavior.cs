using Enums;
using Managers;
using UnityEngine;
using CameraState = Enums.CameraState;

public class SaveBubbleBehavior : MonoBehaviour {

#region Dependencies
	[Header("SFX")]
	[SerializeField] private AudioClip leaveBubbleSound;
	[SerializeField] private AudioClip enterBubbleSound;
#endregion
	
#region Components
	private Transform _transform;
	private Animator _animator;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_animator = GetComponent<Animator>();
    }

	private void OnTriggerEnter2D(Collider2D other) {
		if (GameManager.Instance.GameState is GameState.Start or GameState.GameOver) { return; }
		
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			//Debug.Log(other.name + " has landed on the save bubble.");

			BubbleManager.Instance.SmallBubblePlayerIsOn = this;
			OxygenManager.Instance.InBubble();
			SaveManager.Instance.Save(this);
			AudioManager.Instance.PlaySfx(enterBubbleSound);
			
			player.JetpackOff(false);
			_animator.SetTrigger("enterWithoutJetpack");
			CameraManager.Instance.Switch(CameraState.Bubble, name, _transform);
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (GameManager.Instance.GameState is GameState.Start or GameState.GameOver) { return; }
		
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			//Debug.Log(other.name + " has exited the bubble.");
			
			_animator.SetTrigger("exitWithoutJetpack");
			AudioManager.Instance.PlaySfx(leaveBubbleSound);
			BubbleManager.Instance.SmallBubblePlayerIsOn = null;
			OxygenManager.Instance.OutOfBubble();
			CameraManager.Instance.Switch(CameraState.Ocean, name);
		}
	}

#endregion

}

