using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerClickHandler
{
    public int gridX;
    public int gridY;
    public bool isRotate = false;

    public Image horizontalImage;
    public Image verticalImage;


    ItemData itemData;
    public ItemData ItemData => itemData;

    public RectTransform rect;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerManager.Instance.holdingItem != null)
            return;

        PlayerManager.Instance.OnPickUpItem(this);

        if (gridX == -1)
        {
            StoragePanel.Instance.PickUpItem(this);
        }
    }

    public void EnableOnClickItem(bool enable)
    {
        horizontalImage.raycastTarget = enable;
        verticalImage.raycastTarget = enable;
    }

    public void Init(ItemData data)
    {
        itemData = data;

        var itemSprite = SpriteManager.Instance.GetSprite(itemData.spriteName);
        horizontalImage.sprite = itemSprite;
        horizontalImage.SetNativeSize();
        verticalImage.sprite = itemSprite;
        verticalImage.SetNativeSize();
    }

    public void SetPosition(Vector2 position)
    {
        gridX = -1;
        gridY = -1;
        rect.localPosition = position;
    }

    public void SetPosition(Vector2 grid, Vector2 gridPos)
    {
        gridX = (int)grid.x;
        gridY = (int)grid.y;
        rect.localPosition = gridPos;
    }

    public void OnRotate()
    {
        if (!isRotate)
        {
            isRotate = true;
            horizontalImage.gameObject.SetActive(false);
            verticalImage.gameObject.SetActive(true);
        }
        else
        {
            isRotate = false;
            horizontalImage.gameObject.SetActive(true);
            verticalImage.gameObject.SetActive(false);
        }
    }
}
