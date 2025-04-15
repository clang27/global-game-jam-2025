using Managers;
using UnityEngine;

public class ChestOpen : MonoBehaviour {

#region Dependencies
	[SerializeField] private Sprite openSprite;
	[SerializeField] private AudioClip openSound;
#endregion

#region Attributes
	public bool Opened { get; private set; }
#endregion

#region Components
	private SpriteRenderer _chestSpriteRenderer, _swordSpriteRenderer;
#endregion

#region Unity

	private void Awake() {
		_chestSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		if (transform.childCount > 1) {
			_swordSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();	
		}
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if (Opened) {
			return;
		}

		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			Opened = true;
			AudioManager.Instance.PlaySfx(openSound);
		
			_chestSpriteRenderer.sprite = openSprite;
			if (_swordSpriteRenderer && !SaveManager.Instance.HasSword()) {
				_swordSpriteRenderer.enabled = true;	
			}
		}
	}
#endregion

}

