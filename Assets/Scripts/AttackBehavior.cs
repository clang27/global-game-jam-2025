using System;
using DG.Tweening;
using UnityEngine;

public class AttackBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private Weapon defaultWeapon;
#endregion

#region Attributes
	public Weapon EquippedWeapon { get; private set; }
	public Vector2 DirectionAttacking { get; private set; }
	public bool OnCooldown { get; private set; }
#endregion

#region Components
	private SpriteRenderer _weaponSpriteRenderer, _hitBoxSpriteRenderer;
	private BoxCollider2D _hitBoxCollider;
	private Transform _hitBoxTransform;
#endregion

#region Unity
    private void Awake() {
	    _hitBoxTransform = transform.GetChild(0);
	    _weaponSpriteRenderer = GetComponent<SpriteRenderer>();
	    _hitBoxSpriteRenderer = _hitBoxTransform.GetComponent<SpriteRenderer>();
	    _hitBoxCollider = _hitBoxTransform.GetComponent<BoxCollider2D>();
    }
#endregion

#region Custom
	public void ResetWeapon() {
		EquippedWeapon = ScriptableObject.CreateInstance<Weapon>();
		EquippedWeapon.AttackSpeed = defaultWeapon.AttackSpeed;
		EquippedWeapon.Knockback = defaultWeapon.Knockback;
		EquippedWeapon.Sprite = defaultWeapon.Sprite;
		EquippedWeapon.StunTime = defaultWeapon.StunTime;
		EquippedWeapon.Sound = defaultWeapon.Sound;
		
		_weaponSpriteRenderer.sprite = defaultWeapon.Sprite;
	}
	public void Init() {
		ResetWeapon();
		_hitBoxSpriteRenderer.enabled = false;
		_hitBoxCollider.enabled = false;
	}
	public void Activate(Direction direction) {
		if (OnCooldown) { return; }

		OnCooldown = true;
		
		DOVirtual.DelayedCall(EquippedWeapon.AttackSpeed, () => {
			OnCooldown = false;
		});

		switch (direction) {
			case Direction.Down:
				DirectionAttacking = Vector2.down;
				break;
			case Direction.Up:
				DirectionAttacking = Vector2.up;
				break;
			case Direction.Right:
				DirectionAttacking = Vector2.right;
				break;
			case Direction.Left:
				DirectionAttacking = Vector2.left;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
		}
	}
#endregion

}

