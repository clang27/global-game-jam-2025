using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Managers;
using Scriptable;
using UnityEngine;

public class BarrelBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private ItemId itemId;
	[SerializeField] private List<BarrelItem> itemsStored; 
	[SerializeField] private AudioClip breakSound;
#endregion

#region Attributes
	public int Health { get; private set; } = 1;
#endregion

#region Components
	private Transform _transform;
	private SpriteRenderer _spriteRenderer;
	private Collider2D _collider;
	private ParticleSystem _particleSystem;
#endregion

#region Data
	private readonly List<Transform> _itemTransforms = new();
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<Collider2D>();
		_particleSystem = GetComponentInChildren<ParticleSystem>();

		foreach (var item in itemsStored) {
			for (var i = 0; i < item.count; i++) {
				var itemObject = Instantiate(item.gameObject, _transform);
				itemObject.transform.position = new Vector3(10000f, 10000f);
				_itemTransforms.Add(itemObject.transform);
			}
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
	public void Hurt(Weapon weapon) {
		Health -= weapon.Damage;

		if (Health <= 0) {
			AudioManager.Instance.PlaySfx(breakSound);
			
			foreach (var t in _itemTransforms) {
				t.SetParent(null);
				t.position = _transform.position;
				
				var forceDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.1f, 1f)).normalized;
				var forceStrength = Random.Range(2f, 8f);

				t.DOKill();
				t.DOMove((Vector2) t.position + (forceDirection * forceStrength), 0.5f).SetEase(Ease.OutQuad);
				t.GetComponent<ItemPickup>().DelayCollision();
			}
			
			_particleSystem.Play();
			SaveManager.Instance.ItemCollected(itemId);
			RemoveFromScene();
		}
	}
	
	private void RemoveFromScene() {
		_spriteRenderer.enabled = false;
		_collider.enabled = false;
	}

	private void AddToScene() {
		_spriteRenderer.enabled = true;
		_collider.enabled = true;
	}
#endregion

}

