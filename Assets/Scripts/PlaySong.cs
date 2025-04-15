using Managers;
using UnityEngine;

public class PlaySong : MonoBehaviour {

#region Unity
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			AudioManager.Instance.PlayIntrigue();
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (other.TryGetComponent<PlayerBehavior>(out var player)) {
			AudioManager.Instance.PlayGameTheme();
		}
	}
#endregion

}

