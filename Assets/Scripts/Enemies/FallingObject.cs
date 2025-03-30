using DG.Tweening;
using Scriptable;
using UnityEngine;

public class FallingObject : MonoBehaviour {

#region Dependencies
	[SerializeField] private Weapon weapon;
	[SerializeField] private float fallDistance;
	[SerializeField] private float fallRotation;
#endregion

#region Components
	private Transform _transform;
	private Vector3 _startingPosition;
	private Quaternion _startingRotation;
	private Collider2D _collider;
#endregion

#region Data
	private Tween _fallTween, _rotateTween;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_collider = GetComponent<Collider2D>();
		
		_startingPosition = _transform.position;
		_startingRotation = _transform.rotation;

		_collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
	    if (other.TryGetComponent<PlayerBehavior>(out var player)) {
		    Debug.Log(player.name + " has been hit with a falling object");
		    player.Hurt(weapon, Vector2.down);
	    }
    }
#endregion

#region Custom
	public void Launch(float time) {
		_collider.enabled = true;
		
		_fallTween.Kill();
		_rotateTween.Kill();

		_rotateTween = _transform.DORotate(_startingRotation.eulerAngles - new Vector3(0f, 0f, fallRotation), time, RotateMode.FastBeyond360).SetEase(Ease.Linear);
		_fallTween = _transform.DOMoveY(_startingPosition.y - fallDistance, time).SetEase(Ease.Linear);
	}

	public void ResetPosition() {
		_collider.enabled = false;
		
		_fallTween.Kill();
		_rotateTween.Kill();
		
		_transform.SetPositionAndRotation(_startingPosition, _startingRotation);
	}
#endregion

}

