using DG.Tweening;
using Scriptable;
using UnityEngine;

public class AttackBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private Weapon defaultWeapon;
#endregion

#region Attributes
	public Weapon EquippedWeapon { get; private set; }
	public bool OnCooldown { get; private set; }
#endregion

#region Components
	private SpriteRenderer _weaponSpriteRenderer, _hitBoxSpriteRenderer;
	private Collider2D _hitBoxCollider;
	private Transform _hitBoxTransform;
#endregion

#region Unity
    private void Awake() {
	    _hitBoxTransform = transform.GetChild(0);
	    _weaponSpriteRenderer = GetComponent<SpriteRenderer>();
	    _hitBoxSpriteRenderer = _hitBoxTransform.GetComponent<SpriteRenderer>();
	    _hitBoxCollider = _hitBoxTransform.GetComponent<Collider2D>();
    }

    private void Start() {
	    Init();
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
		EquippedWeapon.Damage = defaultWeapon.Damage;
		EquippedWeapon.Shock = defaultWeapon.Shock;
		
		_weaponSpriteRenderer.sprite = defaultWeapon.Sprite;
	}
	public void Init() {
		ResetWeapon();
		
		_hitBoxSpriteRenderer.enabled = false;	
		_hitBoxCollider.enabled = false;
	}

	public void TurnOnHitBox() {
		_hitBoxSpriteRenderer.enabled = true;	
		_hitBoxCollider.enabled = true;
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

