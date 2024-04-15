using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public RectTransform gamePanel;
    public PlayerPanel playerPanel;
    public PlayerPanel enemyPanel;

    public int currentEnemy = 0;

    public List<EnemyConfig> enemies = new List<EnemyConfig>();

    protected override void InitAfterAwake()
    {

    }

    private void Start()
    {
        playerPanel.onDie += OnDie;
        enemyPanel.onDie += OnDie;

        OnChangeState(GAMESTATE.INVENTORY);
    }

    void OnChangeState(GAMESTATE state)
    {
        gameState = state;
        switch (gameState)
        {
            case GAMESTATE.INVENTORY:
                gamePanel.localPosition = new Vector2(0, 0);
                gamePanel.anchoredPosition = new Vector2(0, 0);
                break;
            case GAMESTATE.SETUPBATTLE:
                gamePanel.localPosition = new Vector2(-1920, 0);
                gamePanel.anchoredPosition = new Vector2(-1920, 0);
                SetupPlayer();
                SetupEnemy(currentEnemy);
                break;
            case GAMESTATE.BATTLE:
                break;
            case GAMESTATE.ENDBATTLE:
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

    void SetupPlayer()
    {
        playerPanel.SetupBattle();
    }

    void SetupEnemy(int enemyIndex)
    {
        enemyPanel.inventory.RemoveAllItem();

        foreach (var n in enemies[enemyIndex].startItemConfigs)
        {
            enemyPanel.inventory.AddItemToInventory(n.itemID, n.gridX, n.gridY);
        }

        enemyPanel.hp = enemies[enemyIndex].hp;
        enemyPanel.block = enemies[enemyIndex].block;
        enemyPanel.stamina = enemies[enemyIndex].stamina;
        enemyPanel.regenStamina = enemies[enemyIndex].regenStamina;
        enemyPanel.baseCooldownRegenStamina = enemies[enemyIndex].baseCooldownRegenStamina;

        enemyPanel.SetupBattle();
    }

    void OnDie(PlayerPanel player)
    {
        OnChangeState(GAMESTATE.ENDBATTLE);
        //Random Add 2 Item to player Inventory
        Item item1 = enemyPanel.inventory.Items[UnityEngine.Random.Range(0, enemyPanel.inventory.Items.Count)];
        Item item2 = enemyPanel.inventory.Items[UnityEngine.Random.Range(0, enemyPanel.inventory.Items.Count)];
        while (item2 == item1)
        {
            item2 = enemyPanel.inventory.Items[UnityEngine.Random.Range(0, enemyPanel.inventory.Items.Count)];
        }

        StoragePanel.Instance.AddItemToStorage(item1.ItemData.id);
        StoragePanel.Instance.AddItemToStorage(item2.ItemData.id);
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

    public List<StartItemConfig> startItemConfigs;
}
