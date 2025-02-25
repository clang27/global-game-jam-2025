using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

[Serializable]
public struct SaveData {
    public List<ItemId> ItemsCollected;
    public string SceneName, StartBubbleName;
    public Vector2 StartPosition;
    
    public SaveData(List<ItemId> itemsCollected, string sceneName, string startBubbleName, Vector2 startPosition) {
        ItemsCollected = itemsCollected;
        SceneName = sceneName;
        StartPosition = startPosition;
        StartBubbleName = startBubbleName;
    }
}
