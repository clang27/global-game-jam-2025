using UnityEngine;

public class ShopManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private float shopTime = 15f;
#endregion

#region Attributes
	public static ShopManager Instance { get; private set; }
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
			GameManager.Instance.ShopDone();
			_timer = 0f;
		}
		
		UiManager.Instance.SetTimer(_timer/shopTime);
	}
#endregion

#region Custom
	public void StartShop() {
		UiManager.Instance.ShowTimer(true);
		_timer = shopTime;
		UiManager.Instance.SetTimer(_timer/shopTime);
	}

	public void StopShop() {
		UiManager.Instance.ShowTimer(false);
	}
#endregion

}

