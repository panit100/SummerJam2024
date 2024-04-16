using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class PlayerPanel : MonoBehaviour
{
    [Header("Stat")]
    public int hp = 100;
    public int block = 0;
    public float stamina = 10;
    public float regenStamina = 1f;
    public float baseCooldownRegenStamina = 2f;

    [Header("UI")]
    public Image hpBar;
    public TMP_Text hpText;
    public Image blockBar;
    public TMP_Text blockText;
    public Image staminaBar;
    public TMP_Text staminaText;

    public InventoryPanel inventory;
    public PlayerPanel enemy;

    int currentHp = 0;
    int currentBlock = 0;
    int tempMaxBlock = 0;
    float currentStamina = 0;
    float cooldownRegenStamina = 0;

    public UnityAction<PlayerPanel> onDie;

    private void Update()
    {
        if (GameManager.Instance.gameState != GAMESTATE.BATTLE)
            return;

        cooldownRegenStamina += Time.deltaTime;
        if (cooldownRegenStamina >= baseCooldownRegenStamina)
        {
            OnRegenStamina(regenStamina);
            cooldownRegenStamina = 0;
        }
    }

    public void SetupBattle()
    {
        currentHp = hp;
        currentBlock = block;
        currentStamina = stamina;
        cooldownRegenStamina = 0;


        foreach (var item in inventory.Items)
        {
            item.onUseItem += OnUseItem;
            currentBlock += item.ItemData.block;
        }

        tempMaxBlock = currentBlock;

        UpdateStat();
    }

    public void OnEndBattle()
    {
        foreach (var item in inventory.Items)
        {
            item.onUseItem -= OnUseItem;
            item.ResetItem();
        }
    }

    void OnUseItem(Item item)
    {
        if (currentStamina < item.ItemData.staminaCost)
            return;

        currentStamina -= item.ItemData.staminaCost;
        switch (item.ItemData.itemType)
        {
            case ItemType.ATTACK:
                var damage = UnityEngine.Random.Range(item.ItemData.minDamage, item.ItemData.maxDamage);
                enemy.OnTakeDamage(damage);
                break;
            case ItemType.SUPPORT:
                OnRegenStamina(item.ItemData.regenStamina);
                OnRegenHp(item.ItemData.regenHp);
                break;
        }

        UpdateStat();
        item.cooldownTime = 0;

    }

    public void OnTakeDamage(int damage)
    {
        if (currentBlock > 0)
        {
            currentBlock -= damage;
            if (currentBlock < 0)
            {
                currentHp += currentBlock;
                currentBlock = 0;
                UpdateStat();
                return;
            }
            else
                return;
        }

        currentHp -= damage;
        UpdateStat();
        if (currentHp <= 0)
        {
            currentHp = 0;
            UpdateStat();
            OnDie();
            return;
        }
    }

    void OnDie()
    {
        onDie?.Invoke(this);
    }

    void OnRegenStamina(float regenStamina)
    {
        currentStamina += regenStamina;

        if (currentStamina >= stamina)
            currentStamina = stamina;
    }

    void OnRegenHp(int regenHp)
    {
        currentHp += regenHp;
        if (currentHp >= hp)
            currentHp = hp;
    }

    void UpdateStat()
    {
        UpdateStatText();
        UpdateStatBar();
    }

    void UpdateStatText()
    {
        hpText.text = $"{currentHp}/{hp}";
        blockText.text = $"{currentBlock}/{tempMaxBlock}";
        staminaText.text = $"{Math.Round(currentStamina, 2)}/{stamina}";
    }

    void UpdateStatBar()
    {
        hpBar.fillAmount = (float)currentHp / (float)hp;
        if (tempMaxBlock == 0)
            blockBar.fillAmount = 0;
        else
            blockBar.fillAmount = (float)currentBlock / (float)tempMaxBlock;
        staminaBar.fillAmount = currentStamina / stamina;

    }
}
