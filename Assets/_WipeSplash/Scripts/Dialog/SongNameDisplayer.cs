using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SongNameDisplayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform displayerTransform;
    [SerializeField] private RectTransform stopPosition;
    
    [Header("Song Name UI")]
    [SerializeField] private TMP_Text songNameText;
    [SerializeField] private TMP_Text artistNameText;

    private Vector3 startPosition;

    void Start()
    {
        displayerTransform.localPosition = displayerTransform.localPosition + (Vector3.left * 1000f);
        startPosition = displayerTransform.localPosition;
        displayerTransform.gameObject.SetActive(false);
    }

    private Sequence DisplaySequence;
    public void DisplaySongName(string songName, string artistName)
    {
        displayerTransform.gameObject.SetActive(true);
        songNameText.text = songName;
        artistNameText.text = artistName;

        DisplaySequence = DOTween.Sequence();

        DisplaySequence.Append(displayerTransform.DOLocalMove(stopPosition.localPosition, 1f));
        DisplaySequence.AppendInterval(3f);
        DisplaySequence.Append(displayerTransform.DOLocalMove(startPosition, 1f));
        DisplaySequence.AppendCallback(StopDisplay);
    }
    public void ForcecStopDisplay()
    {
        if(DisplaySequence.IsActive())
        {
            DisplaySequence?.Kill();
            DisplaySequence = DOTween.Sequence();

            DisplaySequence.Append(displayerTransform.DOLocalMove(startPosition, 1f));
            DisplaySequence.AppendCallback(StopDisplay);
        }
    }
    void StopDisplay()
    {
        displayerTransform.gameObject.SetActive(false);
    }
}
