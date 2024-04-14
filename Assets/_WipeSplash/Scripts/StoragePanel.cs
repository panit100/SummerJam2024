using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoragePanel : Singleton<StoragePanel>, IPointerClickHandler
{
    public RectTransform storageSpace;
    public Transform itemContainer;

    public List<Item> items = new List<Item>();

    public void OnPointerClick(PointerEventData eventData)
    {
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
        var item1 = ItemManager.Instance.createItem(0);
        item1.rect.localPosition = GetRandomPositionInStorage();
        StoreItem(item1);
    }

    void StoreItem(Item item)
    {
        item.rect.SetParent(itemContainer);
        items.Add(item);
        item.EnableOnClickItem(true);
    }

    void MergeItem(Item item)
    {
        var _item = items.Find(i => (item.ItemData.id == i.ItemData.id) && (i != item));
        if (_item != null)
        {
            //Merge
            var newItem = ItemManager.Instance.createItem(_item.ItemData.nextLevel);

            Vector2 tempPos = newItem.rect.localPosition;

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


}
