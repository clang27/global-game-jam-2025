using System;
using DG.Tweening;
using UnityEngine;

public class LootVacuum : MonoBehaviour {

#region Dependencies
	[SerializeField] private float strength = 10f;
#endregion

#region Components
	private Transform _transform;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
    }

    private void OnTriggerEnter2D(Collider2D other) {
	    other.transform.DOKill();
    }

    private void OnTriggerExit2D(Collider2D other) {
	    other.attachedRigidbody.linearVelocity = Vector2.zero;
	    other.transform.DORestart();
    }

    private void OnTriggerStay2D(Collider2D other) {
	    var direction = ((Vector2) _transform.position - (Vector2) other.transform.position).normalized;
	    other.attachedRigidbody.AddForce(direction * strength);
    }
#endregion


}

