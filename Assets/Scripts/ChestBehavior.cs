using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;

public class ChestBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private ItemId itemId;
	[SerializeField] private List<BarrelItem> itemsStored;
	[SerializeField] private Sprite openSprite, closeSprite;
	[SerializeField] private AudioClip openSound;
#endregion

#region Components
	private Transform _transform;
	private SpriteRenderer _spriteRenderer, _spaceBarSpriteRenderer;
	private Collider2D _collider;
#endregion

#region Data
	private readonly List<Transform> _itemTransforms = new();
#endregion

#region Unity
    private void Awake() {
		_transform = transform;

		foreach (var item in itemsStored) {
			for (var i = 0; i < item.count; i++) {
				var itemObject = Instantiate(item.gameObject, _transform);
				itemObject.transform.position = new Vector3(-10000f, -10000f);
				_itemTransforms.Add(itemObject.transform);
			}
		}

		_collider = GetComponent<Collider2D>();
		_spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		_spaceBarSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
		_spaceBarSpriteRenderer.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
	    if (other.TryGetComponent<PlayerBehavior>(out var player)) {
		    Debug.Log(other.name + " has touched the chest.");

		    KeyManager.TouchedChest = this;
		    
		    _spaceBarSpriteRenderer.enabled = true;
	    }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
	    if (other.TryGetComponent<PlayerBehavior>(out var player)) {
		    Debug.Log(other.name + " has left the chest.");

		    KeyManager.TouchedChest = null;
		    _spaceBarSpriteRenderer.enabled = false;
	    }
    }
#endregion

#region Custom
	public void Init() {
		if (SaveManager.Instance.HasBeenCollected(itemId)) {
			RemoveFromScene();
		} else {
			AddToScene();
		}
	}

	public void Open() {
		AudioManager.Instance.PlaySfx(openSound);
			
		foreach (var t in _itemTransforms) {
			t.SetParent(null);
			t.position = _transform.position;
				
			var forceDirection = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
			var forceStrength = Random.Range(4f, 12f);

			t.DOKill();
			t.DOMove((Vector2) t.position + (forceDirection * forceStrength), 0.5f).SetEase(Ease.OutQuad);
			t.GetComponent<ItemPickup>().DelayCollision();
		}
			
		SaveManager.Instance.ItemCollected(itemId);
		RemoveFromScene();
	}
	
	private void RemoveFromScene() {
		_spriteRenderer.sprite = openSprite;
		_collider.enabled = false;
		_spaceBarSpriteRenderer.gameObject.SetActive(false);
	}

	private void AddToScene() {
		_spriteRenderer.sprite = closeSprite;
		_collider.enabled = true;
		_spaceBarSpriteRenderer.gameObject.SetActive(true);
	}
#endregion

}

