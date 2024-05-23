using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffect : MonoBehaviour
{

    [Header("Image")]
    [SerializeField] private ParticleSystem healParticle;
    [SerializeField] private ParticleSystem staminaParticle;
    [SerializeField] private Image blockImage;

    [Header("Effect Position")]
    [SerializeField] private RectTransform startTransform;

    void Start()
    {
        blockImage.rectTransform.localScale = new Vector3();
    }

    public void PlayHealEffect()
    {
        SoundManager.Instance.PlaySFX("SFX_Effect_Heal");
        healParticle.Play();
    }
    public void PlayStaminaEffect()
    {
        SoundManager.Instance.PlaySFX("SFX_Effect_Stamina");
        staminaParticle.Play();
    }
    public void PlayBlockEffect()
    {
        blockImage?.DOKill();
        blockImage.rectTransform.DOScale(1.5f, 0.1f)
        .OnComplete(() => blockImage.rectTransform.DOScale(0, 0.35f));
    }
}
