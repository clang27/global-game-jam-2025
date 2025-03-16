using Enemies;
using UnityEngine;

public class HitBox : MonoBehaviour {
	
#region Components
	private Transform _transform;
	private AttackBehavior _attackBehavior;
#endregion

#region Unity
	private void Awake() {
		_transform = transform;
		_attackBehavior = GetComponentInParent<AttackBehavior>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<EnemyBehavior>(out var enemy)) {
			Debug.Log(enemy.name + " has been hit");
			enemy.Hurt(_attackBehavior.EquippedWeapon, _transform.position);
		}
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			Debug.Log(player.name + " has been hit");
			player.Hurt(_attackBehavior.EquippedWeapon, _transform.position);
		}
		if (other.TryGetComponent<BarrelBehavior>(out var barrel)) {
			Debug.Log(barrel.name + " has been hit");
			barrel.Hurt(_attackBehavior.EquippedWeapon, _transform.position);
		}
		if (other.transform.parent.TryGetComponent<BreakableWallBehavior>(out var wall)) {
			Debug.Log(wall.name + " has been hit");
			wall.Hurt(_attackBehavior.EquippedWeapon, _transform.position);
		}
	}
#endregion

}

