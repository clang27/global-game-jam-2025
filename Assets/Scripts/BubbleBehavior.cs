using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BubbleBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private float shrinkSpeed = 0.0001f;
	[SerializeField] private float popPercent = 0.2f;
	[SerializeField] private float shrinkThreshold = 1f;
	[SerializeField] private float topSpeed = 2f;
#endregion

#region Attributes
	public Vector2 Velocity => _rigidbody.linearVelocity;
	public float Radius => _collider.bounds.extents.x;
	private Vector3 ShrinkRate => Mathf.Sqrt(_playersOnBubble
		.Count(p => 
			p.tag.Equals("Enemy") && p.TimeOnBubble >= shrinkThreshold)) * shrinkSpeed * Vector3.one;

	public bool FullOfAir => _transform.localScale.Equals(_startingScale);
#endregion

#region Components
	private Transform _transform;
	private Rigidbody2D _rigidbody;
	private CircleCollider2D _collider;
#endregion

#region Data
	private Vector3 _startingPosition, _startingScale;
	private readonly List<CharacterBehavior> _playersOnBubble = new(); 
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponentInChildren<CircleCollider2D>();
		_startingPosition = _transform.position;
		_startingScale = _transform.localScale;
    }
	
	private void FixedUpdate() {
		foreach (var player in _playersOnBubble.Where(p => p.tag.Equals("Enemy"))) {
			player.TimeOnBubble += Time.fixedDeltaTime;
		}
		
		_transform.localScale -= ShrinkRate;
		
		UiManager.Instance.SetBubble((_transform.localScale.x - (popPercent * _startingScale.x)) / (_startingScale.x - (popPercent * _startingScale.x)));

		if (_transform.localScale.x / _startingScale.x <= popPercent) {
			_playersOnBubble.Clear();
			_transform.localScale = Vector2.zero;
			_collider.enabled = false;
			GameManager.Instance.GameOver(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }
		
		if (other.TryGetComponent<CharacterBehavior>(out var player)) {
			Debug.Log(other.name + " has landed on the bubble.");
			_playersOnBubble.Add(player);
			player.EnterBubble();
			if (player.tag.Equals("Player")) {
				GameManager.Instance.PlayerInBubble();
				player.JetpackOff();
			} else {
				player.GetComponent<AiController>().Landed();
			}
			
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (GameManager.Instance.GameState == GameState.Start) { return; }
		
		if (other.TryGetComponent<CharacterBehavior>(out var player)) {
			Debug.Log(other.name + " has exited the bubble.");
			_playersOnBubble.Remove(player);
			var direction = (player.transform.position - _transform.position).normalized;
			player.Eject(direction);
			if (player.tag.Equals("Player")) {
				GameManager.Instance.PlayerOutOfBubble();
			}
		}
	}

#endregion

#region Custom
	public void BuyAir() {
		_transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);
		if (_transform.localScale.x > _startingScale.x) {
			_transform.localScale = _startingScale;
		}
		
		UiManager.Instance.SetBubble((_transform.localScale.x - (popPercent * _startingScale.x)) / (_startingScale.x - (popPercent * _startingScale.x)));
	}
	
	public bool OnBubble(CharacterBehavior characterBehavior) {
		return _playersOnBubble.Contains(characterBehavior);
	}
	
	public void Init() {
		_playersOnBubble.Clear();
		_playersOnBubble.Add(GameManager.Instance.Player);
		_collider.enabled = true;
		
		_transform.localScale = _startingScale;
		_transform.position = _startingPosition;
		StopMoving();
	}
	
	public void StartMoving() {
		_rigidbody.linearVelocityY = topSpeed;
	}
	
	public void StopMoving() {
		_rigidbody.linearVelocityY = 0f;
	}
	
#endregion

}

