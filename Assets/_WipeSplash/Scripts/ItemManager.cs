using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Item itemPrefab;
    public Transform itemContainer;

    protected override void InitAfterAwake()
    {
    }

    public Item createItem(int itemID)
    {
        Item newItem = Instantiate(itemPrefab, itemContainer);
        ItemData data = AllItemData.itemDatas.Find(data => data.id == itemID);
        newItem.Init(data);
        return newItem;
    }
}
