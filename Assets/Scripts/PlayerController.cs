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
	public void OnMove(InputValue value) {
		_character.InputVector = value.Get<Vector2>();
	}
		
	public void OnAttack() {
		_character.Dash();
	}
#endregion

}

