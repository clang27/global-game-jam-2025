using System;
using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ItemPickup : MonoBehaviour {

#region Dependencies
	[Header("Id")]
	[SerializeField] private ItemId itemId;
	[Header("Properties")]
	[SerializeField] private AudioClip pickupSound;
	[SerializeField] private UnityEvent onPickup;
	[SerializeField] [Range(0f, 2f)] private float jumpPower = 0.25f;
#endregion
	
#region Attributes
	public bool InBarrel => _transform.parent.name.Contains("Barrel");
	public bool InChest => _transform.parent.name.Contains("Chest");
#endregion

#region Components
	private Transform _transform;
	private Collider2D _collider;
	private Sequence _danceSequence;
	private Action<ItemId> _action;
#endregion

#region Unity
	private void Awake() {
		_transform = transform;
		_collider = GetComponent<Collider2D>();
		name = itemId.ToString();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }

		if (other.tag.Equals("Player")) {
			AudioManager.Instance.PlaySfx(pickupSound);
			_action.Invoke(itemId);
			onPickup.Invoke();
			RemoveFromScene();
			SaveManager.Instance.ItemCollected(itemId);
		}
	}
	
	private void OnDestroy() {
		_danceSequence?.Kill();
	}
#endregion

#region Custom
	public void Init() {
		if (!_transform.parent) { // Was in a barrel or chest and now is lingering
			Destroy(gameObject);
		} else if (!InBarrel && !InChest && SaveManager.Instance.HasBeenCollected(itemId)) {
			RemoveFromScene();
		} else {
			AddToScene();
		}
	}

	public void DelayCollision() {
		_collider.enabled = false;
		DOVirtual.DelayedCall(Random.Range(0.3f, 0.6f), () => _collider.enabled = true);
	}
	
	private void RemoveFromScene() {
		_danceSequence?.Kill();
		gameObject.SetActive(false);
	}

	private void AddToScene() {
		gameObject.SetActive(true);
		
		_action = itemId.type switch {
			ItemType.Coin          => CoinManager.Instance.AddLoot,
			ItemType.Emerald       => CoinManager.Instance.AddLoot,
			ItemType.Ruby          => CoinManager.Instance.AddLoot,
			ItemType.Sapphire      => CoinManager.Instance.AddLoot,
			ItemType.OxygenUpgrade => OxygenManager.Instance.Upgrade,
			ItemType.Key           => KeyManager.Instance.AddKey,
			ItemType.Jetpack       => PlayerManager.Player.AddJetpack,
			ItemType.Weapon        => PlayerManager.Player.AddWeapon,
			_                      => throw new ArgumentOutOfRangeException()
		};

		if (!InBarrel && !InChest) {
			_danceSequence = _transform.DOLocalJump(_transform.localPosition, jumpPower, 1, 2f);
			_danceSequence.SetLoops(-1);	
		}
	}
#endregion

}

