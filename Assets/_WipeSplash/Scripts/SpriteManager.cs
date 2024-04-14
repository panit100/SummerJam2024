using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : Singleton<SpriteManager>
{
    protected override void InitAfterAwake()
    {
    }

    public Sprite GetSprite(string spriteName)
    {
        Sprite sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
        return sprite;
    }
}
