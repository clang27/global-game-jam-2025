using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace Managers {
	public class SaveManager : MonoBehaviour, IManager {
		
	#region Attributes
		public static SaveManager Instance { get; private set; }
	#endregion

	#region Data
		private SaveData _saveData;
	#endregion

	#region Unity
		private void Awake() {
			Instance = this;

			Load();
		}
	#endregion

	#region Custom
		public void Init() { }
		public void SceneChange(string sceneName) {
			foreach (var item in FindObjectsByType<ItemPickup>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
				item.Init();
			}
			foreach (var item in FindObjectsByType<BarrelBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
				item.Init();
			}
			foreach (var item in FindObjectsByType<ChestBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
				item.Init();
			}
		}

		public int NumberOfItem(ItemType itemType) {
			return _saveData.ItemsCollected.Count(item => item.type == itemType);
		}

		public void ItemCollected(ItemId itemId) {
			_saveData.ItemsCollected.Add(itemId);
		}

		public List<ushort> KeysCollected() {
			return _saveData.ItemsCollected
				.Where(item => item.type.Equals(ItemType.Key))
				.Select(item => item.number)
				.ToList();
		}

		public string GetStartScene() {
			return _saveData.SceneName is null or "" ? "Ship" : _saveData.SceneName;
		}
		
		public Vector2 GetStartPosition() {
			return _saveData.StartPosition == Vector2.zero ? new Vector2(-179f, -263f) : _saveData.StartPosition;
		}

		public string GetStartBubbleName() {
			return _saveData.StartBubbleName;
		}

		public bool HasWeapon() {
			return _saveData.ItemsCollected
				.Any(item => item.type.Equals(ItemType.Weapon));
		}

		public bool HasJetpack() {
			return _saveData.ItemsCollected
				.Any(item => item.type.Equals(ItemType.Jetpack));
		}
		
		public bool HasASaveFile() {
			return _saveData.StartBubbleName is not (null or "");
		}

		public bool HasBeenCollected(ItemId itemId)  {
			return _saveData.ItemsCollected != null && _saveData.ItemsCollected.Contains(itemId);
		}

		private static SaveData ConvertData(string s) {
			return JsonUtility.FromJson<SaveData>(s);
		}

		private static SaveData NewData() {
			return new SaveData(new List<ItemId>(), "", "", Vector2.zero);
		}

		public void Load() {
			var saveDataString = PlayerPrefs.GetString("SaveData", "None");
			_saveData = saveDataString.Equals("None") ? NewData() : ConvertData(saveDataString);
			
			Debug.Log("Loading:\n" + saveDataString);
		}

		public void Save(SmallBubbleBehavior bubble) {
			_saveData.StartPosition = bubble.transform.position;
			_saveData.StartBubbleName = bubble.gameObject.name;
			_saveData.SceneName = GameManager.Instance.CurrentSceneName;
			
			var saveDataString = JsonUtility.ToJson(_saveData);
			PlayerPrefs.SetString("SaveData", saveDataString);
			PlayerPrefs.Save();
			Debug.Log("Saving:\n" + saveDataString);
		}

		public void ClearSave() {
			PlayerPrefs.DeleteKey("SaveData");
			PlayerPrefs.Save();
		}
	#endregion
	}
}

