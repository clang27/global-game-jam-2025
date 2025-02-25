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
	private Action<ItemType> _action;
#endregion

#region Unity
	private void Awake() {
		_transform = transform;
		name = itemId.ToString();
	}

	private void Start() {
		if (SaveManager.Instance.HasBeenCollected(itemId)) {
			RemoveFromScene();
		}
		else {
			_action = itemId.type switch {
				ItemType.Coin          => CoinManager.Instance.AddLoot,
				ItemType.Emerald       => CoinManager.Instance.AddLoot,
				ItemType.Ruby          => CoinManager.Instance.AddLoot,
				ItemType.Sapphire      => CoinManager.Instance.AddLoot,
				ItemType.OxygenUpgrade => CoinManager.Instance.AddLoot,
				_                    => throw new ArgumentOutOfRangeException()
			};

			_danceSequence = _transform.DOLocalJump(_transform.localPosition, 0.25f, 1, 2f);
			_danceSequence.SetLoops(-1);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }
			
		if (other.tag.Equals("Player")) {
			AudioManager.Instance.PlaySfx(pickupSound);
			_action.Invoke(itemId.type);
			RemoveFromScene();
			SaveManager.Instance.ItemCollected(itemId);
		}
	}
	
	private void OnDestroy() {
		_danceSequence?.Kill();
	}
#endregion

#region Custom
	private void RemoveFromScene() {
		_danceSequence?.Kill();
		gameObject.SetActive(false);
	}
#endregion

}

