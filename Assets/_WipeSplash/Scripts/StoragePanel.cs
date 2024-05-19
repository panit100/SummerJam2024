using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StoragePanel : Singleton<StoragePanel>, IPointerClickHandler
{
    public RectTransform storageSpace;
    public Transform itemContainer;

    List<Item> items = new List<Item>();
    public List<Item> Items => items;
    public List<int> startItemIds = new List<int>();

    public UnityAction onMergeItem;

    public bool randomGashapon = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.gameState != GAMESTATE.INVENTORY)
        {
            return;
        }

        if (PlayerManager.Instance.holdingItem == null)
        {
            return;
        }

        var item = PlayerManager.Instance.holdingItem;
        PlayerManager.Instance.OnPutDownItem();
        SetItemPositionInStorageSpace(item.transform.localPosition, item);
        StoreItem(item);
        MergeItem(item);

        if (PlayerManager.Instance.holdingItem == null)
            ItemDetailPanel.Instance.Open(item);
    }

    protected override void InitAfterAwake()
    {

    }

    private void Start()
    {
        foreach (var itemID in startItemIds)
        {
            AddItemToStorage(itemID);
        }
    }

    void StoreItem(Item item)
    {
        // item.rect.SetParent(itemContainer);
        item.gridX = -1;
        item.gridY = -1;
        items.Add(item);
        item.EnableOnClickItem(true);
    }

    public void PickUpItem(Item item)
    {
        items.Remove(item);
    }

    void MergeItem(Item item)
    {
        if (item.ItemData.nextLevel == -1)
            return;

        var _item = items.Find(i => (item.ItemData.id == i.ItemData.id) && (i != item));
        if (_item != null)
        {
            //Merge
            onMergeItem?.Invoke();

            var newItem = ItemManager.Instance.createItem(_item.ItemData.nextLevel, itemContainer);

            Vector2 tempPos = item.rect.localPosition;

            items.Remove(item);
            items.Remove(_item);

            Destroy(item.gameObject);
            Destroy(_item.gameObject);

            newItem.rect.localPosition = tempPos;
            StoreItem(newItem);
        }
    }

    Vector2 GetRandomPositionInStorage()
    {
        Vector2 minPosition = new Vector2(
            storageSpace.localPosition.x - storageSpace.sizeDelta.x / 2,
            storageSpace.localPosition.y - storageSpace.sizeDelta.y / 2
        );

        Vector2 maxPosition = new Vector2(
            storageSpace.localPosition.x + storageSpace.sizeDelta.x / 2,
            storageSpace.localPosition.y + storageSpace.sizeDelta.y / 2
        );

        var randomPosX = Random.Range(minPosition.x, maxPosition.x);
        var randomPosY = Random.Range(minPosition.y, maxPosition.y);

        return new Vector2(randomPosX, randomPosY);
    }

    public void AddItemToStorage(int itemID)
    {
        var item = ItemManager.Instance.createItem(itemID, itemContainer);
        var randomPos = GetRandomPositionInStorage();
        randomPos = new Vector2(randomPos.x, randomPos.y);

        item.SetPosition(randomPos);

        SetItemPositionInStorageSpace(randomPos, item);

        StoreItem(item);
    }

    public void RandomItem()
    {
        if (!randomGashapon)
            return;
        var item = Random.Range(0, 18);
        AddItemToStorage(item);
        randomGashapon = false;
    }

    Vector2[] getCornerStorageSpace()
    {
        Vector2 corner1 = new Vector2(storageSpace.localPosition.x - storageSpace.rect.width / 2, storageSpace.localPosition.y - storageSpace.rect.height / 2);
        Vector2 corner2 = new Vector2(storageSpace.localPosition.x + storageSpace.rect.width / 2, storageSpace.localPosition.y + storageSpace.rect.height / 2);

        return new Vector2[] { corner1, corner2 };
    }

    void SetItemPositionInStorageSpace(Vector2 position, Item item)
    {
        var cornersItem = item.GetCornersItem();

        Vector2[] storageSpaceCorners = getCornerStorageSpace();

        position = item.transform.localPosition;

        if (cornersItem[0].y < storageSpaceCorners[0].y)
        {
            var yRange = storageSpaceCorners[0].y - cornersItem[0].y;
            position = new Vector2(position.x, position.y + yRange + 10);
        }

        if (cornersItem[0].x < storageSpaceCorners[0].x)
        {
            var xRange = storageSpaceCorners[0].x - cornersItem[0].x;
            position = new Vector2(position.x + xRange + 10, position.y);
        }

        if (cornersItem[1].x > storageSpaceCorners[1].x)
        {
            var xRange = cornersItem[1].x - storageSpaceCorners[1].x;
            position = new Vector2(position.x - xRange - 10, position.y);
        }

        if (cornersItem[1].y > storageSpaceCorners[1].y)
        {
            var yRange = cornersItem[1].y - storageSpaceCorners[1].y;
            position = new Vector2(position.x, position.y - yRange - 10);
        }

        item.transform.localPosition = position;
    }
}
