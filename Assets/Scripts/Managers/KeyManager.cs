using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Managers {
    public class KeyManager : MonoBehaviour, IManager {
        
    #region Dependencies
        [SerializeField] private GameObject[] keyUiImages;
    #endregion

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
            foreach (var image in keyUiImages) {
               image.transform.GetChild(0).gameObject.SetActive(true);
               image.transform.GetChild(1).gameObject.SetActive(false);
            }
            foreach (var number in KeyNumbers) {
                keyUiImages[number].transform.GetChild(0).gameObject.SetActive(false);
                keyUiImages[number].transform.GetChild(1).gameObject.SetActive(true);
            }
            
            TouchedDoor = null;
            TouchedChest = null;
            
            Debug.Log($"{KeyNumbers.Count} keys have been collected according to save data");
        }

        public void SceneChange(string sceneName) {
            foreach (var d in FindObjectsByType<DoorBehavior>(FindObjectsSortMode.None)) {
                d.AddLock();
            }
        }

        public bool HasKey(ushort num) {
            return KeyNumbers.Contains(num);
        }

        public void AddKey(ItemId id) {
            DoorBehavior.RemoveLock(id.number);
            KeyNumbers.Add(id.number);
            keyUiImages[id.number].transform.GetChild(0).gameObject.SetActive(false);
            keyUiImages[id.number].transform.GetChild(1).gameObject.SetActive(true);
        }
    #endregion

    }
}