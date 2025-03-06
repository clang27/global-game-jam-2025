using System;
using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

#region Dependencies
	[Header("Id")]
	[SerializeField] private ItemId itemId;
	[Header("Properties")]
	[SerializeField] private AudioClip pickupSound;
#endregion

#region Components
	private Transform _transform;
	private Sequence _danceSequence;
	private Action<ItemId> _action;
#endregion

#region Unity
	private void Awake() {
		_transform = transform;
		name = itemId.ToString();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }
			
		if (other.tag.Equals("Player")) {
			AudioManager.Instance.PlaySfx(pickupSound);
			_action.Invoke(itemId);
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
		if (SaveManager.Instance.HasBeenCollected(itemId)) {
			RemoveFromScene();
		} else {
			AddToScene();
		}
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
			ItemType.OxygenUpgrade => CoinManager.Instance.AddLoot,
			ItemType.Key           => KeyManager.Instance.AddKey,
			ItemType.Jetpack       => PlayerManager.Player.AddJetpack,
			ItemType.Weapon       => PlayerManager.Player.AddWeapon,
			_                      => throw new ArgumentOutOfRangeException()
		};

		_danceSequence = _transform.DOLocalJump(_transform.localPosition, 0.25f, 1, 2f);
		_danceSequence.SetLoops(-1);
	}
#endregion

}

