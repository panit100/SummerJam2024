using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoragePanel : Singleton<StoragePanel>, IPointerClickHandler
{
    public RectTransform storageSpace;
    public Transform itemContainer;

    List<Item> items = new List<Item>();

    public List<int> startItemIds = new List<int>();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.gameState != GAMESTATE.INVENTORY)
            return;

        if (PlayerManager.Instance.holdingItem == null)
            return;

        var item = PlayerManager.Instance.holdingItem;
        PlayerManager.Instance.OnPutDownItem();
        StoreItem(item);
        MergeItem(item);
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
        item.rect.SetParent(itemContainer);
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

    void AddItemToStorage(Item item)
    {
        item.SetPosition(GetRandomPositionInStorage());
        StoreItem(item);
    }

    void AddItemToStorage(int itemID)
    {
        var item = ItemManager.Instance.createItem(itemID, itemContainer);
        item.SetPosition(GetRandomPositionInStorage());
        StoreItem(item);
    }
}
