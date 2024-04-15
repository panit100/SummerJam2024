using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGrid : MonoBehaviour, IPointerClickHandler
{
    public int x;
    public int y;
    public Item item;
    public RectTransform rect;

    public Action<int, int> onClickGrid;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.gameState != GAMESTATE.INVENTORY)
            return;

        onClickGrid?.Invoke(x, y);
    }

    public void Init(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public void StoreItem(Item _item)
    {
        item = _item;
    }

    public Item GetItem()
    {
        return item;
    }
}
