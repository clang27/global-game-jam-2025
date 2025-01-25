using UnityEngine;

public class CoinManager : MonoBehaviour {

#region Dependencies
    [SerializeField] private GameObject coinPrefab, chestPrefab;
#endregion

#region Attributes
    // public static GameManager Instance { get; private set; }
#endregion

#region Components
    private Transform _transform;
#endregion

#region Data
    private int _coins = 0;
#endregion

#region Unity
    private void Awake() {
        _transform = transform;
    }

    private void Start() {
        UiManager.Instance.SetCoins(_coins);
    }

    private void Update() {
        
    }
	
    private void FixedUpdate() {
        
    }
#endregion

#region Custom
    public void AddCoin() {
        _coins++;
        UiManager.Instance.SetCoins(_coins);
    }
    
    public void AddChest() {
        _coins+=5;
        UiManager.Instance.SetCoins(_coins);
    }
#endregion

}