using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using DG.Tweening;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class PlayerPanel : MonoBehaviour
{
    [Header("Stat")]
    public float hp = 100;
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

    [Header("Character Image")]
    [SerializeField] private Image playerImage;
    [SerializeField] protected Sprite defaultSprite;
    [SerializeField] protected Sprite hurtSprite;
    [SerializeField] private float bounceValue;
    [SerializeField] private Image splashImage;
    [SerializeField] private Image impactImage;
    [SerializeField] ParticleSystem playerEffect;
    private Vector3 startPosition;

    [Header("System")]
    public InventoryPanel inventory;
    public PlayerPanel enemy;

    float currentHp = 0;
    int currentBlock = 0;
    int tempMaxBlock = 0;
    float currentStamina = 0;
    float cooldownRegenStamina = 0;

    public UnityAction onTakeDamage;
    public UnityAction<PlayerPanel> onDie;

    void Start()
    {
        BouncePositionSetup();
        splashImage.color = new Color(1, 1, 1, 0);

        SetupAdditional();
    }
    protected virtual void SetupAdditional()
    {

    }

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
        {
            UpdateStat();
            item.cooldownTime = 0;
            return;
        }

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

        SoundManager.Instance.PlaySFX("magic_0" + Random.Range(0, 2));

        UpdateStat();
        item.cooldownTime = 0;

    }

    public void OnTakeDamage(int damage)
    {
        onTakeDamage?.Invoke();
        var rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                SoundManager.Instance.PlaySFX("card");
                break;
            case 1:
                SoundManager.Instance.PlaySFX("crossbow");
                break;
            default:
                break;
        }
        if (currentBlock > 0)
        {
            // TODO: add block effect
            SoundManager.Instance.PlaySFX("SFX_Effect_Block");

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

        DamageAnimationSequence();
        UpdateStat();
        if (currentHp <= 0)
        {
            currentHp = 0;
            UpdateStat();
            OnDie();
            return;
        }
    }

    void BouncePositionSetup()
    {
        startPosition = playerImage.rectTransform.localPosition;
    }
    void DamageAnimationSequence()
    {
        playerImage.rectTransform.DOKill();
        splashImage.DOKill();
        impactImage.DOKill();
        playerEffect.Clear();

        playerImage.sprite = hurtSprite;
        splashImage.color = new Color(1, 1, 1, 1);
        impactImage.color = new Color(1, 1, 1, 1);
        splashImage.rectTransform.rotation = quaternion.Euler(new float3(0, 0, Random.Range(0, 360)));
        playerEffect.Play();
        playerImage.rectTransform.localPosition = startPosition + (Vector3.left * bounceValue);
        playerImage.rectTransform.DOLocalMove(startPosition, .5f).OnComplete(() => CompleteBounce());
        splashImage.DOFade(0, .5f).SetEase(Ease.InBounce);
        impactImage.DOFade(0, .15f).SetEase(Ease.Flash);
    }
    void CompleteBounce()
    {
        playerImage.sprite = defaultSprite;
    }

    void OnDie()
    {
        onDie?.Invoke(this);
    }

    void OnRegenStamina(float regenStamina)
    {
        // TODO: increase stamina effect
        SoundManager.Instance.PlaySFX("SFX_Effect_Stamina");

        currentStamina += regenStamina;

        if (currentStamina >= stamina)
            currentStamina = stamina;

        UpdateStat();
    }

    void OnRegenHp(float regenHp)
    {
        // TODO: regen hp effect
        SoundManager.Instance.PlaySFX("SFX_Effect_Heal");

        currentHp += regenHp;
        if (currentHp >= hp)
            currentHp = hp;

        UpdateStat();
    }

    void UpdateStat()
    {
        UpdateStatText();
        UpdateStatBar();
    }

    void UpdateStatText()
    {
        hpText.text = $"{Math.Round(currentHp, 2)}/{hp}";
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
