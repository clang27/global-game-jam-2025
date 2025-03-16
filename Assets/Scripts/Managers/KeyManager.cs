using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Managers {
    public class KeyManager : MonoBehaviour, IManager {

    #region Attributes
        public static KeyManager Instance { get; private set; }
        public static DoorBehavior TouchedDoor { get; set; }
        public static ChestBehavior TouchedChest { get; set; }
        private List<ushort> KeyNumbers { get; set; }
    #endregion
    
    #region Unity
        private void Awake() {
            Instance = this;
        }
    #endregion

    #region Custom
        public void Init() {
            enabled = false;

            KeyNumbers = SaveManager.Instance.KeysCollected();
            TouchedDoor = null;
            TouchedChest = null;
            
            Debug.Log($"{KeyNumbers.Count} keys have been collected according to save data");
        }
        
        public void SceneChange(string sceneName) {}

        public bool HasKey(ushort num) {
            return KeyNumbers.Contains(num);
        }

        public void AddKey(ItemId id) {
            DoorBehavior.RemoveLock(id.number);
            KeyNumbers.Add(id.number);
        }
    #endregion

    }
}