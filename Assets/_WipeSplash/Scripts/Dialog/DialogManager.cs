using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class DialogManager : Singleton<DialogManager>
{

    [Header("Canvas")]
    [SerializeField] private GameObject dialogCanvas;

    [Header("Text")]
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogText;

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

        // run on start
       // StartDialogInteraction();
    }
    private void SetupCanvas()
    {
        speakerText.text = "";
        speakerText.color = new Color(1, 1, 1, 0);

        dialogText.text = "";
        dialogText.color = new Color(1, 1, 1, 0);

        dialogOverlay.color = new Color(1, 1, 1, 0);
        speakerImage.color = new Color(1, 1, 1, 0);
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
    }

#endregion

#region dialog activation

    public void StartDialogInteraction()
    {
        dialogSet = dialogSetList[dialogSetCount];
        dialogCount = 0;
        ChangeDialogState(DialogState.ready);
        dialogCanvas.SetActive(true);
        backgroundImage.sprite = dialogSetList[dialogSetCount].backgroundSprite;

        Sequence StartSequence = DOTween.Sequence();

        
        StartSequence.Append(transitionImage.DOFade(0, 2.5f));
        
        StartSequence.Append(dialogText.DOFade(1f, .5f));
        StartSequence.Join(speakerText.DOFade(1f, .5f));
        StartSequence.Join(dialogOverlay.DOFade(dialogOverlayAlpha, .25f));
        StartSequence.AppendCallback(CreateDialog);

        StartSequence.Play();
    }

#endregion

#region dialog play

    #region dialog create
    private void CreateDialog()
    {
        dialogButton.gameObject.SetActive(true);
        StartCoroutine("CreateDialogSequence");
    }
    private IEnumerator CreateDialogSequence()
    {
        ChangeDialogState(DialogState.progress);

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

    #region check if llast dialog is played
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
        dialogText.DOKill();
        
        Sequence EndSequence = DOTween.Sequence();
        
        EndSequence.Append(dialogText.DOFade(0, .5f));
        EndSequence.Join(speakerText.DOFade(0, .5f));
        EndSequence.Join(speakerImage.DOFade(0, .25f));
        EndSequence.Join(dialogOverlay.DOFade(0, 1.5f).SetEase(Ease.InQuart));
        EndSequence.Append(transitionImage.DOFade(1, 3f));
        EndSequence.AppendCallback(CompleteStopDialogInteraction);

        EndSequence.Play();
    }
    private void CompleteStopDialogInteraction()
    {
        LoadingManager.Instance.OnLoadingComplete +=() => dialogCanvas.SetActive(false);
        LoadingManager.Instance.OnLoadingComplete += () => GameManager.Instance.OnChangeState(GAMESTATE.INVENTORY);
        LoadingManager.Instance.DoLoading();
    }

#endregion

}