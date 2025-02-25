using System;
using System.Collections.Generic;
using Enums;

[Serializable]
public struct SaveData {
    public List<ItemId> ItemsCollected;
    
    public SaveData(List<ItemId> itemsCollected) {
        ItemsCollected = itemsCollected;
    }
}
