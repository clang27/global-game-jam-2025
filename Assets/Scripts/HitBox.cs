using DG.Tweening;
using UnityEngine;

public class HitBox : MonoBehaviour {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	// public static GameManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
	private AttackBehavior _attackBehavior;
#endregion

#region Data
	// private Coroutine _marchOverCoroutine;
#endregion

#region Unity

	private void Awake() {
		_attackBehavior = GetComponentInParent<AttackBehavior>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			if (player.tag.Equals("Enemy")) {
				Debug.Log(other.name + " has been hit");
				player.Knockback(_attackBehavior.DirectionAttacking, _attackBehavior.EquippedWeapon.Knockback);
				player.Stun(false);
				DOVirtual.DelayedCall(_attackBehavior.EquippedWeapon.StunTime, () => player.Unstun());
			}
		}
	}
#endregion

#region Custom
	private void DoFunction() {
		
	}
#endregion

}

