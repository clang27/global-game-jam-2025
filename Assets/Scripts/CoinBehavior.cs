using UnityEngine;

public class CoinBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private AudioClip pickupSound;
	[SerializeField] private int worth = 1;
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
	if (GameManager.Instance.GameState == GameState.Start) { return; }
		
	if (other.tag.Equals("Player")) {
		AudioManager.Instance.PlaySfx(pickupSound);
		CoinManager.Instance.AddCoin(worth);
		Destroy(gameObject);
	}
}
#endregion

#region Custom
	private void DoFunction() {
		
	}
#endregion

}

