using Managers;
using Scriptable;
using UnityEngine;

public class BreakableWallBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private AudioClip breakSound;
#endregion

#region Attributes
	public int Health { get; private set; } = 1;
#endregion

#region Components
	private SpriteRenderer _spriteRenderer;
	private Collider2D[] _colliders;
	private ParticleSystem _particleSystem;
#endregion

#region Unity
    private void Awake() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_colliders = GetComponentsInChildren<Collider2D>();
		_particleSystem = GetComponentInChildren<ParticleSystem>();
    }
#endregion

#region Custom
	public void Hurt(Weapon weapon) {
		Health -= weapon.Damage;

		if (Health <= 0) {
			AudioManager.Instance.PlaySfx(breakSound);
			
			_particleSystem.Play();
			RemoveFromScene();
		}
	}
	
	private void RemoveFromScene() {
		_spriteRenderer.enabled = false;
		foreach (var c in _colliders) {
			c.enabled = false;	
		}
	}

	private void AddToScene() {
		_spriteRenderer.enabled = true;
		foreach (var c in _colliders) {
			c.enabled = false;	
		}
	}
#endregion

}

