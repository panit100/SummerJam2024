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
        Sprite sprite = Resources.Load<Sprite>($"Sprites/Item/{spriteName}");
        if (sprite == null)
            sprite = Resources.Load<Sprite>("Sprites/300x200");
        return sprite;
    }
}
