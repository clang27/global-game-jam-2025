using Enums;
using UnityEngine;

namespace Managers {
    public class CoinManager : MonoBehaviour, IManager {
        
    #region Properties
        [SerializeField] private int coinValue = 1;
        [SerializeField] private int sapphireValue = 3;
        [SerializeField] private int emeraldValue = 6;
        [SerializeField] private int rubyValue = 9;
    #endregion

    #region Attributes
        public static CoinManager Instance { get; private set; }
        public int Dollars { get; private set; }
    #endregion
    
    #region Unity
        private void Awake() {
            Instance = this;
        }
    #endregion

    #region Custom
        public void Init() {
            enabled = false;

            Dollars = 0;
            AddLoot(ItemType.Coin, SaveManager.Instance.NumberOfItem(ItemType.Coin));
            AddLoot(ItemType.Emerald, SaveManager.Instance.NumberOfItem(ItemType.Emerald));
            AddLoot(ItemType.Ruby, SaveManager.Instance.NumberOfItem(ItemType.Ruby));
            AddLoot(ItemType.Sapphire, SaveManager.Instance.NumberOfItem(ItemType.Sapphire));
            
            Debug.Log($"${Dollars} has been collected according to save data");
        }

        public void AddLoot(ItemType type) {
            AddLoot(type, 1);
        }
    
        private void AddLoot(ItemType type, int amount) {
            switch (type) {
                case ItemType.Coin:
                    Dollars += coinValue * amount;
                    break;
                case ItemType.Emerald:
                    Dollars += emeraldValue * amount;
                    break;
                case ItemType.Ruby:
                    Dollars += rubyValue * amount;
                    break;
                case ItemType.Sapphire:
                    Dollars += sapphireValue * amount;
                    break;
            }
        }
    #endregion

    }
}