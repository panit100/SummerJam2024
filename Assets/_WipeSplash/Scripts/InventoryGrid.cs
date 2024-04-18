using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum GridStatus
{
    NORMAL,
    EMPTY,
    FULL,
}
public class InventoryGrid : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int x;
    public int y;
    public Item item;
    public RectTransform rect;

    public Image gridSprite;
    public Sprite normalGrid;
    public Sprite emptyGrid;
    public Sprite fullGrid;

    public Action<int, int> onEnterGrid;
    public Action<int, int> onExitGrid;
    public Action<int, int> onClickGrid;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.gameState != GAMESTATE.INVENTORY)
        {
            return;
        }
        onEnterGrid?.Invoke(x, y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.gameState != GAMESTATE.INVENTORY)
        {
            return;
        }
        onExitGrid?.Invoke(x, y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.gameState != GAMESTATE.INVENTORY)
        {
            return;
        }

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

    public void ChangeGridSprite(GridStatus gridStatus)
    {
        switch (gridStatus)
        {
            case GridStatus.NORMAL:
                gridSprite.sprite = normalGrid;
                break;
            case GridStatus.EMPTY:
                gridSprite.sprite = emptyGrid;
                break;
            case GridStatus.FULL:
                gridSprite.sprite = fullGrid;
                break;
        }
    }
}
