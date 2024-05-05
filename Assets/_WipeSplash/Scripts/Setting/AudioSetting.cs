using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class AudioSetting : MonoBehaviour
{
    [Header("Sound Data")]
    [SerializeField] private SoundDatas SoundDatas;

    [Header("UI")]
    [SerializeField] private Button audioButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform settingGroup;
    [SerializeField] private List<Slider> sliderList;

    private UnityAction<float> SFXAction, BGMAction;
    private Sequence settingSQ;

    void Start()
    {
        SetupUI();
    }
    void SetupUI()
    {
        SFXAction += (value) => ChangeSFXLevel();
        BGMAction += (value) => ChangeBGMLevel();

        sliderList[0].onValueChanged.AddListener(SFXAction);
        sliderList[1].onValueChanged.AddListener(BGMAction);

        audioButton.onClick.AddListener(OpenAudioSetting);
        closeButton.onClick.AddListener(CloseAudioSetting);

        closeButton.gameObject.SetActive(false);
        settingGroup.gameObject.SetActive(false);
    }

    void OpenAudioSetting()
    {
        settingGroup.localScale = new Vector3();

        settingSQ = DOTween.Sequence();

        settingSQ.AppendCallback(() => settingGroup.gameObject.SetActive(true));
        settingSQ.AppendCallback(() => closeButton.gameObject.SetActive(true));
        settingSQ.AppendCallback(() => closeButton.interactable = false);
        settingSQ.Append(settingGroup.DOScale(new Vector3(1, 1, 1), .25f)).SetEase(Ease.OutFlash);
        settingSQ.AppendCallback(() => closeButton.interactable = true);

        settingSQ.Play();
    }
    void CloseAudioSetting()
    {
        settingSQ = DOTween.Sequence();

        settingSQ.AppendCallback(() => closeButton.interactable = false);
        settingSQ.Append(settingGroup.DOScale(new Vector3(), .25f)).SetEase(Ease.InFlash);
        settingSQ.AppendCallback(() => closeButton.gameObject.SetActive(false));
        settingSQ.AppendCallback(() => settingGroup.gameObject.SetActive(false));

        settingSQ.Play();
    }
    void ChangeSFXLevel()
    {
        SoundDatas.audioMixers[0].audioMixer.SetFloat("SFX", sliderList[0].value);
    }
     void ChangeBGMLevel()
    {
        SoundDatas.audioMixers[0].audioMixer.SetFloat("BGM", sliderList[1].value);
    }
}
