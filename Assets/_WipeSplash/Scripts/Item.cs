using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Item : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int gridX;
    public int gridY;
    public bool isRotate = false;

    public Image horizontalImage;
    public Image horizontalCooldown;
    public Image verticalImage;
    public Image verticalCooldown;


    ItemData itemData;
    public ItemData ItemData => itemData;

    public RectTransform rect;

    public UnityAction<Item> onUseItem;

    public float cooldownTime = 0;

    bool isOpenDetail = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOpenDetail = true;
        if (PlayerManager.Instance.holdingItem == null)
            ItemDetailPanel.Instance.Open(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOpenDetail = false;
        ItemDetailPanel.Instance.Close();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.gameState != GAMESTATE.INVENTORY)
            return;

        if (PlayerManager.Instance.holdingItem != null)
            return;

        PlayerManager.Instance.OnPickUpItem(this);

        if (gridX == -1)
        {
            StoragePanel.Instance.PickUpItem(this);
            ItemDetailPanel.Instance.Close();
        }
        else
        {
            PlayerManager.Instance.inventory.OnPickupItem(gridX, gridY);
            ItemDetailPanel.Instance.Close();
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
        horizontalCooldown.sprite = itemSprite;
        horizontalCooldown.SetNativeSize();
        verticalImage.sprite = itemSprite;
        verticalImage.SetNativeSize();
        verticalCooldown.sprite = itemSprite;
        verticalCooldown.SetNativeSize();
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
            horizontalCooldown.gameObject.SetActive(false);
            verticalCooldown.gameObject.SetActive(true);
        }
        else
        {
            isRotate = false;
            horizontalImage.gameObject.SetActive(true);
            verticalImage.gameObject.SetActive(false);
            horizontalCooldown.gameObject.SetActive(true);
            verticalCooldown.gameObject.SetActive(false);
        }
    }

    private void Update()
    {

        if (GameManager.Instance.gameState != GAMESTATE.BATTLE)
            return;

        if (itemData.itemType == ItemType.DEFENSE)
            return;

        cooldownTime += Time.deltaTime;
        if (cooldownTime >= itemData.cooldown)
        {
            var rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    SoundManager.Instance.PlaySFX("magic_01");
                    break;
                case 1:
                    SoundManager.Instance.PlaySFX("magic_02");
                    break;
                default:
                    break;
            }
            onUseItem?.Invoke(this);
        }
        UpdateCooldown();
    }

    public void ResetItem()
    {
        cooldownTime = 0;
        UpdateCooldown();
    }

    public void UpdateCooldown()
    {
        horizontalCooldown.fillAmount = cooldownTime / itemData.cooldown;
        verticalCooldown.fillAmount = cooldownTime / itemData.cooldown;
    }

    public void PlayItemPopUpAnimation()
    {
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), .25f);
    }

    public void PlayItemPopDownAnimation()
    {
        transform.DOScale(new Vector3(1, 1, 1), .25f);
    }
}
