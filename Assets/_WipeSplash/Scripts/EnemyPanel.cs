using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPanel : PlayerPanel
{
    [Header("System")]
    [SerializeField] private GameManager GameManager;

    protected override void SetupAdditional()
    {
        SetupSprite();
    }
    public void SetupSprite()
    {
        defaultSprite = GameManager.enemies[GameManager.currentEnemy].enemyImage;
        hurtSprite = GameManager.enemies[GameManager.currentEnemy].enemyHurtSprite;
    }
}
