using DG.Tweening;
using Scriptable;
using UnityEngine;

public class AttackBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private Weapon defaultWeapon;
	[SerializeField] private bool startOn;
#endregion

#region Attributes
	public Weapon EquippedWeapon { get; private set; }
	public bool OnCooldown { get; private set; }
	public bool Stunned { get; set; }
#endregion

#region Components
	private SpriteRenderer _weaponSpriteRenderer, _hitBoxSpriteRenderer;
	private Collider2D _hitBoxCollider;
	private Transform _hitBoxTransform;
#endregion

#region Unity
    private void Awake() {
	    _hitBoxTransform = transform.GetChild(0);
	    _weaponSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
	    _hitBoxSpriteRenderer = _hitBoxTransform.GetComponent<SpriteRenderer>();
	    _hitBoxCollider = _hitBoxTransform.GetComponent<Collider2D>();
    }

    private void Start() {
	    Init();
    }
#endregion

#region Custom
	public void Init() {
		EquipWeapon(defaultWeapon);
		_hitBoxSpriteRenderer.enabled = startOn;	
		_hitBoxCollider.enabled = startOn;
	}

	public void EquipWeapon(Weapon weapon) {
		if (weapon == null) {
			_weaponSpriteRenderer.sprite = null;
			EquippedWeapon = null;
		}
		else {
			_weaponSpriteRenderer.sprite = weapon.Sprite;
			EquippedWeapon = weapon;
		}
	}

	public void Activate() {
		if (OnCooldown) { return; }

		OnCooldown = true;
		
		DOVirtual.DelayedCall(EquippedWeapon.AttackSpeed, () => {
			OnCooldown = false;
		});
	}
#endregion

}

