using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Item holdingItem;
    public RectTransform canvas;
    public InventoryPanel inventory;

    protected override void InitAfterAwake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        var mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll != 0)
        {
            OnRotateItem();
        }
    }

    void FixedUpdate()
    {
        UpdateItemPosition();
    }

    public void OnPickUpItem(Item item)
    {
        holdingItem = item;
        item.EnableOnClickItem(false);
    }

    public void OnPutDownItem()
    {
        holdingItem = null;
    }

    void OnRotateItem()
    {
        if (holdingItem == null)
            return;

        int tempX = inventory.CurrentGridX;
        int tempY = inventory.CurrentGridY;
        inventory.OnExitGrid(tempX, tempY);

        holdingItem.OnRotate();

        inventory.OnEnterGrid(tempX, tempY);
    }

    void UpdateItemPosition()
    {
        if (holdingItem == null)
            return;

        Vector2 mousePos = Input.mousePosition;
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas, mousePos, Camera.main, out canvasPosition);

        holdingItem.rect.localPosition = canvasPosition;
    }
}
