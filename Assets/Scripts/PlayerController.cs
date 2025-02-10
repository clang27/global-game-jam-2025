using System;
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
	private CharacterBehavior _character;
	private PlayerInput _playerInput;
#endregion

#region Data
	private bool _inUi;
#endregion

#region Unity
    private void Awake() {
		_character = GetComponent<CharacterBehavior>();
		_playerInput = GetComponent<PlayerInput>();
    }
#endregion

#region Custom
	public void GoToUiControls() {
		Debug.Log("Going to UI controls");
		
		_character.InputVector = Vector2.zero;
		_character.JetpackOff();

		_inUi = true;
	}
	
	public void GoToPlayerControls() {
		Debug.Log("Going to Player controls");

		_inUi = false;
	}
	
	public void OnMove(InputAction.CallbackContext context) {
		if (_inUi) { return; }
		
		_character.InputVector = context.ReadValue<Vector2>();
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
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
			
	public void OnJetpack(InputAction.CallbackContext context) {
		if (_inUi) { return; }
		
		if (context.started) {
			_character.JetpackOn();	
		} else if(context.canceled) {
			_character.JetpackOff();	
		}
	}
	
	public void OnAttack(InputAction.CallbackContext context) {
		if (_inUi) { return; }
		if (!context.started) { return; }
		
		_character.Attack();	
	}
#endregion

}

