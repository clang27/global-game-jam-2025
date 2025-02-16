using Enums;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	public bool Enabled {
		get => _enabled;
		set {
			_enabled = value;
			_player.InputVector = _enabled ? _storedInput : Vector2.zero;
		}
	}
	
	public bool InUi {
		get => _inUi;
		set {
			_inUi = value;
			_player.InputVector = _enabled ? _storedInput : Vector2.zero;
		}
	}
#endregion

#region Components
	private PlayerBehavior _player;
	private PlayerInput _playerInput;
#endregion

#region Data
	private Vector2 _storedInput;
	private bool _enabled;
	private bool _inUi;
#endregion

#region Unity
    private void Awake() {
		_player = GetComponent<PlayerBehavior>();
		_playerInput = GetComponent<PlayerInput>();
    }
#endregion

#region Custom
	public void OnMove(InputAction.CallbackContext context) {
		_storedInput = context.ReadValue<Vector2>();
		
		if (InUi) { return; }
		if (!Enabled) { return; }
		
		_player.InputVector = _storedInput;
	}

	public void OnPause(InputAction.CallbackContext context) {
		if (!Enabled) { return; }
		if (!context.started) { return; }
		
		switch (GameManager.Instance.GameState) {
			case GameState.Playing:
				GameManager.Instance.Pause();
				break;
			case GameState.Paused:
				GameManager.Instance.Unpause();
				break;
		}
	}
			
	public void OnJetpack(InputAction.CallbackContext context) {
		if (InUi) { return; }
		if (!Enabled) { return; }
		
		if (context.started) {
			_player.JetpackOn();	
		} else if(context.canceled) {
			_player.JetpackOff();	
		}
	}
	
	public void OnAttack(InputAction.CallbackContext context) {
		if (InUi) { return; }
		if (!Enabled) { return; }
		if (!context.started) { return; }
		
		_player.Attack();	
	}
#endregion

}

