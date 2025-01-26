using UnityEngine;

public class ShopManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private AudioClip successfulBuySound, failedBuySound, nextWaveSound;
	[SerializeField] private float shopTime = 15f;
#endregion

#region Attributes
	public static ShopManager Instance { get; private set; }
	public bool InWindow { get; set; }
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
	public void Init() {
		UiManager.Instance.ShowShopItems(false);
		UiManager.Instance.ShowTimer(false);
		InWindow = false;
	}
	public void StartShop() {
		InWindow = true;
		UiManager.Instance.ShowShopItems(true);
		UiManager.Instance.ShowTimer(true);
		_timer = shopTime;
		UiManager.Instance.SetTimer(_timer/shopTime);
	}

	public void StopShop() {
		InWindow = false;
		UiManager.Instance.ShowShopItems(false);
		UiManager.Instance.ShowTimer(false);
	}

	public void CloseShop() {
		InWindow = false;
		UiManager.Instance.ShowShopItems(false);
		
		AudioManager.Instance.PlaySfx(nextWaveSound);
	}

	public void BuyBubbleAir(int amount) {
		if (!CoinManager.Instance.HaveEnoughCoins(amount) || GameManager.Instance.Bubble.FullOfAir) {
			AudioManager.Instance.PlaySfx(failedBuySound);
			return;
		}
		
		AudioManager.Instance.PlaySfx(successfulBuySound);
		CoinManager.Instance.SpendCoins(amount);
		GameManager.Instance.Bubble.BuyAir();
	}
	
	public void BuyUpgrade(int amount) {
		if (!CoinManager.Instance.HaveEnoughCoins(amount)) {
			AudioManager.Instance.PlaySfx(failedBuySound);
			return;
		}
		
		AudioManager.Instance.PlaySfx(successfulBuySound);
		CoinManager.Instance.SpendCoins(amount);
		GameManager.Instance.Player.UpgradeDash();
	}
	
	public void BuyWeapon(int amount) {
		if (!CoinManager.Instance.HaveEnoughCoins(amount)) {
			AudioManager.Instance.PlaySfx(failedBuySound);
			return;
		}
		
		AudioManager.Instance.PlaySfx(successfulBuySound);
		CoinManager.Instance.SpendCoins(amount);
	}
#endregion

}

