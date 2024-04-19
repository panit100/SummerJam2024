using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Plugins.Animate_UI_Materials;
using UnityEngine;
using UnityEngine.Events;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] GraphicPropertyOverrideRange loadingBar;
     float targetValue = 1f; 
     float duration = 2f; 
     bool loading = false;

     public UnityAction OnLoadingComplete;

   

     public void DoLoading()
    {
        if(loading) return;
        loading = true;
        DOTween.Sequence().Append(DOTween
            .To(() => loadingBar.PropertyValue, x => loadingBar.PropertyValue = x, targetValue, duration).OnComplete(
                (() => { 
                    OnLoadingComplete?.Invoke();
                    OnLoadingComplete = null;
                }))
        ).AppendInterval(1.25f).Append(DOTween
            .To(() => loadingBar.PropertyValue, x => loadingBar.PropertyValue = x, 0f, 1f)
            .OnComplete(() =>
            {
                loading = false;
                Debug.Log("Tween completed!");
            }));

    }
}
