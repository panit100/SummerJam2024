using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HowToPlayPage : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject howToPlayCanvas;
    [SerializeField] private Animator panelAnimator;
    [SerializeField] private CanvasGroup panelGroup;

    [Header("Sound Effect")]
    [SerializeField] private string SfxId;

    [Header("Button")]
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;

    [Header("Tutorial Page")]
    [SerializeField] private TMP_Text pageText;
    [SerializeField] private int pageIndex;
    [SerializeField] private List<GameObject> pageList;
    private GameObject currentPage;

    void Start()
    {
        SetupButton();
    }
    void SetupButton()
    {
        howToPlayCanvas.SetActive(false);

        howToPlayButton.onClick.AddListener(OpenHowToPlayPage);
        backButton.onClick.AddListener(CloaseHowToPlayPage);
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);
    }

    void OpenHowToPlayPage()
    {
        howToPlayCanvas.SetActive(true);
        SoundManager.Instance.PlaySFX(SfxId);

        panelGroup.alpha = 1;
        panelGroup.interactable = true;
        
        pageIndex = 0;
        currentPage = pageList[0];
        currentPage.SetActive(true);
        pageText.text = "1 / " + pageList.Count;
    }
    void CloaseHowToPlayPage()
    {
        panelAnimator.Play("Exit");
        panelGroup.interactable = false;
        SoundManager.Instance.PlaySFX(SfxId);
        
        if(SwitchSQ.IsActive())
            SwitchSQ.Kill();

        SwitchSQ = DOTween.Sequence();

        SwitchSQ.AppendInterval(.5f);
        SwitchSQ.Append(panelGroup.DOFade(0, .15f));
        SwitchSQ.AppendCallback(() => currentPage.SetActive(false));
        SwitchSQ.AppendCallback(() => howToPlayCanvas.SetActive(false));

        SwitchSQ.Play();
    }

    void NextPage()
    {
        pageIndex++;
        CheckaPageIndex();
    }
    void PreviousPage()
    {
        pageIndex--;
        CheckaPageIndex();
    }
    void CheckaPageIndex()
    {
        if(pageIndex < 0)
            pageIndex = pageList.Count - 1;
        else if(pageIndex >= pageList.Count)
            pageIndex = 0;

        panelAnimator.Play("Empty");
        panelAnimator.Play("Switch");
        SoundManager.Instance.PlaySFX(SfxId);
        StartSwitchSequence();
    }
    private Sequence SwitchSQ;
    void StartSwitchSequence()
    {
        if(SwitchSQ.IsActive())
            SwitchSQ.Kill();

        backButton.interactable = false;

        SwitchSQ = DOTween.Sequence();

        SwitchSQ.AppendInterval(.25f);
        SwitchSQ.AppendCallback(() => currentPage.SetActive(false));
        SwitchSQ.AppendCallback(() => pageText.text = $"{pageIndex + 1} / {pageList.Count}");
        SwitchSQ.AppendCallback(() => currentPage = pageList[pageIndex]);
        SwitchSQ.AppendCallback(() => currentPage.SetActive(true));

        SwitchSQ.AppendInterval(.35f);
        SwitchSQ.AppendCallback(() => backButton.interactable = true);

        SwitchSQ.Play();
    }
}
