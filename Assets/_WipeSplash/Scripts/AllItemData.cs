using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public static class AllItemData
{
    public static List<ItemData> itemDatas = new List<ItemData>{
        new ItemData{
            id = 0,
            nextLevel = 1,
            row = 1,
            column = 1,
            name = "Test-01",
            spriteName = "100x100",
            minDamage = 1,
            maxDamage = 4,
            block = 0,
            regenHp = 0,
            regenStamina = 0,
            staminaCost = 1,
            cooldown = 1,
            itemType = ItemType.ATTACK
        },

        new ItemData{
            id = 1,
            nextLevel = -1,
            row = 2,
            column = 1,
            name = "Test-01",
            spriteName = "200x100",
            minDamage = 1,
            maxDamage = 4,
            block = 0,
            regenHp = 0,
            regenStamina = 0,
            staminaCost = 1,
            cooldown = 1,
            itemType = ItemType.ATTACK
        },

    };
}
