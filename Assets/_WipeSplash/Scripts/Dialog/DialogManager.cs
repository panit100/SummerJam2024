using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DialogManager : Singleton<DialogManager>
{

    [Header("Canvas")]
    [SerializeField] private GameObject dialogCanvas;

    [Header("Text")]
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogText;

    [Header("Dialog Button Animation")]
    [SerializeField] private Animator dialogButtonAnimator;

    [Header("Image")]
    [SerializeField] private Image speakerImage;
    [SerializeField] private Image dialogOverlay;
    [SerializeField] private float dialogOverlayAlpha = 0.5f;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject dialogButtonPin;
    [SerializeField] private Button dialogButton;
    private Vector3 speakerImageStartPosition;

    [Header("Transition Image")]
    [SerializeField] private Image transitionImage;

    [Header("Dialog State")]
    [SerializeField] private DialogState dialogState;
    [SerializeField] private enum DialogState
    {
        disabled, progress, ready
    }
    [SerializeField] private int dialogCount;

    [Header("Song Name Displayer")]
    [SerializeField] private SongNameDisplayer SongNameDisplayer;

    [Header("Skip Button")]
    [SerializeField] private Button skipButton;

    [Header("Hide BG Button")]
    [SerializeField] private Button hideBackgroundButton;
    [SerializeField] private Button showBackgroundButton;
    [SerializeField] private CanvasGroup dialogGroup;

    [Header("Dialog Set")]
    [SerializeField] private int dialogSetCount;
    [SerializeField] private List<Dialog> dialogSetList;

    private Dialog dialogSet;

#region setup functions

protected override void InitAfterAwake()
{
    
}

private void Start()
    {
        SetupCanvas();
        SetupButton();
        ChangeDialogState(DialogState.disabled);

        // REMOVE AFTER TEST: test run on start
       StartDialogInteraction();
    }
    private void SetupCanvas()
    {
        speakerText.text = "";
        dialogText.text = "";

        dialogGroup.alpha = 0;

        speakerImage.color = new Color(1, 1, 1, 0);

        transitionImage.gameObject.SetActive(false);
        transitionImage.color = new Color(0, 0, 0, 1);

        speakerImage.sprite = null;
        speakerImageStartPosition = speakerImage.rectTransform.localPosition;

        // UNCOMMENT AFTER TEST
        // dialogCanvas.SetActive(false);
    }
    private void SetupButton()
    {
        dialogButton.onClick.AddListener(NextDialog);
        dialogButton.gameObject.SetActive(false);

        skipButton.onClick.AddListener(SkipDialog);
        skipButton.gameObject.SetActive(false);
        
        dialogButtonAnimator.gameObject.SetActive(false);
        hideBackgroundButton.gameObject.SetActive(false);
        showBackgroundButton.gameObject.SetActive(false);

        hideBackgroundButton.onClick.AddListener(HideBackground);
        showBackgroundButton.onClick.AddListener(ShowBackground);
    }

#endregion

#region dialog activation

    public void StartDialogInteraction()
    {
        dialogSet = dialogSetList[dialogSetCount];
        dialogCount = 0;
        
        ChangeDialogState(DialogState.ready);
        dialogCanvas.SetActive(true);
        transitionImage.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
        hideBackgroundButton.gameObject.SetActive(true);

        backgroundImage.sprite = dialogSetList[dialogSetCount].backgroundSprite;
        SoundManager.Instance.ChangeBGM(dialogSet.bgmName);

        Sequence StartSequence = DOTween.Sequence();
        
        StartSequence.Append(transitionImage.DOFade(0, 2.5f));
        StartSequence.AppendCallback(() => transitionImage.gameObject.SetActive(false));
        StartSequence.Append(dialogGroup.DOFade(1, 1));
        StartSequence.AppendCallback(() => SongNameDisplayer.DisplaySongName(dialogSet.songName, dialogSet.artistName));
        StartSequence.AppendCallback(CreateDialog);

        StartSequence.Play();
    }

#endregion

#region dialog play

    #region dialog create
    private void CreateDialog()
    {
        dialogButton.gameObject.SetActive(true);
        dialogButtonAnimator.gameObject.SetActive(true);
        StartCoroutine("CreateDialogSequence");
    }
    private IEnumerator CreateDialogSequence()
    {
        ChangeDialogState(DialogState.progress);
        ActiveAnimationProgress();

        // text setup
        speakerText.text = dialogSet.dialogList[dialogCount].speakername;
        dialogText.text = "";
        dialogText.color = dialogSet.dialogList[dialogCount].color;
        dialogText.fontSize = dialogSet.dialogList[dialogCount].fontSize;
        char[] charArray = dialogSet.dialogList[dialogCount].dialog.ToCharArray();

        // portrait setup
        CheckChangeSprite();

        // background setup
        CheckChangeBackground();

        foreach(char character in charArray)
        {
            dialogText.text += character; 
            yield return new WaitForSeconds(0.05f);
        }

        CompleteDialogSequence();
    }
    private void CompleteDialogSequence()
    {
        StopCoroutine("CreateDialogSequence");
        ChangeDialogState(DialogState.ready);
        ActiveAnimationEnd();
    }
    #endregion

    #region image setup

        void CheckChangeSprite()
        {
            Sprite currentSprite = dialogSet.dialogList[dialogCount].speakerSprite;

            if(currentSprite == null)
            {
                speakerImage.DOFade(0, .5f);
            }
            else if(currentSprite != null && currentSprite != speakerImage.sprite)
            {
                speakerImage.DOKill();

                speakerImage.sprite = currentSprite;
                speakerImage.rectTransform.localPosition = speakerImageStartPosition + (Vector3.up * -150);
                speakerImage.rectTransform.DOLocalMove(speakerImageStartPosition, .5f).SetEase(Ease.OutBounce);
                speakerImage.DOFade(1f, .5f);
            }
        }

    #endregion

    #region background setup

        void CheckChangeBackground()
        {
            if(dialogSet.dialogList[dialogCount].isChangeBackground)
            {
                transitionImage.DOFade(1, 1f);
            }
        }

    #endregion

    #region dialog button
    public void ActiveAnimationProgress()
    {
        dialogButtonAnimator.SetBool("bool_isProgress", true);
    }
    public void ActiveAnimationEnd()
    {
        dialogButtonAnimator.SetBool("bool_isProgress", false);
    }
    #endregion

    #region next dialog button
    private void NextDialog()
    {
        if(dialogState == DialogState.ready && dialogCount + 1 == dialogSet.dialogList.Count)
        {
            CheckCompleteDialogInteraction();
        }
        if(dialogState == DialogState.ready)
        {
            dialogCount++;
            CreateDialog();
        }
        else if(dialogState == DialogState.progress)
        {
            ForceDisplayDialog();
        }
    }
    private void ForceDisplayDialog()
    {
        dialogText.text = dialogSet.dialogList[dialogCount].dialog;
        CompleteDialogSequence();
    }
    #endregion

    #region check if last dialog is played
    private void CheckCompleteDialogInteraction()
    {
        StopDialogInteraction();
        ChangeDialogState(DialogState.disabled);
        dialogButton.gameObject.SetActive(false);
        dialogSetCount++;
    }
    #endregion

#endregion

#region state change function

    private void ChangeDialogState(DialogState state)
    {
        dialogState = state;
    }

#endregion

#region dialog deactivation

    public void StopDialogInteraction()
    {
        StopCoroutine("CreateDialogSequence");
        dialogText.text = "";
        speakerText.text = "";

        dialogText.DOKill();
        dialogButtonAnimator.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        
        Sequence EndSequence = DOTween.Sequence();
        ChangeDialogState(DialogState.disabled);
        EndSequence.Append(dialogGroup.DOFade(0, 1));
        EndSequence.Join(speakerImage.DOFade(0, .5f));
        EndSequence.AppendCallback(() => speakerImage.sprite = null);
        EndSequence.AppendCallback(() => transitionImage.gameObject.SetActive(true));
        EndSequence.Append(transitionImage.DOFade(1, 3f));
        EndSequence.AppendCallback(CompleteStopDialogInteraction);

        EndSequence.Play();
    }
    private void CompleteStopDialogInteraction()
    {    
        if(dialogSet.nextState == Dialog.NextState.ENDGAME)
        {
            // SHOW END GAME PANEL
        }
        else if(dialogSet.nextState == Dialog.NextState.DIALOG)
        {
            StartDialogInteraction();
        }
        else if(dialogSet.nextState == Dialog.NextState.BATTLE)
        {
            LoadingManager.Instance.OnLoadingComplete +=() => dialogCanvas.SetActive(false);
            LoadingManager.Instance.OnLoadingComplete += () => GameManager.Instance.OnChangeState(GAMESTATE.INVENTORY);
            LoadingManager.Instance.DoLoading();
        }
    }

#endregion

#region skip button

    void SkipDialog()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CheckCompleteDialogInteraction();
        SongNameDisplayer.ForcecStopDisplay();
    }

#endregion

#region hide BG button

    private Sequence BGSequence;
    void HideBackground()
    {
        BGSequence = DOTween.Sequence();

        BGSequence.AppendCallback(() => dialogGroup.interactable = false);
        BGSequence.Append(dialogGroup.DOFade(0, 1f));
        BGSequence.AppendCallback(() => hideBackgroundButton.interactable = true);
        BGSequence.AppendCallback(() => showBackgroundButton.gameObject.SetActive(true));

        BGSequence.Play();
    }
    void ShowBackground()
    {
        BGSequence = DOTween.Sequence();

        BGSequence.AppendCallback(() => showBackgroundButton.gameObject.SetActive(false));
        BGSequence.Append(dialogGroup.DOFade(1, 1f));
        BGSequence.AppendCallback(() => dialogGroup.interactable = true);
        BGSequence.AppendCallback(() => hideBackgroundButton.interactable = true);

        BGSequence.Play();
    }
    

#endregion
}