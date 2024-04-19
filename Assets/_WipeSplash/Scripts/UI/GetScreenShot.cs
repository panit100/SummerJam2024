using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScreenShot : MonoBehaviour
{
    [SerializeField] ScreenTextureCapturer screenCapture;
    [SerializeField] RawImage image;

    void Start()
    {
        image.color = new Color(0, 0, 0, 0);
    }

    public void OnGetScreenShot()
    {
        StartCoroutine(screenCapture.UpdateScreenshotTexture());
        image.texture = screenCapture.ScreenshotTexture;
    }
    
}
