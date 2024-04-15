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
    }

    void OnChangeState(GAMESTATE state)
    {
        gameState = state;
        switch (gameState)
        {
            case GAMESTATE.INVENTORY:
                break;
            case GAMESTATE.SETUPBATTLE:
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
