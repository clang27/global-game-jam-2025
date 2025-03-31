using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;

namespace Managers {
    public class CoinManager : MonoBehaviour, IManager {
        
    #region Properties
        [SerializeField] private int coinValue = 1;
        [SerializeField] private TextMeshProUGUI coinTextMesh;
        [SerializeField] private int sapphireValue = 3;
        [SerializeField] private TextMeshProUGUI sapphireTextMesh;
        [SerializeField] private int emeraldValue = 6;
        [SerializeField] private TextMeshProUGUI emeraldTextMesh;
        [SerializeField] private int rubyValue = 9;
        [SerializeField] private TextMeshProUGUI rubyTextMesh;
        [SerializeField] private GameObject lootHolder;
    #endregion

    #region Attributes
        public static CoinManager Instance { get; private set; }
        public int Coins { get; private set; }
        public int Sapphires { get; private set; }
        public int Emeralds { get; private set; }
        public int Rubies { get; private set; }
        public int Dollars => (Coins * coinValue) + (Sapphires * sapphireValue) + (Emeralds * emeraldValue) + (Rubies * rubyValue);
    #endregion
        
    #region Components
        private readonly List<GameObject> _coinUiObjects = new();
        private readonly List<GameObject> _sapphireUiObjects = new();
        private readonly List<GameObject> _emeraldUiObjects = new();
        private readonly List<GameObject> _rubyUiObjects = new();
    #endregion

    #region Unity
        private void Awake() {
            Instance = this;
            
            for (var i = 0; i < lootHolder.transform.GetChild(0).childCount; i++) {
                _coinUiObjects.Add(lootHolder.transform.GetChild(0).GetChild(i).gameObject);
            }
            for (var i = 0; i < lootHolder.transform.GetChild(1).childCount; i++) {
                _sapphireUiObjects.Add(lootHolder.transform.GetChild(1).GetChild(i).gameObject);
            }
            for (var i = 0; i < lootHolder.transform.GetChild(2).childCount; i++) {
                _emeraldUiObjects.Add(lootHolder.transform.GetChild(2).GetChild(i).gameObject);
            }
            for (var i = 0; i < lootHolder.transform.GetChild(3).childCount; i++) {
                _rubyUiObjects.Add(lootHolder.transform.GetChild(3).GetChild(i).gameObject);
            }
            
            _coinUiObjects.Sort(CompareObjects);
            _sapphireUiObjects.Sort(CompareObjects);
            _emeraldUiObjects.Sort(CompareObjects);
            _rubyUiObjects.Sort(CompareObjects);
        }
    #endregion

    #region Custom
        private int CompareObjects(GameObject a, GameObject b) {
            if (a.transform.position.y < b.transform.position.y) { return -1; } 
            return a.transform.position.y > b.transform.position.y ? 1 : 0;
        }
        
        public void Init() {
            enabled = false;

            foreach (var a in _coinUiObjects) {
                a.SetActive(false);
            }
            foreach (var a in _sapphireUiObjects) {
                a.SetActive(false);
            }
            foreach (var a in _emeraldUiObjects) {
                a.SetActive(false);
            }
            foreach (var a in _rubyUiObjects) {
                a.SetActive(false);
            }

            Coins = 0;
            coinTextMesh.text = "000";
            Sapphires = 0;
            sapphireTextMesh.text = "000";
            Emeralds = 0;
            emeraldTextMesh.text = "000";
            Rubies = 0;
            rubyTextMesh.text = "000";
            
            AddLoot(ItemType.Coin, SaveManager.Instance.NumberOfItem(ItemType.Coin));
            AddLoot(ItemType.Emerald, SaveManager.Instance.NumberOfItem(ItemType.Emerald));
            AddLoot(ItemType.Ruby, SaveManager.Instance.NumberOfItem(ItemType.Ruby));
            AddLoot(ItemType.Sapphire, SaveManager.Instance.NumberOfItem(ItemType.Sapphire));
        }
        
        public void SceneChange(string sceneName) {}

        public void AddLoot(ItemId id) {
            AddLoot(id.type, 1);
        }
    
        private void AddLoot(ItemType type, int amount) {
            if (amount == 0) { return; }
            
            var counter = 0;
            
            switch (type) {
                case ItemType.Coin:
                    Coins+=amount;
                    coinTextMesh.text = Coins.ToString("D3");
                    
                    while (_coinUiObjects[counter].activeInHierarchy) {
                        counter++;
                    }
                    for (var i = counter; i < counter + amount; i++) {
                        _coinUiObjects[i].SetActive(true);
                    }
                    
                    break;
                case ItemType.Emerald:
                    Emeralds+=amount;
                    emeraldTextMesh.text = Emeralds.ToString("D3");
                    
                    while (_emeraldUiObjects[counter].activeInHierarchy) {
                        counter++;
                    }
                    for (var i = counter; i < counter + amount; i++) {
                        _emeraldUiObjects[i].SetActive(true);
                    }
                    
                    _emeraldUiObjects[counter].SetActive(true);
                    break;
                case ItemType.Ruby:
                    Rubies+=amount;
                    rubyTextMesh.text = Rubies.ToString("D3");
                    
                    while (_rubyUiObjects[counter].activeInHierarchy) {
                        counter++;
                    }
                    for (var i = counter; i < counter + amount; i++) {
                        _rubyUiObjects[i].SetActive(true);
                    }
                    
                    _rubyUiObjects[counter].SetActive(true);
                    break;
                case ItemType.Sapphire:
                    Sapphires+=amount;
                    sapphireTextMesh.text = Sapphires.ToString("D3");

                    while (_sapphireUiObjects[counter].activeInHierarchy) {
                        counter++;
                    }
                    for (var i = counter; i < counter + amount; i++) {
                        _sapphireUiObjects[i].SetActive(true);
                    }
                    
                    _sapphireUiObjects[counter].SetActive(true);
                    break;
            }
        }
    #endregion

    }
}