using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LootVacuum : MonoBehaviour {

#region Dependencies
	[SerializeField] private float strength = 10f;
	[SerializeField] private float maxPullSpeed = 20f;
#endregion

#region Components
	private Transform _transform;
	private readonly List<Rigidbody2D> _rigidBodies = new();
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
    }

    private void FixedUpdate() {
	    for (var index = 0; index < _rigidBodies.Count; index++) {
		    var rb = _rigidBodies[index];
		    if (rb && rb.gameObject.activeInHierarchy) {
			    var direction = ((Vector2) _transform.position - (Vector2) rb.transform.position).normalized;
			    var distance = Vector2.Distance(_transform.position, rb.transform.position);
			    rb.AddForce(direction * (strength * distance));
			    rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxPullSpeed);
		    } else {
			    _rigidBodies.RemoveAt(index);
		    }
	    }
    }

    private void OnTriggerEnter2D(Collider2D other) {
	    Debug.Log(other.name);
	    _rigidBodies.Add(other.attachedRigidbody);
	    other.transform.DOKill();
    }
#endregion


}

