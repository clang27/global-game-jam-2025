using System;
using DG.Tweening;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	// public static GameManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
	private AiController _aiController;
#endregion

#region Data
	// private Coroutine _marchOverCoroutine;
#endregion

#region Unity

	private void Awake() {
		_transform = transform;
		_aiController = GetComponentInParent<AiController>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<CharacterBehavior>(out var player)) {
			if (player.tag.Equals("Player")) {
				Debug.Log(other.name + " has been hit");

				var direction = _aiController.Type == AiStyle.Jellyfish
					? -_aiController.Direction
					: _aiController.Direction;
				
				player.Knockback(direction, _aiController.Knockback);
				player.Stun();
				DOVirtual.DelayedCall(_aiController.StunTime, () => player.Unstun());
			}
		}
	}
#endregion

#region Custom
	private void DoFunction() {
		
	}
#endregion

}

