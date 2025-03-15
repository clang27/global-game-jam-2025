using System;

namespace Enums {
    [Serializable]
    public enum ItemType {
        Coin, Emerald, Ruby, Sapphire, OxygenUpgrade, Key, Jetpack, Weapon, Barrel, Chest
    }

    [Serializable]
    public struct ItemId {
        public ItemType type;
        public ushort number;

        public override string ToString() {
            return type + "-" + number;
        }
    }
}