using UnityEngine;

public class TimeManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private float gameTime = 120f;
#endregion

#region Attributes
	public static TimeManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
#endregion

#region Data
	private float _timer;
#endregion

#region Unity
    private void Awake() {
	    Instance = this;
		_transform = transform;
    }
	
	private void FixedUpdate() {
		_timer -= Time.fixedDeltaTime;
		
		if (_timer <= 0f) {
			GameManager.Instance.GameOver(false);
			_timer = 0f;
		}
		
		UiManager.Instance.SetTimer(_timer/gameTime);
	}
#endregion

#region Custom
	public void Init() {
		enabled = false;
		UiManager.Instance.ShowTimer(false);
		_timer = gameTime;
	}

	public void StartGame() {
		enabled = true;
		UiManager.Instance.ShowTimer(true);
	}
#endregion

}

