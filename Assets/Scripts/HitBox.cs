using Enemies;
using UnityEngine;

public class HitBox : MonoBehaviour {
	
#region Dependencies
	[SerializeField] private bool directionOverride;
	[SerializeField] private Vector2 direction = Vector2.up;
#endregion
	
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
		if (_attackBehavior.Stunned) {
			return;
		}
		
		var dir = directionOverride ? 
			direction :
			((Vector2) other.transform.position - (Vector2) _transform.position).normalized;
		
		if (other.TryGetComponent<EnemyBehavior>(out var enemy)) {
			Debug.Log(enemy.name + " has been hit");
			enemy.Hurt(_attackBehavior.EquippedWeapon, dir);
		}
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			Debug.Log(player.name + " has been hit");
			player.Hurt(_attackBehavior.EquippedWeapon, dir);
		}
		if (other.TryGetComponent<BarrelBehavior>(out var barrel)) {
			Debug.Log(barrel.name + " has been hit");
			barrel.Hurt(_attackBehavior.EquippedWeapon);
		}

		if (other.transform.parent) {
			if (other.transform.parent.TryGetComponent<BreakableWallBehavior>(out var wall)) {
				Debug.Log(wall.name + " has been hit");
				wall.Hurt(_attackBehavior.EquippedWeapon);
			}	
		}
	}
#endregion

}

