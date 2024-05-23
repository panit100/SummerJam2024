using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffect : MonoBehaviour
{

    [Header("Image")]
    [SerializeField] private Image healImage;
    [SerializeField] private Image staminaImage;
    [SerializeField] private Image blockImage;

    [Header("Effect Position")]
    [SerializeField] private RectTransform startPosition;

    void Start()
    {
        healImage.color = new Color();
        staminaImage.color = new Color();
        blockImage.rectTransform.localScale = new Vector3();
    }

    public void PlayHealEffect()
    {
        SoundManager.Instance.PlaySFX("SFX_Effect_Heal");

        healImage.color = new Color(1, 1, 1, 1);
        healImage?.DOKill();

        healImage.rectTransform.position = startPosition.position + new Vector3(Random.Range(1.5f, -1.5f), 0, 0);
        healImage.rectTransform.DOLocalMoveY(50, .25f);
        healImage.DOFade(0, .25f);
    }
    public void PlayStaminaEffect()
    {
        SoundManager.Instance.PlaySFX("SFX_Effect_Stamina");

        staminaImage.color = new Color(1, 1, 1, 1);
        staminaImage?.DOKill();

        staminaImage.rectTransform.position = startPosition.position + new Vector3(Random.Range(1.5f, -1.5f), 0, 0);
        staminaImage.rectTransform.DOLocalMoveY(50, .25f);
        staminaImage.DOFade(0, .25f);
    }
    public void PlayBlockEffect()
    {
        blockImage?.DOKill();
        blockImage.rectTransform.DOScale(1.5f, 0.2f);
        blockImage.rectTransform.DOScale(0, 0.8f);
    }
}
