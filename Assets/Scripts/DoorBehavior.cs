using Managers;
using UnityEngine;

public class DoorBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private Sprite lockedSpacebarSprite, unlockedSpacebarSprite;
	[SerializeField] private DoorBehavior matchingDoor;
	[SerializeField] private bool locked;
	[SerializeField] private ushort keyNumber;
#endregion

#region Components
	private SpriteRenderer _spaceBarSpriteRenderer;
#endregion

#region Attributes
	public bool CanOpen => !locked || (locked && KeyManager.Instance.HasKey(keyNumber));
	public Vector3 TeleportLocation => matchingDoor.transform.position;
#endregion

#region Unity
    private void Awake() {
	    _spaceBarSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
	    
	    _spaceBarSpriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
	    if (other.TryGetComponent<PlayerBehavior>(out var player)) {
		    Debug.Log(other.name + " has touched the door.");

		    _spaceBarSpriteRenderer.enabled = true;
		    _spaceBarSpriteRenderer.sprite =
			    CanOpen ? unlockedSpacebarSprite : lockedSpacebarSprite;
		    
		    KeyManager.TouchedDoor = this;
		    
	    }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
	    if (other.TryGetComponent<PlayerBehavior>(out var player)) {
		    Debug.Log(other.name + " has left the door.");

		    _spaceBarSpriteRenderer.enabled = false;
		    KeyManager.TouchedDoor = null;
	    }
    }
#endregion

}

