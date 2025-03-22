using UnityEngine;

public class Wind : MonoBehaviour {

#region Dependencies
	[SerializeField] private float strength = 1f;
	[SerializeField] private Vector2 forceDirection = Vector2.down;
#endregion

#region Attributes
	// public static GameManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
#endregion

#region Data
	// private Coroutine _marchOverCoroutine;
#endregion

#region Unity
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			player.ExternalForces.Add(strength * forceDirection);
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			player.ExternalForces.Remove(strength * forceDirection);
		}
	}
#endregion

#region Custom
	private void DoFunction() {
		
	}
#endregion

}

