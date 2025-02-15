using Enums;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	// public static GameManager Instance { get; private set; }
#endregion

#region Components
	private PlayerBehavior _player;
	private PlayerInput _playerInput;
#endregion

#region Data
	private bool _inUi;
	private Vector2 _storedInput;
#endregion

#region Unity
    private void Awake() {
		_player = GetComponent<PlayerBehavior>();
		_playerInput = GetComponent<PlayerInput>();
    }
#endregion

#region Custom
	public void GoToUiControls() {
		Debug.Log("Going to UI controls");
		
		_inUi = true;
	}
	
	public void GoToPlayerControls() {
		Debug.Log("Going to Player controls");

		_inUi = false;
		_player.InputVector = _storedInput;
	}
	
	public void OnMove(InputAction.CallbackContext context) {
		_storedInput = context.ReadValue<Vector2>();
		
		if (_inUi) { return; }
		
		_player.InputVector = _storedInput;
	}

	public void OnSubmit(InputAction.CallbackContext context) {
        if (!_inUi) { return; }
        if (!context.started) { return; }
        
		switch (GameManager.Instance.GameState) {
			case GameState.Start:
				GameManager.Instance.StartGame();
				break;
			case GameState.GameOver:
				GameManager.Instance.ResetGame();
				break;
		}
	}

	public void OnPause(InputAction.CallbackContext context) {
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
		if (_inUi) { return; }
		
		if (context.started) {
			_player.JetpackOn();	
		} else if(context.canceled) {
			_player.JetpackOff();	
		}
	}
	
	public void OnAttack(InputAction.CallbackContext context) {
		if (_inUi) { return; }
		if (!context.started) { return; }
		
		_player.Attack();	
	}
#endregion

}

