using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Item itemPrefab;

    protected override void InitAfterAwake()
    {
    }

    public Item createItem(int itemID, Transform parent)
    {
        Item newItem = Instantiate(itemPrefab, parent);
        ItemData data = AllItemData.itemDatas.Find(data => data.id == itemID);
        newItem.Init(data);
        return newItem;
    }
}
