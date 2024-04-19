using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GAMESTATE
{
    INVENTORY,
    SETUPBATTLE,
    BATTLE,
    ENDBATTLE,
}

public class GameManager : Singleton<GameManager>
{
    public GAMESTATE gameState = GAMESTATE.INVENTORY;

    public PlayerPanel playerPanel;

    [Header("Enemy")]
    public EnemyPanel enemyPanel;
    public Image enemyImage;

    [Header("Background")]
    [SerializeField] private Image backgroundImage;

    [Header("UI")]
    public RectTransform inventoryStatePanel;
    public RectTransform fightStatePanel;
    public TMP_Text winText;
    public TMP_Text loseText;
    public Button endButton;

    public int currentEnemy = 0;

    public List<EnemyConfig> enemies = new List<EnemyConfig>();

    protected override void InitAfterAwake()
    {

    }

    void OnEnable()
    {
        playerPanel.onDie += OnDie;
        enemyPanel.onDie += OnDie;

     //   OnChangeState(GAMESTATE.INVENTORY);
    }




    public void OnChangeState(GAMESTATE state)
    {
        gameState = state;
        switch (gameState)
        {
            case GAMESTATE.INVENTORY:
                loseText.gameObject.SetActive(false);
                winText.gameObject.SetActive(false);
                endButton.gameObject.SetActive(false);
                SoundManager.Instance.ChangeBGM("PreparationPhrase");
                foreach (var n in StoragePanel.Instance.Items)
                {
                    n.ResetItem();
                }

                inventoryStatePanel.localPosition = new Vector2(0, 0);
                inventoryStatePanel.anchoredPosition = new Vector2(0, 0);
                fightStatePanel.localPosition = new Vector2(1920, 0);
                fightStatePanel.anchoredPosition = new Vector2(-1920, 0);
                break;
            case GAMESTATE.SETUPBATTLE:
                SoundManager.Instance.ChangeBGM("Battle_PaShed");
                fightStatePanel.localPosition = new Vector2(0, 0);
                fightStatePanel.anchoredPosition = new Vector2(0, 0);
                inventoryStatePanel.localPosition = new Vector2(1920, 0);
                inventoryStatePanel.anchoredPosition = new Vector2(-1920, 0);
                SetupPlayer();
                SetupEnemy(currentEnemy);
                break;
            case GAMESTATE.BATTLE:
                break;
            case GAMESTATE.ENDBATTLE:
                playerPanel.OnEndBattle();
                enemyPanel.OnEndBattle();
                break;
        }
    }

    public void OnSetUpBattle()
    {
        OnChangeState(GAMESTATE.SETUPBATTLE);
    }

    public void OnStartBattle()
    {
        OnChangeState(GAMESTATE.BATTLE);
    }
    public void OnEndBattle()
    {
        if (gameState != GAMESTATE.ENDBATTLE)
            return;

        if (currentEnemy < enemies.Count)
        {
            StoragePanel.Instance.randomGashapon = true;
            OnChangeState(GAMESTATE.INVENTORY);
        }
        else
            OnEndGame();
    }

    public void OnEndGame()
    {
        print("EndGame");
    }

    void SetupPlayer()
    {
        playerPanel.SetupBattle();
    }

    void SetupEnemy(int enemyIndex)
    {
        enemyPanel.inventory.RemoveAllItem();

        foreach (var n in enemies[enemyIndex].startItemConfigs)
        {
            enemyPanel.inventory.AddItemToInventory(n.itemID, n.gridX, n.gridY, n.isRotate);
        }

        enemyPanel.hp = enemies[enemyIndex].hp;
        enemyPanel.block = enemies[enemyIndex].block;
        enemyPanel.stamina = enemies[enemyIndex].stamina;
        enemyPanel.regenStamina = enemies[enemyIndex].regenStamina;
        enemyPanel.baseCooldownRegenStamina = enemies[enemyIndex].baseCooldownRegenStamina;
        enemyImage.sprite = enemies[enemyIndex].enemyImage;
        backgroundImage.sprite = enemies[enemyIndex].enemyBackground;

        enemyPanel.SetupSprite();
        enemyPanel.SetupBattle();
    }

    void OnDie(PlayerPanel player)
    {
        OnChangeState(GAMESTATE.ENDBATTLE);

        if (player == playerPanel)
        {
            //TODO open lose text
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(true);
        }
        else
        {
            //TODO open win text
            winText.gameObject.SetActive(true);
            loseText.gameObject.SetActive(false);

            Item item1 = enemyPanel.inventory.Items[UnityEngine.Random.Range(0, enemyPanel.inventory.Items.Count)];
            Item item2 = enemyPanel.inventory.Items[UnityEngine.Random.Range(0, enemyPanel.inventory.Items.Count)];
            while (item2 == item1)
            {
                item2 = enemyPanel.inventory.Items[UnityEngine.Random.Range(0, enemyPanel.inventory.Items.Count)];
            }

            StoragePanel.Instance.AddItemToStorage(item1.ItemData.id);
            StoragePanel.Instance.AddItemToStorage(item2.ItemData.id);
            currentEnemy++;
        }

        endButton.gameObject.SetActive(true);

    }
}

[Serializable]
public class EnemyConfig
{
    public int hp;
    public int block;
    public float stamina;
    public float regenStamina;
    public float baseCooldownRegenStamina;

    [Header("Enemy Sprite")]
    public Sprite enemyImage;
    public Sprite enemyHurtSprite;

    [Header("Background")]
    public Sprite enemyBackground;


    public List<StartItemConfig> startItemConfigs;
}
