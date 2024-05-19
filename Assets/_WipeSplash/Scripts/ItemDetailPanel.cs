using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : Singleton<ItemDetailPanel>
{
    public CanvasGroup canvasGroup;

    public TMP_Text itemName;
    public TMP_Text itemType;
    public TMP_Text itemDamage;
    public TMP_Text itemBlock;
    public TMP_Text itemRegenHp;
    public TMP_Text itemRegenStamina;
    public TMP_Text itemStaminaCost;
    public TMP_Text itemCooldown;
    public TMP_Text itemDescription;

    public RectTransform canvas;
    RectTransform rect;

    Item currnetItem;

    protected override void InitAfterAwake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        this.Close();
    }

    public void Open(Item item)
    {
        PlayEntryAnimation();
        currnetItem = item;
        switch (item.ItemData.itemType)
        {
            case ItemType.ATTACK:
                itemDamage.gameObject.SetActive(true);
                itemBlock.gameObject.SetActive(false);
                itemRegenHp.gameObject.SetActive(false);
                itemRegenStamina.gameObject.SetActive(false);
                itemStaminaCost.gameObject.SetActive(true);
                itemCooldown.gameObject.SetActive(true);
                break;
            case ItemType.DEFENSE:
                itemDamage.gameObject.SetActive(false);
                itemBlock.gameObject.SetActive(true);
                itemRegenHp.gameObject.SetActive(false);
                itemRegenStamina.gameObject.SetActive(false);
                itemStaminaCost.gameObject.SetActive(false);
                itemCooldown.gameObject.SetActive(false);
                break;
            case ItemType.SUPPORT:
                itemDamage.gameObject.SetActive(false);
                itemBlock.gameObject.SetActive(false);
                itemRegenHp.gameObject.SetActive(true);
                itemRegenStamina.gameObject.SetActive(true);
                itemStaminaCost.gameObject.SetActive(false);
                itemCooldown.gameObject.SetActive(true);
                break;
        }

        setItemText(item.ItemData);

        setPanelPosition(item);

        canvasGroup.alpha = 1;
    }

    void PlayEntryAnimation()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        transform.DOScale(new Vector3(1, 1, 1), .25f);
    }

    void setItemText(ItemData item)
    {
        itemName.text = item.name;
        itemType.text = $"{item.itemType.ToString()} Type";
        itemDamage.text = $"Damage : {item.minDamage} - {item.maxDamage}";
        itemBlock.text = $"Block : {item.block}";
        itemRegenHp.text = $"Regen Hp : {item.regenHp} ";
        itemRegenStamina.text = $"Regen Stamina : {item.regenStamina}";
        itemStaminaCost.text = $"Stamina Cost : {item.staminaCost}";
        itemCooldown.text = $"Cooldown : {item.cooldown}";
        itemDescription.text = $"{item.description}";
    }

    void setPanelPosition(Item item)
    {
        //     screen hight
        //         ^
        //         |
        // 0 <- [1][2] -> screen.width
        //      [0][3]
        //         |
        //         v
        //         0


        var minposition = item.rect.localPosition;
        var maxposition = item.rect.localPosition + new Vector3(100 * (item.ItemData.row - 1), 0, 0);

        var startPos = maxposition;
        var pivot = new Vector2(0, 1);
        var offset = new Vector3(50, 0, 0);

        rect.pivot = pivot;
        rect.localPosition = startPos + offset;

        var tempPos = rect.localPosition;
        Vector2 cornerPoint = new Vector2(tempPos.x + rect.rect.width, tempPos.y - rect.rect.height);
        var screenCornerPos = GetCanvasCornerPosition();

        if (cornerPoint.y < screenCornerPos.y)
        {
            pivot = new Vector2(pivot.x, 0);
        }

        if (cornerPoint.x > screenCornerPos.x)
        {
            pivot = new Vector2(1, pivot.y);
            startPos = minposition;
            offset = new Vector3(-50, 0, 0);
        }

        rect.pivot = pivot;
        rect.localPosition = startPos + offset;

    }

    Vector3 GetCanvasCornerPosition()
    {
        Vector3 corner = new Vector3(canvas.rect.width / 2, -canvas.rect.height / 2);
        return corner;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
    }
}
