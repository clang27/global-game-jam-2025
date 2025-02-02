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
	private Transform _transform;
#endregion

#region Data
	// private Coroutine _marchOverCoroutine;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_character = GetComponent<CharacterBehavior>();
    }
#endregion

#region Custom
	public void OnMove(InputAction.CallbackContext context) {
		_character.InputVector = context.ReadValue<Vector2>();
	}
		
	public void OnDash(InputAction.CallbackContext context) {
		if (GameManager.Instance.GameState == GameState.Start) {
			if (context.started) {
				GameManager.Instance.StartGame();
			}
		} else if (GameManager.Instance.GameState == GameState.GameOver) {
			if (context.started) {
				GameManager.Instance.ResetGame();
			}
		} else if (GameManager.Instance.GameState == GameState.Wave || GameManager.Instance.GameState == GameState.Shop) {
			if (context.started) {
				_character.JetpackOn();	
			} else if(context.canceled) {
				_character.JetpackOff();	
			}
		}
	}
	
	// public void OnAttack() {
	// 	if (ShopManager.Instance.InWindow) { return; }
	// 	
	// 	if (GameManager.Instance.GameState == GameState.Wave) {
	// 		_character.Attack();	
	// 	}
	// }
#endregion

}

