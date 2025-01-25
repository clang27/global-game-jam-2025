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
	public void EquipWeapon(Weapon w) {
		EquippedWeapon = w;
		_hitBoxCollider.size = w.HitboxSize;
		_weaponSpriteRenderer.sprite = w.Sprite;
	}
	public void Init() {
		EquipWeapon(defaultWeapon);
		_hitBoxSpriteRenderer.enabled = false;
		_hitBoxCollider.enabled = false;
	}
	public void Activate(Direction direction) {
		if (OnCooldown) { return; }

		OnCooldown = true;
		_hitBoxSpriteRenderer.enabled = true;
		_hitBoxCollider.enabled = true;

		DOVirtual.DelayedCall(EquippedWeapon.ActiveTime, () => {
			_hitBoxSpriteRenderer.enabled = false;
			_hitBoxCollider.enabled = false;
		});
		
		DOVirtual.DelayedCall(EquippedWeapon.AttackSpeed, () => {
			OnCooldown = false;
		});

		switch (direction) {
			case Direction.Down:
				_hitBoxTransform.localPosition = new Vector3(0f, -1.59f, 0f);
				_hitBoxTransform.eulerAngles = new Vector3(0f, 0f, 270f);
				DirectionAttacking = Vector2.down;
				break;
			case Direction.Up:
				_hitBoxTransform.localPosition = new Vector3(0f, 1.59f, 0f);
				_hitBoxTransform.eulerAngles = new Vector3(0f, 0f, 90f);
				DirectionAttacking = Vector2.up;
				break;
			case Direction.Right:
				_hitBoxTransform.localPosition = new Vector3(1.59f, 0f, 0f);
				_hitBoxTransform.eulerAngles = new Vector3(0f, 0f, 0f);
				DirectionAttacking = Vector2.right;
				break;
			case Direction.Left:
				_hitBoxTransform.localPosition = new Vector3(-1.59f, 0f, 0f);
				_hitBoxTransform.eulerAngles = new Vector3(0f, 0f, 180f);
				DirectionAttacking = Vector2.left;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
		}
	}
#endregion

}

