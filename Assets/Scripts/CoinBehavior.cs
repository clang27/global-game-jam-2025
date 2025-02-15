using DG.Tweening;
using Enums;
using Managers;
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
	private Sequence _danceSequence;
#endregion

#region Data
	// private Coroutine _marchOverCoroutine;
#endregion

#region Unity
	private void Awake() {
		_transform = transform;
	}

	private void Start() {
		_danceSequence = _transform.DOLocalJump(_transform.localPosition, 0.25f, 1, 2f);
		_danceSequence.SetLoops(-1);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }
			
		if (other.tag.Equals("Player")) {
			AudioManager.Instance.PlaySfx(pickupSound);
			CoinManager.Instance.AddCoin(worth);
			Destroy(gameObject);
		}
	}
	
	private void OnDestroy() {
		_danceSequence.Kill();
	}
#endregion

#region Custom
	private void DoFunction() {
		
	}
#endregion

}

