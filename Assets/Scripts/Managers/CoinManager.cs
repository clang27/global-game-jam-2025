using UnityEngine;

namespace Managers {
    public class CoinManager : MonoBehaviour, IManager {

        #region Dependencies
        [SerializeField] private GameObject coinPrefab, chestPrefab;
        #endregion

        #region Attributes
        public static CoinManager Instance { get; private set; }
        public int Coins { get; private set; }
        #endregion

        #region Components
        private Transform _transform;
        #endregion

        #region Data
        #endregion

        #region Unity
        private void Awake() {
            Instance = this;
        
            _transform = transform;
        }

        private void Update() {
        
        }
	
        private void FixedUpdate() {
        
        }
        #endregion

        #region Custom
        public void Init() {
            enabled = false;
            Coins = 0;
            //UiManager.Instance.SetCoins(Coins);
        }
    
        public void AddCoin(int amount) {
            Coins+=amount;
            //UiManager.Instance.SetCoins(Coins);
        }

        public bool HaveEnoughCoins(int amount) {
            return Coins >= amount;
        }
    
        public void SpendCoins(int amount) {
            Coins -= amount;
            //UiManager.Instance.SetCoins(Coins);
        }
        #endregion

    }
}