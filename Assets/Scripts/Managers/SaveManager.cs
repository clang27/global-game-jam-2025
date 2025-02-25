using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace Managers {
	public class SaveManager : MonoBehaviour {
		
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

		public int NumberOfItem(ItemType itemType) {
			return _saveData.ItemsCollected.Count(item => item.type == itemType);
		}
		public void ItemCollected(ItemId itemId) {
			_saveData.ItemsCollected.Add(itemId);
		}

		public bool HasBeenCollected(ItemId itemId)  {
			return _saveData.ItemsCollected.Contains(itemId);
		}

		private static SaveData ConvertData(string s) {
			return JsonUtility.FromJson<SaveData>(s);
		}

		private static SaveData NewData() {
			return new SaveData(new List<ItemId>());
		}

		public void Load() {
			var saveDataString = PlayerPrefs.GetString("SaveData", "None");
			_saveData = saveDataString.Equals("None") ? NewData() : ConvertData(saveDataString);
			
			Debug.Log("Loading:\n" + saveDataString);
		}

		public void Save() {
			var saveDataString = JsonUtility.ToJson(_saveData);
			PlayerPrefs.SetString("SaveData", saveDataString);
			PlayerPrefs.Save();
			Debug.Log("Saving:\n" + saveDataString);
		}
	#endregion
	}
}

